using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryPlayer : Singleton<InventoryPlayer>
{
    [SerializeField] InventoryUI inventoryUI;

    public bool AddItem(InventoryItemSO NewItem)
    {
        return inventoryUI.AddItem(NewItem);
    }

    private void Start()
    {
        if (inventoryUI == null) inventoryUI = FindFirstObjectByType<InventoryUI>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
