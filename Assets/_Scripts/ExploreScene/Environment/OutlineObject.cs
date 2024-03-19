using UnityEngine;

public class OutlineObject : MonoBehaviour
{
    [SerializeField]
    internal Material _baseMaterial;

    [SerializeField]
    internal Material _outlineMaterial;

    // Start is called before the first frame update
    internal void Start()
    {
        /*        _baseMaterial = gameObject.GetComponent<SpriteRenderer>().material;
                //Outline materials preparing
                Outlines basicOutline = Outlines.OutlineBasic;
                _outlineMaterial = Resources.Load($"Materials/{basicOutline.ToString()}") as Material;
                _outlineMaterial.SetColor("Outline Color", Color.white);*/
    }

    internal void OnMouseEnter()
    {
        gameObject.GetComponent<SpriteRenderer>().material = _outlineMaterial;
    }

    private void OnMouseOver()
    {
        //Show Interact sign

    }

    internal void OnMouseExit()
    {
        gameObject.GetComponent<SpriteRenderer>().material = _baseMaterial;
    }
}
