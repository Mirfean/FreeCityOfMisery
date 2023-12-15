using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcData : MonoBehaviour
{
    [SerializeField] string npcName;
    [SerializeField] DeckSO npcDeck;
    [SerializeField] GridData gridData;

    [Range(0f, 10f), SerializeField] int respectLevel;

    //Fraction(to modify respect and deck)

    public string NpcName { get => npcName; set => npcName = value; }
    public DeckSO NpcDeck { get => npcDeck; set => npcDeck = value; }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public DeckSO GetDeck()
    {
        //Place for any modifiers to the deck
        return npcDeck;
    }

    public void SendNPCDeck()
    {
        if (npcDeck == null || gridData == null) return;
        GameManager.SetEnemyDeck(npcDeck);
        GameManager.SetGridData(gridData);
    }
}
