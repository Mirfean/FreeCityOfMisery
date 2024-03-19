using System;
using UnityEngine;

/// <summary>
/// Round manager and also START and END of the game UI
/// </summary>
public class RoundManager : MonoBehaviour
{
    [SerializeField] EnemyFightAI _enemyAI;
    [SerializeField] DeckManager _deckManager;
    [SerializeField] ScoreManager _scoreManager;

    [SerializeField] GameObject _startGameUI;
    [SerializeField] StartFightScreen _startScreenData;

    [SerializeField] GameObject _endResultUI;
    [SerializeField] EndFightScreen _endScreenData;

    [SerializeField] bool _playerSkipped;
    [SerializeField] bool _enemySkipped;

    public bool PlayerCanMove { get; private set; }

    public int RoundLimiter;

    public static Action EndPlayerMove;
    // Start is called before the first frame update
    void Start()
    {
        EndPlayerMove += ConfirmMove;
    }

    // Update is called once per frame
    void Update()
    {

    }
    void ConfirmMove()
    {
        _playerSkipped = false;
        PlayerCanMove = false;
        EndTurn();
    }

    public void SkipTurn()
    {
        _playerSkipped = true;
        PlayerCanMove = false;
        EndTurn();
    }

    public void EndTurn()
    {
        PlayerCanMove = false;
        Debug.Log("End of turn");
        if (_enemyAI.EnemyMove())
        {
            //PlayerCanMove = true;
        }
        else
        {
            _enemySkipped = true;
        }
        PlayerCanMove = true;
        EndRoundCheck();
    }

    bool isEndRound()
    {
        return false;
    }

    public void StartGame(String Player, String Enemy, int Rounds)
    {
        RoundLimiter = Rounds;
        _startGameUI.SetActive(true);
        _startScreenData.SetRounds(RoundLimiter);
        _startScreenData.SetEnemy(Enemy);
        _startScreenData.SetPlayer(Player);
    }

    public void HideStartUI()
    {
        _startGameUI.SetActive(false);
    }

    public void EndRoundCheck()
    {
        if((_playerSkipped || _deckManager.PlayerHand.Cards.Length == 0)
            && (_enemySkipped || _deckManager.EnemyHand.Cards.Length == 0))
        {
            EndRound();
        }
        else
        {
            PlayerCanMove = true;
            _enemySkipped = false;
            _playerSkipped = false;
        }
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
            _endResultUI.SetActive(true);
            bool Success = _endScreenData.SetEndScreenData(_scoreManager.PlayerScore, _scoreManager.EnemyScore);

            _endScreenData.SetWinLose(Success);
        }
    }
}
