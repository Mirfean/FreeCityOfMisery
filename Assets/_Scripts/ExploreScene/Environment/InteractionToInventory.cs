using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionToInventory : MonoBehaviour
{
    public bool ExpectItem;

    [SerializeField] InventoryItemSO[] ExpectedItems;

    [SerializeField] InventoryItemSO[] GetItems;

    public bool DisableWhenEmpty;

 
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public InventoryItemSO GetFirstExpected()
    {
        if(ExpectedItems != null && ExpectedItems.Length > 0)
            return ExpectedItems[0];
        return null;
    }

    public InventoryItemSO GetFirstGet()
    {
        if(GetItems != null && GetItems.Length > 0)
            return GetItems[0];
        return null;
    }
}
