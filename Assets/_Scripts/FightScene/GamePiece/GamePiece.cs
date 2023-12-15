using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class GamePiece : MonoBehaviour
{
    [SerializeField] int id;

    public RectTransform _rect;

    public bool reversed;

    public List<PowerType> side0;
    public List<PowerType> side1;
    public List<PowerType> side2;
    public GridTriangle PlacedTriangle { get => placedTriangle; set => placedTriangle = value; }
    public Vector3 PreviousPosition { get => previousPosition; set => previousPosition = value; }
    public int Id { get => id; set => id = value; }
    public List<CardEffect> Effects { get => effects; set => effects = value; }

    [SerializeField] GridTriangle placedTriangle;

    [SerializeField] Vector3 previousPosition;

    public bool IMMOVABLE;

    public int handID;

    [SerializeField] List<CardEffect> effects;

    private void Awake()
    {
        if(side0 == null) side0 = new List<PowerType>();
        if (side1 == null) side1 = new List<PowerType>();
        if (side2 == null) side2 = new List<PowerType>();

        Debug.Log("Take rect");
        _rect = GetComponent<RectTransform>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PrepareNewLists()
    {
        side0 = new List<PowerType>();
        side1 = new List<PowerType>();
        side2 = new List<PowerType>();
    } 

    public void ReverseTriangle()
    {
        reversed = !reversed;

        RotateLR();

        _rect.eulerAngles += new Vector3(
            _rect.eulerAngles.x, 
            _rect.eulerAngles.y, 
            180 * ReverseModifier());
    }

    int ReverseModifier()
    {
        if (reversed) return 1;
        else return -1;
    }

    public void Rotate(bool clockwise)
    {
        if (reversed == clockwise) Rotate120();
        else Rotate201();
        RotateTriangle(clockwise);
    }

    internal void SetSides(List<PowerType> verticalSide, List<PowerType> leftSide, List<PowerType> rightSide)
    {
        side0 = verticalSide;
        side1 = leftSide;
        side2 = rightSide;
    }

    public void RotateTriangle(bool clockwise)
    {
        if (reversed == clockwise)
        {
            _rect.eulerAngles = new Vector3(
                    _rect.eulerAngles.x,
                    _rect.eulerAngles.y,
                    _rect.eulerAngles.z + (-120 * ReverseModifier()));
        }
        else
        {
            _rect.eulerAngles = new Vector3(
                   _rect.eulerAngles.x,
                   _rect.eulerAngles.y,
                   _rect.eulerAngles.z + (120 * ReverseModifier()));
        }

    }


    //For basic triangle(/\) it's counterclockwise
    public void Rotate120()
    {
        List<PowerType> temp = side0;
        side0 = side1;
        side1 = side2;
        side2 = temp;
    }

    //For basic triangle(/\) it's clockwise
    public void Rotate201()
    {
        List<PowerType> temp = side0;
        side0 = side2;
        side2 = side1;
        side1 = temp;
    }

    public void RotateLR()
    {
        List<PowerType> temp = side1;
        side1 = side2;
        side2 = temp;
    }

    public List<PowerType> getSideById(int id)
    {
        switch (id)
        {
            case 0:
                return side0;
            case 1:
                return side1;
            case 2:
                return side2;
            default:
                return null;
        }
    }

    PowerType[] GetSamePower(List<PowerType> side, List<PowerType> neighbourSide)
    {
        return side.Intersect(neighbourSide).ToArray();
    }

    public void DebugSides()
    {
        Debug.Log("Gamepiece " + name);
        foreach (var item in side0)
        {
            Debug.Log("Side 0 " + item);
        }
        foreach (var item in side1)
        {
            Debug.Log("Side 1 " + item);
        }
        foreach (var item in side2)
        {
            Debug.Log("Side 2 " + item);
        }
    }

    #region Card Effects



    #endregion
}
