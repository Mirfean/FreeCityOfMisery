using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class DebugGridTriangle : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] TextMeshProUGUI X;
    [SerializeField] TextMeshProUGUI Y;

    [SerializeField] GridField gridField;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ChangeTexts(int x, int y)
    {
        X.text = x.ToString();
        Y.text = y.ToString();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //Debug.Log($"Touching {X.text} {Y.text}");
        //PlacingManager.CheckToPlace(gameObject.GetComponent<GridTriangle>());
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //Debug.Log($"Leaving {X.text} {Y.text}");
    }

}
