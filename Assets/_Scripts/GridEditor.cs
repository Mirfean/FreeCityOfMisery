using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GridEditor : EditorWindow
{
    GridData _gridData;

    int _id;

    int _width = 1;
    int _height = 1;

    GridEffect[,] _fieldsArray = new GridEffect[0, 0];

    int _combinableSize;
    public string[] _secondItem = new string[0];
    public string[] _result = new string[0];

    ScriptableObject scriptableObj;
    SerializedObject serialObj;
    SerializedProperty secondItemProperty;
    SerializedProperty resultProperty;

    private void OnEnable()
    {

    }

    [MenuItem("Window/GridEditor")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(GridEditor));
    }

    void OnGUI()
    {
        GUIStyle Description = new GUIStyle()
        {
            fixedHeight = 200,
            richText = true,
        };

        EditorGUI.BeginChangeCheck();
        _gridData = (GridData)EditorGUILayout.ObjectField(_gridData, typeof(GridData), true);
        if (EditorGUI.EndChangeCheck())
        {
            ApplyNewItem();
        }

        
        GUILayout.Label("Item", EditorStyles.boldLabel);
        _id = EditorGUILayout.IntField("ID", _id);


        GUILayout.Label("Array width/height", EditorStyles.miniBoldLabel);
        _width = EditorGUILayout.IntField("Width", _width);
        _height = EditorGUILayout.IntField("Height", _height);

        EditorGUI.BeginChangeCheck();

        if (_width != _fieldsArray.GetLength(0) || _height != _fieldsArray.GetLength(1))
        {
            _fieldsArray = new GridEffect[_width, _height];
        }
        if (EditorGUI.EndChangeCheck())
        {
            UpdateEditorItem();
        }
        ChangeArrayWidthAndHeight();
        if (GUILayout.Button("Apply"))
        {
            SaveItemChanges();
        }
    }

    void ApplyNewItem()
    {
        if (_gridData != null)
        {
            _id = _gridData.ID;
            _width = _gridData.Width;
            _height = _gridData.Height;

            _gridData.AfterDeserializeGridEffects();

            if (_gridData.Effects != null)
            {
                _fieldsArray = _gridData.Effects;
            }
                
            else
            {
                //Debug.Log("fill is empty " + _gridData.Effects.Length);
                _gridData.AfterDeserializeGridEffects();
                _fieldsArray = _gridData.Effects;
            }

            scriptableObj = this;
            serialObj = new SerializedObject(scriptableObj);


            /*secondItemProperty = serialObj.FindProperty("_secondItem");
            resultProperty = serialObj.FindProperty("_result");

            secondItemProperty.ClearArray();
            resultProperty.ClearArray();*/

            /*_secondItem = _gridData.SecondItem;
            _result = _gridData.Result;*/

/*            if (secondItemProperty.arraySize > 0)
            {
                _hasCombinables = true;
                SetArrays();
            }
            else _hasCombinables = false;*/
            foreach(GridEffect effect in _gridData.Effects)
            {
                Debug.Log(effect);
            }
        }
    }

    private void SaveItemChanges()
    {
        if (_gridData != null)
        {


            _gridData.ID = _id;
            _gridData.Width = _width;
            _gridData.Height = _height;
            _gridData.Effects = new GridEffect[_width, _height];

            if (_secondItem.Length > 0)
            {
                Debug.Log("Second Item " + _secondItem.Length + " " + _secondItem[0]);
                Debug.Log("Result Item " + _result.Length + " " + _result[0]);
            }

            System.Array.Copy(_fieldsArray, _gridData.Effects, _fieldsArray.Length);
            

            _gridData.BeforeSerializeGridEffects();

            EditorUtility.SetDirty(_gridData);
            this.SaveChanges();

            Debug.Log("Saved");
        }
    }

    private void UpdateEditorItem()
    {
        if (_gridData != null)
        {
/*            _name = _gridData.ItemName;
            _description = _gridData.Description;*/
            _id = _gridData.ID;
            _width = _gridData.Width;
            _height = _gridData.Height;
/*            _icon = _gridData.ItemIcon;
            _isKeyItem = _gridData.IsKeyItem;
            _itemType = _gridData.itemType;*/

            if (_gridData.Effects != null)
            {
                _fieldsArray = _gridData.Effects;
            }
                
            else
            {
                Debug.Log("fill is empty " + _gridData.Effects.Length);
                _gridData.AfterDeserializeGridEffects();
                _fieldsArray = _gridData.Effects;
            }
        }
    }

    void ChangeArrayWidthAndHeight()
    {
        for (int j = 0; j < _height; j++)
        {
            EditorGUILayout.BeginHorizontal();
            for (int i = 0; i < _width; i++)
            {
                _fieldsArray[i, j] = (GridEffect)EditorGUILayout.EnumFlagsField(_fieldsArray[i, j]);
            }
            EditorGUILayout.EndHorizontal();
        }
    }

/*    void SetArrays()
    {
        if (_hasCombinables && _secondItem != null)
        {
            secondItemProperty.arraySize = _secondItem.Length;
            resultProperty.arraySize = _result.Length;
            for (int i = 0; i < _secondItem.Length; i++)
            {
                secondItemProperty.GetArrayElementAtIndex(i).stringValue = _secondItem[i];
                resultProperty.GetArrayElementAtIndex(i).stringValue = _result[i];
            }
        }
        else
        {
            secondItemProperty.arraySize = 0;
            resultProperty.arraySize = 0;
        }

    }*/

}

