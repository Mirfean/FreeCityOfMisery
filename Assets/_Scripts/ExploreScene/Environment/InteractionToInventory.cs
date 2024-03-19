using Assets._Scripts.ExploreScene.Environment;
using UnityEngine;

[RequireComponent(typeof(InteractableObject))]
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
        if (ExpectedItems != null && ExpectedItems.Length > 0)
            return ExpectedItems[0];
        return null;
    }

    public void RemoveFirstFromExpected()
    {
        ExpectedItems[0] = null;
        ReplaceToFront(ExpectedItems);
    }

    public InventoryItemSO GetFirstGetItem()
    {
        if (GetItems != null && GetItems.Length > 0)
            return GetItems[0];
        return null;
    }

    public void UseRemoveItemFromInventory(InventoryItemSO item)
    {
        InventoryPlayer.Instance.RemoveItem(item);
    }

    public void RemoveFirstGetItem()
    {
        GetItems[0] = null;
        ReplaceToFront(GetItems);
    }

    public void ReplaceToFront(InventoryItemSO[] tableToReplace)
    {
        for (int i = 0; i < tableToReplace.Length - 1; i++)
        {
            if (tableToReplace[i] == null && tableToReplace[i + 1] != null)
            {
                tableToReplace[i] = tableToReplace[i + 1];
                tableToReplace[i + 1] = null;
            }
        }
    }
}
