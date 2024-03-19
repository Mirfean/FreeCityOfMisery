using UnityEngine;
using UnityEngine.UI;

public class ItemInventoryUI : MonoBehaviour
{
    [SerializeField] Button itemButton;
    //[SerializeField] SpriteRenderer spriteRend;

    [SerializeField] Image image;

    [SerializeField] InventoryItemSO itemSO;
    [SerializeField] InventoryPlayer inventoryPlayer;

    public InventoryItemSO ItemSO { get => itemSO; set => itemSO = value; }

    // Start is called before the first frame update
    void Start()
    {
        if (inventoryPlayer == null) inventoryPlayer = FindObjectOfType<InventoryPlayer>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void AddItem(InventoryItemSO item)
    {
        ItemSO = item;
        image.overrideSprite = ItemSO.SpriteItem;
    }

    public void RemoveItem()
    {
        ItemSO = null;
        image.overrideSprite = null;
        //Deactivate button
    }

    public void ClickItem()
    {
        if (ItemSO != null) 
        {
            Player._POPUP_(ItemSO.ItemName + " - " +ItemSO.Description);
            Debug.Log("Clicked " + ItemSO.ItemName + " " + ItemSO.Description);
        } 
        else Debug.Log("Clicked empty inventory");
    }




}
