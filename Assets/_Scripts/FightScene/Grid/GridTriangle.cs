using Assets.Scripts;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GridTriangle : MonoBehaviour, IDropHandler
{
    [SerializeField] int _xIndex;
    [SerializeField] int _yIndex;
    [SerializeField] Transform placementPoint;

    public List<PowerType> VerticalSide;
    public List<PowerType> LeftSide;
    public List<PowerType> RightSide;

    [SerializeField] bool reversed;

    [SerializeField] GamePiece attachedPiece;

    [SerializeField] GridTriangle[] neighbours;

    [SerializeField] Image triangleImage;

    [SerializeField] CellEffect specialEffect;

    public bool IsReversed { get { return reversed; } }

    public Transform PlacementPoint { get => placementPoint; set => placementPoint = value; }
    public int XIndex { get => _xIndex; set => _xIndex = value; }
    public int YIndex { get => _yIndex; set => _yIndex = value; }
    public GamePiece AttachedPiece { get => attachedPiece; set => attachedPiece = value; }
    public GridTriangle[] Neighbours { get => neighbours; private set => neighbours = value; }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetTriangle(int x, int y)
    {
        XIndex = x;
        YIndex = y;

        gameObject.name = $"gridTriangle[{x} {y}]";

        //DEBUG
        if (gameObject.GetComponent<DebugGridTriangle>() != null)
        {
            gameObject.GetComponent<DebugGridTriangle>().ChangeTexts(x, y);
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("Dropped on me! " + XIndex + " " + YIndex);

        if (PlacingManager.Instance.IsTriangleHolded() && attachedPiece == null)
        {
            Debug.Log("Attaching to grid");
            PlacingManager.Instance.PlaceTriangleOn(gameObject);
        }

        else
        {
            PlacingManager.Instance.MoveToOriginPos();
        }


    }

    internal void SetSides(GamePiece triangleGamePiece)
    {
        VerticalSide = triangleGamePiece.side0;
        LeftSide = triangleGamePiece.side1;
        RightSide = triangleGamePiece.side2;
    }

    public void SetNewNeighbours()
    {
        neighbours = new GridTriangle[3];
    }

    public void SetNeighbour(int number, GridTriangle triangle)
    {
        //Debug.Log(triangle.name);
        neighbours[number] = triangle;
    }

    public bool CheckIfHasActiveNeighbour()
    {
        for (int x = 0; x < neighbours.Length; x++)
        {
            if (neighbours[x] != null)
            {
                if (neighbours[x].attachedPiece != null) return true;
            }
        }
        return false;
    }

    public void ClearCell()
    {
        VerticalSide.Clear();
        LeftSide.Clear();
        RightSide.Clear();
        attachedPiece = null;
    }

    public void ChangeTriangleMaterial(Material material)
    {
        
        triangleImage.material = material;
    }
}
