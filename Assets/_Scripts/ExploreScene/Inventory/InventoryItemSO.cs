using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Inventory Item", menuName = "Duck/InventoryItem"), Serializable]
public class InventoryItemSO : ScriptableObject
{

    public string ID;
    public string ItemName;
    public string Description;
    public Sprite SpriteItem;

}
