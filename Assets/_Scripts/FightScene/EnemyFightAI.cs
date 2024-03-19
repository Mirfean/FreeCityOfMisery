using Assets._Scripts.Enum;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyFightAI : MonoBehaviour
{
    [SerializeField] PlacingManager placingManager;
    [SerializeField] DeckManager deckManager;
    [SerializeField] GridGame grid;


    /// <summary>
    /// Behaviour variables
    /// </summary>
    [SerializeField, Range(0f, 10f)] int smartness;
    [SerializeField, Range(0f, 2f)] int centralized;
    [SerializeField, Range(0f, 3f)] int agressor;


    // Start is called before the first frame update
    void Start()
    {
        if (grid == null) grid = GameObject.FindAnyObjectByType<GridGame>();
        if (deckManager == null) deckManager = GameObject.FindAnyObjectByType<DeckManager>();
        if (placingManager == null) placingManager = GameObject.FindAnyObjectByType<PlacingManager>();



    }

    // Update is called once per frame
    void Update()
    {

    }

    public bool EnemyMove()
    {
        if (deckManager.EnemyHand.Cards.Length > 0)
        {
            List<placeFinderValues> result = placingManager.GetBestCell2(deckManager.EnemyHand.Cards);
            if (result.Count == 0) return false;
            
            Debug.Log("Found grid cell to place");

            placeFinderValues moveToPlay = DecideNextMove(result);

            PlaceEnemyCard(moveToPlay, deckManager.EnemyHand.Cards.
                 Where(x => x != null).
                 First(x => x.Id == moveToPlay.Piece.Id));

                //SpecialEffectManager.USE_ALL_EFFECTS(piece.Effects);

                return true;
        }

        return false;
    }

    private placeFinderValues DecideNextMove(List<placeFinderValues> result)
    {
        var ordered = result.OrderByDescending(x => x.SumScore);

        foreach (var item in ordered)
        {
            Debug.Log($"Card {item.Piece.name} score {item.SumScore} on {item.PosGrid.x} {item.PosGrid.y}");
        }

        //Place to add additional logic to decide the best move


        return ordered.First();
    }

    void PlaceEnemyCard(placeFinderValues placeFinder, GamePiece gamePiece)
    {
        PrepareGamePieceToPlace(placeFinder, gamePiece);

        PlaceEnemyCard(placeFinder, gamePiece.gameObject);

        gamePiece.HIDE = false;

        RemoveCardFromHand(placeFinder);
    }

    private void PrepareGamePieceToPlace(placeFinderValues placeFinder, GamePiece gamePiece)
    {
        //Debug.Log(gamePiece.reversed);

        if (grid.GridTriangles[placeFinder.PosGrid.x, placeFinder.PosGrid.y].IsReversed) gamePiece.ReverseTriangle();
        //gamePiece.DebugSides();

        for (int i = 0; i < placeFinder.Rotation; i++)
        {
            Debug.Log("Rotating");
            gamePiece.Rotate(true);
        }

        //Debug.Log("In the end");
        //gamePiece.DebugSides();
    }

    private void RemoveCardFromHand(placeFinderValues placeFinder)
    {
        for (int i = 0; i < deckManager.EnemyHand.Cards.Length; i++)
        {
            if (deckManager.EnemyHand.Cards[i] != null)
            {
                if (deckManager.EnemyHand.Cards[i] == placeFinder.Piece)
                {
                    deckManager.EnemyHand.Cards[i] = null;
                }
            }
        }
    }

    void PlaceEnemyCard(placeFinderValues placeFinder, GameObject card)
    {
        StartCoroutine(MoveToPlaceAndDeploy(placeFinder, card));
        
    }
    public IEnumerator MoveToPlaceAndDeploy(placeFinderValues placeFinder, GameObject ObjectToCarry)
    {
        Vector3 positionToGo = grid.GridCore[placeFinder.PosGrid.x, placeFinder.PosGrid.y].transform.position;
        
        if (positionToGo == Vector3.zero)
        {
            Debug.Log("Empty positionToGo - getting handPosition");
            positionToGo = ObjectToCarry.GetComponent<GamePiece>().handPosition;
        }

        Debug.Log("position to go " + positionToGo);
        Debug.Log("Triangle position " + ObjectToCarry.transform.position);
        
        while (Vector3.Distance(ObjectToCarry.transform.position, positionToGo) > 0.1f)
        {
            ObjectToCarry.transform.position = Vector3.Lerp(ObjectToCarry.transform.position, positionToGo, 0.1f);
            yield return new WaitForSeconds(0.01f);
        }
        
        PlacingManager.Instance.PlaceEnemyTriangle(placeFinder, ObjectToCarry, 0, true, CurrentPointReceiver.Enemy);
        yield return null;
    }
}
