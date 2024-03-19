using UnityEngine;

public class InventoryPlayer : Singleton<InventoryPlayer>
{
    [SerializeField] InventoryUI inventoryUI;

    private void Start()
    {
        if (inventoryUI == null) inventoryUI = FindFirstObjectByType<InventoryUI>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public bool CheckIfItemInInventory(string itemID)
    {
        return inventoryUI.CheckIfItemInInventory(itemID);
    }

    public bool AddItem(InventoryItemSO NewItem)
    {
        return inventoryUI.AddItem(NewItem);
    }

    public void RemoveItem(InventoryItemSO item)
    {
        inventoryUI.RemoveItem(item.ID);
    }
}
