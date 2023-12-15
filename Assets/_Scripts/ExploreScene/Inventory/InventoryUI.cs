using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] ItemInventoryUI[] items;

    [SerializeField] InventoryItemSO test;

    // Start is called before the first frame update
    void Start()
    {
        //items[0].AddItem(test);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    ItemInventoryUI CheckIfEmptyAvailable()
    {
        foreach(var item in items)
        {
            if (item.ItemSO != null) return item;
        }
        return null;
    }

    bool CheckIfItemInInventory(string itemID)
    {
        if (GetItemById(itemID) != null) return true;
        return false;
    }

    ItemInventoryUI GetItemById(string id)
    {
        foreach (var item in items)
        {
            if (item.ItemSO.ID == id)
            {
                return item;
            }
        }
        return null;
    }

    public bool AddItem(InventoryItemSO NewItem)
    {
        var itemSlot = CheckIfEmptyAvailable();
        if (itemSlot != null)
        {
            itemSlot.AddItem(NewItem);
            return true;
        }
        return false;
    }

    public bool RemoveItem(string itemId)
    {
        var item = GetItemById(itemId);
        if(item != null)
        {
            item.RemoveItem();
            return true;
        }
        return false;
    }

    public bool UseItem(string itemID, bool removeAfterUse = false)
    {
        var item = GetItemById(itemID);
        if (item != null)
        {
            Debug.Log($"Use {item.ItemSO.ItemName}");
            if(removeAfterUse) item.RemoveItem();
            return true;
        }
        return false;
    }
}
