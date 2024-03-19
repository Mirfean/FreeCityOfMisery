using Assets.ScriptableObjects;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DescriptionCardWindow : MonoBehaviour
{
    [SerializeField] GameObject cardDescriptionPanel;
    [SerializeField] TextMeshProUGUI cardDescriptionTitle;
    [SerializeField] TextMeshProUGUI cardDescriptionText;
    [SerializeField] TextMeshProUGUI cardDescriptionFlavour;
    [SerializeField] Image cardDescriptionImage;
    [SerializeField] GamePiece gamePiece;

    public static Action<GamePiece> ShowDescription;
    public static Action HideDescription;

    // Start is called before the first frame update
    void Start()
    {
        ShowDescription += ShowCardDescription;
        HideDescription += HideCardDescription;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowCardDescription(GamePiece newGamePiece)
    {
        if(newGamePiece.HIDE)
        {
            return;
        }
        cardDescriptionPanel.SetActive(true);
        cardDescriptionTitle.text = newGamePiece.gamePieceSO.cardName;
        if(newGamePiece.gamePieceSO.description != null) cardDescriptionFlavour.text = newGamePiece.gamePieceSO.description;

        foreach (CardEffect effect in newGamePiece.gamePieceSO.cardEffects)
        {
            cardDescriptionText.text += effect.description + "\n";
        }
        
        this.gamePiece.gamePieceSO = newGamePiece.gamePieceSO;
        gamePiece.SetNewIcons();
        //this.gamePiece
    }

    public void HideCardDescription()
    {
        cardDescriptionText.text = "";
        cardDescriptionTitle.text = "";
        cardDescriptionFlavour.text = "";
        gamePiece.ResetIcons();
        cardDescriptionPanel.SetActive(false);
    }
}
