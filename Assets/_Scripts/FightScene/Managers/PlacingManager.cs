using Assets._Scripts.Enum;
using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public struct PowerAmount
{
    public PowerType powerType;
    public int Amount;
}

public class placeFinderValues
{
    public bool Found;
    public int SumScore;
    public int Rotation;
    public PositionGrid PosGrid;
    public Dictionary<PowerType, int> Score;
    public GamePiece Piece;

    public placeFinderValues()
    {
        Found = false;
    }

    public placeFinderValues(bool found, int sumScore, int rotation, PositionGrid posGrid, Dictionary<PowerType, int> score, GamePiece piece)
    {
        Found = found;
        SumScore = sumScore;
        Rotation = rotation;
        PosGrid = posGrid;
        Score = score;
        Piece = piece;
    }
}

public class PlacingManager : Singleton<PlacingManager>
{
    Triangles input;

    [SerializeField] Canvas triangleCanvas;

    public GameObject TriangleToCarry;
    [SerializeField] GamePiece triangleGamePiece;
    [SerializeField] RectTransform triangleTransform;
    //public Vector3 originPos;

    [SerializeField] GridGame grid;
    [SerializeField] GameHand playerHand;
    [SerializeField] GameHand enemyHand;
    [SerializeField] ScoreManager scoreManager;

    public static Action<GameObject> Add;
    public static Action Remove;
    public static Action<GameObject> PlaceHere;

    public static Action showAvailable;

    [SerializeField] Material defaultGridMaterial;
    [SerializeField] Material showGridMaterial;
    [SerializeField] bool DEBUG_NOT_CHECK_NEIGHBOUR;

    public override void Awake()
    {
        base.Awake();

        Add += AddTriangle;
        Remove += RemoveTriangle;
        PlaceHere += PlaceTriangleOn;
        showAvailable += showAvailableSpots;

        input = new Triangles();

        input.Puzzle.RotateClockwise.performed += RotateClockwise;
        input.Puzzle.RotateCounterClock.performed += RotateCounterClock;
        input.Puzzle.Reverse.performed += Reverse;

        input.Puzzle.ScrollWheel.performed += RotateScroll;
    }



    // Start is called before the first frame update
    void Start()
    {
        if (playerHand == null) FindObjectOfType<GameHand>();
        input.Enable();
    }

    // Update is called once per frame
    void Update()
    {
    }

    #region Placing

    internal void MoveToOriginPos()
    {
        if (TriangleToCarry == null) return;
        StartCoroutine(MoveToPlace(TriangleToCarry, TriangleToCarry.GetComponent<GamePiece>().handPosition));


    }

    public IEnumerator MoveToPlace(GameObject ObjectToCarry, Vector3 positionToGo)
    {
        if(positionToGo == Vector3.zero)
        {
            Debug.Log("Empty positionToGo - getting handPosition");
            positionToGo = ObjectToCarry.GetComponent<GamePiece>().handPosition;
        }

        //Debug.Log("position to go " + positionToGo);
        //Debug.Log("Triangle position " + ObjectToCarry.transform.position);
        while (Vector3.Distance(ObjectToCarry.transform.position, positionToGo) > 0.1f)
        {
            Debug.Log("Going back to hand");
            ObjectToCarry.transform.position = Vector3.Lerp(ObjectToCarry.transform.position, positionToGo, 0.1f);
            yield return new WaitForSeconds(0.01f);
        }

        yield return null;
    }

    public IEnumerator MoveToPlaceAndDeploy(GameObject ObjectToCarry, Vector3 positionToGo)
    {
        if (positionToGo == Vector3.zero)
        {
            Debug.Log("Empty positionToGo - getting handPosition");
            positionToGo = ObjectToCarry.GetComponent<GamePiece>().handPosition;
        }

        Debug.Log("position to go " + positionToGo);
        Debug.Log("Triangle position " + ObjectToCarry.transform.position);
        while (Vector3.Distance(ObjectToCarry.transform.position, positionToGo) > 0.1f)
        {
            Debug.Log("Going back to hand");
            ObjectToCarry.transform.position = Vector3.Lerp(ObjectToCarry.transform.position, positionToGo, 0.1f);
            yield return new WaitForSeconds(0.01f);
        }

        yield return null;
    }

    internal bool IsTriangleHolded()
    {
        if (TriangleToCarry != null && triangleGamePiece != null) return true;
        else return false;
    }

    public void PlaceTriangleOn(GameObject gridCell)
    {
        PlaceTriangleToGridCell(gridCell, TriangleToCarry, 0, true, CurrentPointReceiver.Player);

        //RoundManager.EndPlayerMove();
        //RemoveTriangle();
    }

    private void AttachPieceToGrid(GameObject gamePiece, GridTriangle gridTriangle, GamePiece piece)
    {
        Vector3 correctedPosition = new Vector3(
            gridTriangle.PlacementPoint.position.x, gridTriangle.PlacementPoint.position.y, 0);
        gamePiece.gameObject.GetComponent<RectTransform>().position = correctedPosition;


        if (piece.reversed != gridTriangle.IsReversed)
        {
            piece.ReverseTriangle(); 
        }

        if (piece.PlacedTriangle != null) piece.PlacedTriangle.ClearCell();
        piece.PlacedTriangle = gridTriangle;
        gridTriangle.SetSides(piece);
        gridTriangle.AttachedPiece = piece;
        piece.IMMOVABLE = true;

        //AVAILABLE PART

        grid.RemoveAvailable(gridTriangle.XIndex, gridTriangle.YIndex);

        foreach (GridTriangle triangle in gridTriangle.Neighbours.Where(x => x != null))
        {
            if (triangle.AttachedPiece == null)
            {
                if (grid.AvailablePositions.Any(z => z.x == triangle.XIndex && z.y == triangle.YIndex))
                {
                    UpdateAvailableSides(triangle, gridTriangle, 
                        grid.AvailablePositions.First(z => z.x == triangle.XIndex && z.y == triangle.YIndex));
                }
                else
                {
                    var newAvailable = new PositionGrid(triangle.XIndex, triangle.YIndex);
                    UpdateAvailableSides(triangle, gridTriangle, newAvailable);
                    grid.AddAvailable(newAvailable);
                }
            }
        }

        RemoveTriangle();
    }

    public void DEBUG_PlaceTriangleToGridCell(GameObject gridCell, GameObject gamePiece, int rotate = 0, bool checkNeighbours = true, CurrentPointReceiver playerMove = CurrentPointReceiver.Neutral)
    {
        GridTriangle gridTriangle = gridCell.GetComponent<GridTriangle>();
        GamePiece piece = gamePiece.GetComponent<GamePiece>();

        //var NeighbourResult = CheckGridCellDELUXE(gridTriangle, piece);

        AttachPieceToGrid(gamePiece, gridTriangle, piece);
        //GivePointsTo(playerMove, piece, NeighbourResult);


        if (piece.Effects != null)
        {
            SpecialEffectManager.USE_ALL_EFFECTS(piece.Effects);
        }
    }

    public void PlaceEnemyTriangle(placeFinderValues placeFinder,
        GameObject gamePiece,
        int rotate = 0,
        bool checkNeighbours = true,
        CurrentPointReceiver player = CurrentPointReceiver.Neutral)
    {
        GameObject gridCell = grid.GridCore[placeFinder.PosGrid.x, placeFinder.PosGrid.y];
        GridTriangle gridTriangle = gridCell.GetComponent<GridTriangle>();
        GamePiece piece = gamePiece.GetComponent<GamePiece>();

        //DEBUG
        if (DEBUG_NOT_CHECK_NEIGHBOUR)
        {
            DEBUG_PlaceTriangleToGridCell(gridCell, gamePiece, rotate, checkNeighbours, player);
            return;
        }

            if (!gridTriangle.CheckIfHasActiveNeighbour() && checkNeighbours)
            {
                MoveToOriginPos();
                return;
            }

            GivePointsToEnemy(piece, ConventToPointList(placeFinder.Score));
            AttachPieceToGrid(gamePiece, gridTriangle, piece);
        }

    private List<PowerType> ConventToPointList(Dictionary<PowerType, int> score)
    {
        List<PowerType> result = new List<PowerType>();
        foreach(var x in score)
        {
            for (int i = 0; i < x.Value; i++)
            {
                result.Add(x.Key);
            }
        }
        return result;
    }

    public void PlaceTriangleToGridCell(
        GameObject gridCell, 
        GameObject gamePiece, 
        int rotate = 0, 
        bool checkNeighbours = true, 
        CurrentPointReceiver player = CurrentPointReceiver.Neutral)
    {
        //DEBUG
        if (DEBUG_NOT_CHECK_NEIGHBOUR)
        {
            DEBUG_PlaceTriangleToGridCell(gridCell, gamePiece, rotate, checkNeighbours, player);
            return;
        }

        GridTriangle gridTriangle = gridCell.GetComponent<GridTriangle>();
        GamePiece piece = gamePiece.GetComponent<GamePiece>();
        
        if (!gridTriangle.CheckIfHasActiveNeighbour() && checkNeighbours)
        {
            MoveToOriginPos();
            return;
        }

        if (checkNeighbours)
        {
            var NeighbourResult = CheckGridCellDELUXE(gridTriangle, piece);

            if (NeighbourResult.Item1)
            {
                AttachPieceToGrid(gamePiece, gridTriangle, piece);
                GivePointsTo(player, piece, NeighbourResult);

                if (piece.Effects != null)
                {
                    SpecialEffectManager.USE_ALL_EFFECTS(piece.Effects);
                }
            }
            else
            {
                Debug.Log($"Can't place it here! {piece.name} {gridTriangle.XIndex} {gridTriangle.YIndex}");
                if (TriangleToCarry != null) MoveToOriginPos();
                piece.IMMOVABLE = false;
            }
        }
        else
        {
            AttachPieceToGrid(gamePiece, gridTriangle, piece);
            //RoundManager.EndPlayerMove();
        }

    }


    #endregion

    #region Triangle Manipulation
    public void AddTriangle(GameObject triangle)
    {
        TriangleToCarry = triangle;
        triangleGamePiece = triangle.GetComponent<GamePiece>();
        triangleTransform = triangle.GetComponent<RectTransform>();

        showAvailableSpots();
    }

    public void RemoveTriangle()
    {
        TriangleToCarry = null;
        triangleGamePiece = null;
        triangleTransform = null;
    }

    void Reverse(InputAction.CallbackContext context)
    {
        if (TriangleToCarry != null && triangleGamePiece != null)
        {
            triangleGamePiece.ReverseTriangle();
        }

    }

    void RotateClockwise(InputAction.CallbackContext context)
    {
        if (TriangleToCarry != null && triangleGamePiece != null)
        {
            triangleGamePiece.Rotate(true);
        }
    }

    void RotateCounterClock(InputAction.CallbackContext context)
    {
        if (TriangleToCarry != null && triangleGamePiece != null)
        {
            triangleGamePiece.Rotate(false);
        }
    }

    void RotateScroll(InputAction.CallbackContext context)
    {
        //Debug.Log("scroll " + context.ReadValue<Vector2>());
    }

    #endregion

    #region Points

    Dictionary<PowerType, int> CountAllPowers(List<PowerType> powers1, List<PowerType> powers2, List<PowerType> powers3)
    {
        var resultDict = new Dictionary<PowerType, int>();

        resultDict.Add(PowerType.CHARM, CountPower(PowerType.CHARM, powers1, powers2, powers3));
        resultDict.Add(PowerType.AUTHORITY, CountPower(PowerType.AUTHORITY, powers1, powers2, powers3));
        resultDict.Add(PowerType.AGGRESSION, CountPower(PowerType.AGGRESSION, powers1, powers2, powers3));
        resultDict.Add(PowerType.LOGIC, CountPower(PowerType.LOGIC, powers1, powers2, powers3));

        return resultDict;
    }

    int CountPower(PowerType powerType,
        List<PowerType> list,
        List<PowerType> list2 = null,
        List<PowerType> list3 = null)
    {
        int count = 0;
        if (list != null) count += list.Count(x => x == powerType);
        if (list2 != null) count += list2.Count(x => x == powerType);
        if (list3 != null) count += list3.Count(x => x == powerType);
        return count;
    }

    private void GivePointsToEnemy(GamePiece piece, List<PowerType> result)
    {
        scoreManager.EnemyScore.AddPoints(result);
        if (piece.handID >= 0)
        {
            enemyHand.RemoveCard(piece.handID);
            piece.handID = -69;
        }
    }

    private void GivePointsTo(CurrentPointReceiver player, GamePiece piece, (bool, Dictionary<int, List<PowerType>>) NeighbourResult)
    {
        if (player == CurrentPointReceiver.Player)
        {
            scoreManager.PlayerScore.AddScore(NeighbourResult.Item2);
            if (piece.handID >= 0)
            {
                playerHand.RemoveCard(piece.handID);
                piece.handID = -69;
            }
            RoundManager.EndPlayerMove();
        }
/*
        else if(player == CurrentPointReceiver.Enemy)
        {
            Debug.Log("Enemy score");
            foreach (var x in NeighbourResult.Item2)
            {
                Debug.Log(x.Key + " " + x.Value.Count);
                foreach (var y in x.Value)
                {
                    Debug.Log(y);
                }
            }
            scoreManager.EnemyScore.AddScore(NeighbourResult.Item2);

            if (piece.handID >= 0)
            {
                enemyHand.RemoveCard(piece.handID);
                piece.handID = -69;
            }
        }*/
    }


    #endregion

    #region Grid Recognition

    GridTriangle GetNeighbour(int sideNo, GridTriangle gridTriangle)
    {
        switch (sideNo)
        {
            case 0:
                if (gridTriangle.IsReversed)
                {
                    if (gridTriangle.YIndex - 1 >= 0)
                        return grid.GridCore[gridTriangle.XIndex, gridTriangle.YIndex - 1].GetComponent<GridTriangle>();
                    break;
                }
                else
                {
                    if (gridTriangle.YIndex + 1 < grid.Height)
                        return grid.GridCore[gridTriangle.XIndex, gridTriangle.YIndex + 1].GetComponent<GridTriangle>();
                    break;
                }
            case 1:
                if (gridTriangle.XIndex - 1 >= 0)
                {
                    return grid.GridCore[gridTriangle.XIndex - 1, gridTriangle.YIndex].GetComponent<GridTriangle>();
                }
                break;
            case 2:
                if (gridTriangle.XIndex + 1 < grid.Width)
                {
                    return grid.GridCore[gridTriangle.XIndex + 1, gridTriangle.YIndex].GetComponent<GridTriangle>();
                }
                break;
        }
        return null;
    }

    public void AddToAvailable(PositionGrid pg)
    {
        grid.AvailablePositions.Add(pg);
    }

    private void showAvailableSpots()
    {
        StartCoroutine(ShowAvailable());

        IEnumerator ShowAvailable()
        {
            void changeMaterial(Material material)
            {
                foreach (PositionGrid posGrid in grid.AvailablePositions)
                {
                    grid.GridTriangles[posGrid.x, posGrid.y].ChangeTriangleMaterial(material);
                }
            }

            changeMaterial(showGridMaterial);

            while (TriangleToCarry != null)
            {
                yield return new WaitForSeconds(0.5f);
            }

            changeMaterial(defaultGridMaterial);

            yield return null;
        }
    }

    (bool, Dictionary<int, List<PowerType>>) CheckGridCellDELUXE(GridTriangle gridTriangle, GamePiece gamePiece)
    {
        Dictionary<int, List<PowerType>> correlations = new Dictionary<int, List<PowerType>>();
        List<PowerType> sideToCheck, NsideToCheck;
        GridTriangle neighbour;
        int correlationCounter = 0;
        bool blankToPut = false;

        for (int i = 0; i <= 2; i++)
        {
            neighbour = GetNeighbour(i, gridTriangle);
            Debug.Log("Neighbour " + i + " " + neighbour);
            if (neighbour != null)
            {
                sideToCheck = gamePiece.getSideById(i);
                NsideToCheck = GetSideToCheck(i, neighbour);

                Debug.Log("Side to check " + i + " " + sideToCheck.FirstOrDefault() + " " + NsideToCheck.FirstOrDefault());

                if (NsideToCheck.Count == 0) continue;

                if (NsideToCheck.Contains(PowerType.XXX))
                {
                    return (false, correlations);
                }

                if (sideToCheck.Count == 0)
                {
                    if (neighbour.AttachedPiece != null)
                    {
                        blankToPut = true;
                    }
                    correlations.Add(i, new List<PowerType>());
                    Debug.Log("Blank GamePiece side");
                    continue;
                }

                //IF either side has a BLANK
                if (NsideToCheck.Count == 0)
                {
                    correlations.Add(i, new List<PowerType>());
                    Debug.Log("Blank on Grid side");
                    continue;
                }

                List<PowerType> correlatedperSide = GetCorrelatePowers(sideToCheck, NsideToCheck);
                Debug.Log($"corr of {i} in power of "  + correlatedperSide.Count);

                if (correlatedperSide.Count > 0)
                {
                    correlations.Add(i, correlatedperSide);
                }
                else
                {
                    Debug.Log("No correlations");
                    return (false, correlations);
                }
                correlationCounter += correlatedperSide.Count;
            }
            else Debug.Log("Empty Neighbour " + i);
        }
        if (correlations.Count == 0 && blankToPut)
        {
            Debug.Log("No correlations and no blank");
            return (false, correlations);
        }
        foreach (var x in correlations)
        {
            Debug.Log(x.Key + " " + x.Value.Count);
            foreach (var y in x.Value)
            {
                Debug.Log(y);
            }
        }
        return (true, correlations);
    }

    public placeFinderValues GetBestCell(GamePiece[] cards)
    {
        placeFinderValues bestPlacement = new placeFinderValues();
        List<placeFinderValues> allResults = new List<placeFinderValues>();

        void CheckAndCompare(int toRotate, PositionGrid positionGrid, GamePiece gamePiece)
        {
            GameObject temp = new GameObject();
            temp.AddComponent<GamePiece>();
            var tempGP = temp.GetComponent<GamePiece>();

            tempGP.side0.AddRange(gamePiece.side0);
            tempGP.side1.AddRange(gamePiece.side1);
            tempGP.side2.AddRange(gamePiece.side2);

            //Rotation part
            if (grid.GridCore[positionGrid.x, positionGrid.y].GetComponent<GridTriangle>().IsReversed)
            {
                tempGP.RotateLR();
                for (int i = 0; i < toRotate; i++) tempGP.Rotate120();

            }
            else
            {
                for (int i = 0; i < toRotate; i++) tempGP.Rotate201();
            }

            //gamePiece.DebugSides();

            var result = CheckAvailable(positionGrid, tempGP);
            if (result.Item1)
            {
                allResults.Add(new placeFinderValues(true, result.Item2, toRotate, positionGrid, result.Item3, gamePiece));

                if (bestPlacement.SumScore == 0 && result.Item2 == 0)
                {
                    bestPlacement = new placeFinderValues(true, result.Item2, toRotate, positionGrid, result.Item3, gamePiece);
                }
                if (result.Item2 > bestPlacement.SumScore)
                {
                    //Debug.Log($"Setting on {positionGrid.x} {positionGrid.y} gp {gamePiece.name} with score {result.Item2} and rotation {toRotate}");
                    bestPlacement = new placeFinderValues(true, result.Item2, toRotate, positionGrid, result.Item3, gamePiece);
                }
            }
            Destroy(temp);
        }

        if (cards.Length == 0) return new placeFinderValues();

        foreach (GamePiece card in cards.Where(x => x != null))
        {
            foreach (PositionGrid positionGrid in grid.AvailablePositions)
            {
                CheckAndCompare(0, positionGrid, card);
                CheckAndCompare(1, positionGrid, card);
                CheckAndCompare(2, positionGrid, card);
            }
        }

        if (bestPlacement.Found)
        {
            return bestPlacement;
        }

        return new placeFinderValues();
    }

    public List<placeFinderValues> GetBestCell2(GamePiece[] cards)
    {
        List<placeFinderValues> allResults = new List<placeFinderValues>();

        void CheckAndCompare(int toRotate, PositionGrid positionGrid, GamePiece gamePiece)
        {
            GameObject temp = new GameObject();
            temp.AddComponent<GamePiece>();
            var tempGP = temp.GetComponent<GamePiece>();

            tempGP.side0.AddRange(gamePiece.side0);
            tempGP.side1.AddRange(gamePiece.side1);
            tempGP.side2.AddRange(gamePiece.side2);

            //Rotation part
            if (grid.GridCore[positionGrid.x, positionGrid.y].GetComponent<GridTriangle>().IsReversed)
            {
                tempGP.RotateLR();
                for (int i = 0; i < toRotate; i++) tempGP.Rotate120();

            }
            else
            {
                for (int i = 0; i < toRotate; i++) tempGP.Rotate201();
            }

            //gamePiece.DebugSides();

            var result = CheckAvailable(positionGrid, tempGP);
            if (result.Item1)
            {
                allResults.Add(new placeFinderValues(true, result.Item2, toRotate, positionGrid, result.Item3, gamePiece));
            }
            Destroy(temp);
        }

        //If no cards are available return empty list
        if (cards.Length == 0) return new List<placeFinderValues>();

        foreach (GamePiece card in cards.Where(x => x != null))
        {
            foreach (PositionGrid positionGrid in grid.AvailablePositions)
            {
                CheckAndCompare(0, positionGrid, card);
                CheckAndCompare(1, positionGrid, card);
                CheckAndCompare(2, positionGrid, card);
            }
        }

        return allResults;
    }


    public (bool, int, Dictionary<PowerType, int>) CheckAvailable(PositionGrid positionGrid, GamePiece gamePiece)
    {
        var result0 = CompareSides(positionGrid.side0Powers, gamePiece.side0);
        var result1 = CompareSides(positionGrid.side1Powers, gamePiece.side1);
        var result2 = CompareSides(positionGrid.side2Powers, gamePiece.side2);

        int resultsValue = 0;
        if (result0.Item2 != null) { resultsValue += result0.Item2.Count(); }
        if (result1.Item2 != null) { resultsValue += result1.Item2.Count(); }
        if (result2.Item2 != null) { resultsValue += result2.Item2.Count(); }

        //Debug.Log("value = " + resultsValue);

        if (result0.Item1 && result1.Item1 && result2.Item1)
        {
            var score = CountAllPowers(result0.Item2, result1.Item2, result2.Item2);

            return (true, resultsValue, score);
        }
        else
        {
            return (false, resultsValue, null);
        }
    }

    void UpdateAvailableSides(GridTriangle currentNeighbour, GridTriangle currentGridCell, PositionGrid newAvailable)
    {
        if (currentNeighbour.XIndex < currentGridCell.XIndex)
        {
            newAvailable.side2Powers.AddRange(currentGridCell.LeftSide);
            return;
        }
        if (currentNeighbour.XIndex > currentGridCell.XIndex)
        {
            newAvailable.side1Powers.AddRange(currentGridCell.RightSide);
            return;
        }
        if (currentNeighbour.YIndex < currentGridCell.YIndex || currentNeighbour.YIndex > currentGridCell.YIndex)
        {
            newAvailable.side0Powers.AddRange(currentGridCell.VerticalSide);
            return;
        }
    }


    #endregion

    //TESTED
    public (bool, List<PowerType>) CompareSides(List<PowerType> posGridSide, List<PowerType> gamePieceSide)
    {
        if (posGridSide.Contains(PowerType.XXX)) return (false, null);
        if (gamePieceSide.Contains(PowerType.XXX)) return (true, null);
        if (posGridSide.Count == 0 || gamePieceSide.Count == 0) return (true, null);

        List<PowerType> result = new List<PowerType>();

        foreach (PowerType power in posGridSide)
        {
            if (result.Contains(power))
            {
                continue;
            }

            var temp = gamePieceSide.Where(x => x == power);
            if (temp.Count() > 0)
            {
                result.AddRange(posGridSide.FindAll(x => x == power));
                result.AddRange(temp);
            }

        }
        if (result.Count > 0)
        {
            Debug.Log("Found " + result.Count + " powers");
            return (true, result);
        }

        return (false, null);
    }

    
    List<PowerType> GetSideToCheck(int sideNo, GridTriangle gridTriangle)
    {
        switch (sideNo)
        {
            case 0:
                return gridTriangle.VerticalSide;
            case 1:
                return gridTriangle.RightSide;
            case 2:
                return gridTriangle.LeftSide;
            default:
                return new List<PowerType>();
        }
    }

    List<PowerType> GetCorrelatePowers(List<PowerType> side, List<PowerType> neighbourSide)
    {
        return side.Intersect(neighbourSide).ToList();
    }

    public void RevertButton()
    {
        TriangleToCarry.transform.position = triangleGamePiece.PreviousPosition;
        triangleGamePiece.IMMOVABLE = false;
        RemoveTriangle();
    }
}
