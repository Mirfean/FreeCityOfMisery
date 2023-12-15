using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionToInventory : MonoBehaviour
{
    [SerializeField] bool ExpectItem;

    [SerializeField] InventoryItemSO[] ExpectedItems;

    [SerializeField] InventoryItemSO[] GetItems;

    [SerializeField] bool DisableWhenEmpty;

 
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
