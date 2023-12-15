using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class scoreCounter : MonoBehaviour
{
    public PowerType powerType;
    [SerializeField] TextMeshProUGUI text;     
    public int value { get; private set; }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetScoreValue(int newValue)
    {
        value = newValue;
    }

    public void AddToScore(int add)
    {
        value += add;
        text.text = value.ToString();
    }
}
