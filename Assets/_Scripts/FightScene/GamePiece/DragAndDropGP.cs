using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragAndDropGP : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] RectTransform rectTransform;
    [SerializeField] Image image;
    [SerializeField] CanvasGroup canvasGroup;
    [SerializeField] GamePiece gamePiece;

    [SerializeField] Vector3 previousPosition;

    public Vector3 PreviousPosition
    {
        get { return previousPosition; }
        set
        {
            previousPosition = value;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        image = GetComponent<Image>();
        canvasGroup = GetComponent<CanvasGroup>();
        gamePiece = GetComponent<GamePiece>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (gamePiece.IMMOVABLE) eventData.pointerDrag = null;
        else
        {
            image.color = new Color32(255, 255, 255, 130);
            canvasGroup.blocksRaycasts = false;
            //previousPosition = transform.position;
            PlacingManager.Instance.AddTriangle(gameObject);
        }



    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        image.color = Color.white;
        canvasGroup.blocksRaycasts = true;
        PlacingManager.Instance.RemoveTriangle();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //image.color = Color.green;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //image.color = Color.white;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        //throw new System.NotImplementedException();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        //throw new System.NotImplementedException();
    }
}
