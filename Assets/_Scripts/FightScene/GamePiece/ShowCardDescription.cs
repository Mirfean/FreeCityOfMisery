using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShowCardDescription : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] AnimationClip _showUpAnim;
    [SerializeField] Animator _animator;

    public void OnPointerEnter(PointerEventData eventData)
    {
        DescriptionCardWindow.ShowDescription(gameObject.GetComponent<GamePiece>());

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        DescriptionCardWindow.HideDescription();
    }


}
