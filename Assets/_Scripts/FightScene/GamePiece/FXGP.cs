using UnityEngine;
using UnityEngine.EventSystems;

public class VFXGP : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, IPointerUpHandler
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnPointerClick(PointerEventData eventData)
    {
        //Show Effects
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        //Hide Effects
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //Show card info
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //Hide card info
    }


}
