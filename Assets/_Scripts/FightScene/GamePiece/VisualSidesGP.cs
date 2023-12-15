using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class VisualSidesGP : MonoBehaviour
{
    [SerializeField] List<GameObject> side0;
    [SerializeField] List<GameObject> side1;
    [SerializeField] List<GameObject> side2;

    public List<GameObject> Side0 { get => side0; set => side0 = value; }
    public List<GameObject> Side1 { get => side1; set => side1 = value; }
    public List<GameObject> Side2 { get => side2; set => side2 = value; }
    public void ChangeSide(int sideNO, List<PowerType> newPowers) 
    {
        switch (sideNO)
        {
            case 0:
                ModifyIcons(side0, newPowers);
                break;
            case 1:
                ModifyIcons(side1, newPowers);
                break;
            case 2:
                ModifyIcons(side2, newPowers);
                break;
            default:
                Debug.LogWarning("Wrong number!!!");
                break;
        }
    }

    void ModifyIcons(List<GameObject> side, List<PowerType> newPowers)
    {
        for(int i = 0; i < side.Count; i++)
        {
            if(i >= newPowers.Count)
            {
                side[i].SetActive(false);
                continue;
            }
            //side[i].



        }
    }

    
}
