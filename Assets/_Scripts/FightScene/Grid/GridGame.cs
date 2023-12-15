using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct PositionGrid
{
    public int x;
    public int y;

    public List<PowerType> side0Powers;
    public List<PowerType> side1Powers;
    public List<PowerType> side2Powers;

    public PositionGrid(int x, int y)
    {
        this.x = x;
        this.y = y;

        side0Powers = new List<PowerType>();
        side1Powers = new List<PowerType>();
        side2Powers = new List<PowerType>();
    }

    List<PowerType> GetPowers(int side)
    {
        switch (side)
        {
            case 0: return side0Powers;
            case 1: return side1Powers;
            case 2: return side2Powers;
            default: return new List<PowerType>();
        }
    }

}

public class GridGame : MonoBehaviour
{

    [SerializeField] GameObject gridTri;
    [SerializeField] GameObject gridTriReversed;
    [SerializeField] Transform gridTransform;

    [SerializeField] GridData gridData;

    [SerializeField] int width;
    [SerializeField] int height;

    [SerializeField] GameObject [,] gridCore;
    [SerializeField] GridTriangle [,] gridTriangles;
    [SerializeField] HashSet<PositionGrid> availablePositions;


    [SerializeField] RectTransform triangleToSize;
    [SerializeField, Range(0f,100f)]float gridModifier;

    //TEMPORARY
    public GameObject TRIANGLE;

    public GameObject[,] GridCore { get => gridCore; set => gridCore = value; }
    public GridTriangle[,] GridTriangles { get => gridTriangles; set => gridTriangles = value; }

    public int Width { get => width; set => width = value; }
    public int Height { get => height; set => height = value; }
    public HashSet<PositionGrid> AvailablePositions { get => availablePositions; set => availablePositions = value; }

    // Start is called before the first frame update
    void Start()
    {
        if(gridData != null)
        {
            width = gridData.Width;
            height = gridData.Height;
        }

        GenerateGrid();

        availablePositions = new HashSet<PositionGrid>();

        SpawnStartingTriangles();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void GenerateGrid()
    {
        GridCore = new GameObject[Width, Height];
        gridTriangles = new GridTriangle[Width, Height];

        Vector2 startPoint = gameObject.GetComponent<RectTransform>().anchoredPosition;
        Vector2 triangleSize = new Vector2(100f, 100f);
        
        CreateGridTriangles(startPoint, triangleSize);

        PlaceGridEffects();

        SetNeighboursForGridTriangles();
    }

    private void PlaceGridEffects()
    {
        if (gridData == null) return;

        for(int i = 0; i < gridData.Width; i++)
        {
            for(int j = 0; j < gridData.Height; j++)
            {
                switch (gridData.Effects[i, j])
                {
                    case GridEffect.EMPTY:
                        continue;
                    case GridEffect.DISABLED:
                        GridCore[i, j].SetActive(false);
                        break;
                }
                
            }
        }
        
    }

    private void CreateGridTriangles(Vector2 startPoint, Vector2 triangleSize)
    {
        GameObject triangle;
        for (int i = 0; i < Width; i++)
        {
            for (int j = 0; j < Height; j++)
            {
                if (RotateTriangle(i, j))
                {
                    triangle = Instantiate(gridTriReversed);
                }
                else
                {
                    triangle = Instantiate(gridTri);
                }

                triangle.transform.SetParent(gridTransform);
                triangle.GetComponent<RectTransform>().anchoredPosition = new Vector3(startPoint.x + 0.5f * i * triangleSize.x + gridModifier * i,
                    startPoint.y - (Mathf.Sqrt(3) / 2) * j * triangleSize.y - gridModifier * j * 0.5f,
                    50);

                triangle.transform.localScale = Vector3.one;
                GridCore[i, j] = triangle;
                gridTriangles[i, j] = GridCore[i, j].GetComponent<GridTriangle>();

                gridTriangles[i, j].SetTriangle(i, j);


            }
        }
    }

    private void SetNeighboursForGridTriangles()
    {
        Debug.Log("grid size " +gridTriangles.Length);
        foreach (GridTriangle gridTriangle in gridTriangles)
        {
            if (gridTriangle == null) continue;

            if(gridTriangle.Neighbours.Length == 0)
            {
                gridTriangle.SetNewNeighbours();
                //Debug.Log("Setting new neighbours for " + gridTriangle.XIndex + gridTriangle.YIndex);
            }

            if (gridTriangle.IsReversed && gridTriangle.YIndex + 1 < height)
            {
                gridTriangle.SetNeighbour(0, gridTriangles[gridTriangle.XIndex, gridTriangle.YIndex + 1]);
            }
            else if (!gridTriangle.IsReversed && gridTriangle.YIndex - 1 >= 0)
            {
                gridTriangle.SetNeighbour(0, gridTriangles[gridTriangle.XIndex, gridTriangle.YIndex - 1]);
            }
            if (gridTriangle.XIndex - 1 >= 0)
            {
                gridTriangle.SetNeighbour(1, gridTriangles[gridTriangle.XIndex - 1, gridTriangle.YIndex]);
            }
            if (gridTriangle.XIndex + 1 < width)
            {
                gridTriangle.SetNeighbour(2, gridTriangles[gridTriangle.XIndex + 1, gridTriangle.YIndex]);
            }
        }
    }



    bool RotateTriangle(int i, int j)
    {
        bool x = i % 2 == 0;
        bool y = j % 2 == 0;

        if(x == y) return false;
        return true;
    }

    void SpawnStartingTriangles()
    {
        if (gridData.StartingGamePieces == null) return;
        foreach(var triangle in gridData.StartingGamePieces)
        {
            SpawnAndRotateStartingTriangle(triangle.IndexX, triangle.IndexY, triangle.gamePiece, triangle.rotation);        
        }
    }

    void SpawnAndRotateStartingTriangle(int x, int y, GamePieceSO gpSO, int rotate)
    {
        //Make triangle
        GameObject newGamePiece = Instantiate(TRIANGLE);
        //Set triangle


        newGamePiece.transform.SetParent(transform, false);

        //Debug.Log("Broke X" + x + " Broke Y" + y);
        Debug.Log(newGamePiece.GetComponent<GamePiece>()._rect.name);

        PlacingManager.Instance.PlaceTriangleToGridCell(GridCore[x, y], newGamePiece, rotate, false);
    }

    public void AddAvailable(int x, int y)
    {
        AvailablePositions.Add(new PositionGrid(x, y));
    }

    public void AddAvailable(PositionGrid positionGrid)
    {
        AvailablePositions.Add(positionGrid);
    }

    public void RemoveAvailable(int x, int y)
    {
        PositionGrid Find(int x, int y)
        {
            foreach (PositionGrid grid in availablePositions)
            {
                if (grid.x == x && grid.y == y) return grid;
            }
            return new PositionGrid(-1, -1);
        }

        PositionGrid temp = Find(x, y);
        if(temp.x >=0 && temp.y >= 0)
        {
            AvailablePositions.Remove(temp);
        }

    }

}
