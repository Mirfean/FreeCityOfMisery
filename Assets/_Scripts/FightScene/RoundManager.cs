using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundManager : MonoBehaviour
{
    [SerializeField] EnemyFightAI _enemyAI;
    [SerializeField] DeckManager _deckManager;
    [SerializeField] ScoreManager _scoreManager;

    [SerializeField] GameObject _endResultUI;
    [SerializeField] EndFightScreen _endScreenData;

    public bool PlayerRound { get; private set; }
    
    public int RoundLimiter { get; private set; }

    public static Action EndPlayerMove;
    // Start is called before the first frame update
    void Start()
    {
        EndPlayerMove += ConfirmMove;
        RoundLimiter = 3;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void ConfirmMove()
    {
        PlayerRound = false;
        EndTurn();
    }

    public void EndTurn()
    {
        PlayerRound = false;
        Debug.Log("End of turn");
        if (_enemyAI.EnemyMove())
        {
            //Player round now
        }
        else
        {
            //Enemy has no cards
            //Play to end of this round
        }
        PlayerRound = true;
    }

    bool isEndRound()
    {
        return false;
    }

    public void EndRound()
    {
        _deckManager.discardAllHands();
        RoundLimiter--;

        //GAME STOP
        if (RoundLimiter == 0)
        {
            _endResultUI.SetActive(true);
            FightManager.ENDFIGHT();
            //EndResultUI.SetActive(true);
        }
    }
}
