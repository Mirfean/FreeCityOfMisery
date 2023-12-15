using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GridField : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    Vector2 pos;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    internal void SetValue(int i, int j)
    {
        pos.x = i;
        pos.y = j;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log($"Entering {pos.x} {pos.y}");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log($"Leaving {pos.x} {pos.y}");
    }

    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log($"drop on {pos.x} {pos.y}");
    }
}
