using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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
    [SerializeField, Range(0f,3f)] int agressor;


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
        if(deckManager.EnemyHand.Cards.Length > 0)
        {
            placeFinderValues result = placingManager.GetBestCell(deckManager.EnemyHand.Cards);
            if (result.Found)
            {
                Debug.Log("Found grid cell to place");
                GamePiece piece = deckManager.EnemyHand.Cards.
                    Where(x => x != null).
                    First(x => x.Id == result.Piece.Id);

                PlaceEnemyCard(result, deckManager.EnemyHand.Cards.
                    Where(x => x != null).
                    First(x => x.Id == result.Piece.Id));

                //SpecialEffectManager.USE_ALL_EFFECTS(piece.Effects);

                return true;
                
            }
            return false;
        }

        return false;
    }

    void PlaceEnemyCard(placeFinderValues placeFinder, GamePiece gamePiece)
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

        PlaceEnemyCard(placeFinder.PosGrid.x, placeFinder.PosGrid.y, gamePiece.gameObject);

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

    void PlaceEnemyCard(int xGrid, int yGrid, GameObject card)
    {
        PlacingManager.Instance.PlaceTriangleToGridCell(grid.GridCore[xGrid, yGrid], card, 0, true, false);
    }

}
