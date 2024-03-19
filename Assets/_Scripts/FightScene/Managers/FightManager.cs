using System;
using System.Collections.Generic;
using UnityEngine;

public class FightManager : MonoBehaviour
{
    [SerializeField] List<DeckSO> decks;
    [SerializeField] CurrentGameData gameData;

    [SerializeField] DeckManager _deckManager;
    [SerializeField] ScoreManager _scoreManager;
    [SerializeField] RoundManager _roundManager;
    [SerializeField] GridGame gridGame;

    [SerializeField] EndFightScreen _endScreenData;

    [SerializeField] GameHand _hand;

    public static Action ENDFIGHT;

    private void Awake()
    {
        BattlePreparer();

        if (gameData != null)
            gridGame.gridData = gameData._GridData;

        ENDFIGHT += EndFight;
    }

    private void OnDestroy()
    {
        ENDFIGHT -= EndFight;
    }

    // Start is called before the first frame update
    void Start()
    {
        _deckManager.DrawToFullHandPlayer();
        _deckManager.DrawToFullHandEnemy();

        _roundManager.StartGame(gameData.CharacterName, gameData.CurrentEnemyName, gameData.RoundLimiter);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void BattlePreparer()
    {
        LoadDecks();
    }

    void LoadDecks()
    {
        _deckManager.PreparePlayerDeck(gameData.PlayerDeck);
        _deckManager.PrepareEnemyDeck(gameData.EnemyDeck);
    }

    public void EndFight()
    {
        //Compare score -> get a winner
        //var finalScore = _scoreManager.GetFinalScore();
        //gameData.FightResult = finalScore;
        //gameData.LastFightWin = finalScore.Success


        //_endScreenData.SetPlayerData(gameData.CharacterName, _scoreManager.PlayerScore.GetSum());
        //_endScreenData.SetEnemyData(gameData.CurrentEnemyName, _scoreManager.EnemyScore.GetSum());

/*        if (finalScore.Success)
        {
            // Send info win/lose + order of most used powers(for future)
        }*/

        //Effects


        //Hide scene by using method from game manager
    }

    public void EndFightToExplore()
    {
        ExploreManager.HideFight();
    }
}
