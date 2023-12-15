using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[CreateAssetMenu(fileName = "GridData", menuName = "Duck/GridData"), Serializable]
public class GridData : ScriptableObject
{
    public int ID;

    public int Width;
    public int Height;

    public GridEffect[,] Effects
    {
        get
        {
            if(effects.Length == 0) AfterDeserializeGridEffects();
            return effects;
        }
        set { effects = value; }
    }

    public GridEffect[,] effects = new GridEffect[0, 0];

    public List<PieceOnStart> StartingGamePieces = new List<PieceOnStart>();

    [SerializeField, HideInInspector] private List<Field<int>> serializableField;
    // A package to store our stuff
    [System.Serializable]
    struct Field<TElement>
    {
        public int IndexX;
        public int IndexY;
        public TElement Element;
        public Field(int idx0, int idx1, TElement element)
        {
            IndexX = idx0;
            IndexY = idx1;
            Element = element;
        }
    }

    public void BeforeSerializeGridEffects()
    {
        // Convert our unserializable array into a serializable list
        serializableField = new List<Field<int>>();
        for (int i = 0; i < Effects.GetLength(0); i++)
        {
            for (int j = 0; j < Effects.GetLength(1); j++)
            {
                serializableField.Add(new Field<int>(i, j,(int) Effects[i, j]));
                Debug.Log("ELO!");
            }
        }

    }
    public void AfterDeserializeGridEffects()
    {
        // Convert the serializable list into our unserializable array
        Effects = new GridEffect[Width, Height];
        foreach (var package in serializableField)
        {
            Effects[package.IndexX, package.IndexY] = (GridEffect) package.Element;
            Debug.Log("Deserialize " + package.Element);
        }
    }

    [System.Serializable]
    public struct PieceOnStart
    {
        public int IndexX;
        public int IndexY;
        public GamePieceSO gamePiece;
        public int rotation;

        public PieceOnStart(int idx0, int idx1, GamePieceSO element, int rotation)
        {
            IndexX = idx0;
            IndexY = idx1;
            gamePiece = element;
            this.rotation = rotation;
        }
    }
}
