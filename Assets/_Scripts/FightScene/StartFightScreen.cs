using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StartFightScreen : MonoBehaviour
{
    [SerializeField] TMP_Text RoundsTest;

    [SerializeField] TMP_Text player_name;
    [SerializeField] TMP_Text enemy_name;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetRounds(int rounds)
    {
        RoundsTest.text = $"Round {rounds}";
    }

    public void SetEnemy(string enemy) { enemy_name.text = enemy; }
    public void SetPlayer(string player) { player_name.text = player; }
}
