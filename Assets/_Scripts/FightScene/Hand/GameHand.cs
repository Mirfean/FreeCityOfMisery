using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameHand : MonoBehaviour
{
    [SerializeField] int limitCards;

    [SerializeField] Transform[] slots;
    [SerializeField] GamePiece[] cards;

    public GamePiece[] Cards { get => cards; set => cards = value; }
    public Transform[] Slots { get => slots; set => slots = value; }

    
    
    // Start is called before the first frame update
    void Start()
    {
        SetSlots(limitCards);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SetSlots(int limit = 0)
    {
        void setNewCards()
        {
            if (cards == null || cards.Length == 0)
            {
                cards = new GamePiece[limit];
            }
            else
            {
                GamePiece[] temp = cards;
                cards = new GamePiece[limitCards];
                for(int i = 0; i < temp.Length; i++)
                {
                    cards[i] = temp[i];
                    //cards[i].gameObject.GetComponent<DragAndDropGP>().PreviousPosition = slots[i].position;
                }
            }
        }

        if(limit != 0)
        {
            limitCards = limit;
        }

        //slots = new Transform[limitCards];
        setNewCards();

    }

    internal void SetNewHand()
    {
        cards = new GamePiece[limitCards];
    }

    public int MaxCardsToGet()
    {
        return cards.Where(x => x == null).Count();
    }

    public bool CheckEmptySlot()
    {
        return cards.Where(x => x != null).Count() < limitCards ? true : false;
    }

    public void PlaceCardOnHand(GamePiece card)
    {
        for(int i = 0; i < slots.Length; i++)
        {
            if(cards[i] == null)
            {
                PlaceCardAtSlot(i, card);
                return;
            }
        }
    }

    void PlaceCardAtSlot(int slotNO, GamePiece card)
    {
        cards[slotNO] = card;
        cards[slotNO].transform.position = slots[slotNO].position;
        card.handID = slotNO;
    }

    public void RemoveCard(int id)
    {
        cards[id] = null;
    }
}
