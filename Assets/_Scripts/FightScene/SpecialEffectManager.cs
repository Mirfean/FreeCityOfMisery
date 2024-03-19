using Assets.Scripts;
using System;
using System.Collections.Generic;
using UnityEngine;

public struct FieldPower
{
    public SpecialEffects effectType;
    public PowerType powerType;
    public int value;
}

public class SpecialEffectManager : MonoBehaviour
{
    public static Action<CardEffect> USE_EFFECT;
    public static Action<List<CardEffect>> USE_ALL_EFFECTS;

    [SerializeField] ScoreManager scoreManager;
    [SerializeField] DeckManager deckManager;
    [SerializeField] RoundManager roundManager;
    [SerializeField] GridGame grid;

    private void OnEnable()
    {
        USE_EFFECT += UseEffect;
        USE_ALL_EFFECTS += ActivateAllEffects;
    }

    private void OnDisable()
    {
        USE_EFFECT -= UseEffect;
        USE_ALL_EFFECTS -= ActivateAllEffects;
    }

    void UseEffect(CardEffect cardEffect)
    {
        switch (cardEffect.effect)
        {
            case SpecialEffects.AddPts:
                scoreManager.GetCurrentUserScore(roundManager.PlayerCanMove).
                    AddToCounter(cardEffect.powerType, cardEffect.value);
                break;
            case SpecialEffects.RemovePts:
                scoreManager.GetCurrentUserScore(roundManager.PlayerCanMove).
                    AddToCounter(cardEffect.powerType, -cardEffect.value);
                break;
        }
    }

    void ActivateAllEffects(List<CardEffect> cardEffects)
    {
        foreach (CardEffect cardEffect in cardEffects)
        {
            Debug.Log("Activate effect " + cardEffect.name);
            UseEffect(cardEffect);
        }

    }
}
