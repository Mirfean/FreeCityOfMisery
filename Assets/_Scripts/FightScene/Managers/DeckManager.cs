using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DeckManager : MonoBehaviour
{
    [SerializeField] GameHand _playerHand;
    [SerializeField] GameHand _enemyHand;

    [SerializeField] List<GamePiece> _playerDeck;
    [SerializeField] List<GamePiece> _playerDiscard;

    [SerializeField] Transform _playerDeckPlace;
    [SerializeField] Transform _enemyDeckPlace;

    [SerializeField] List<GamePiece> _enemyDeck;
    [SerializeField] List<GamePiece> _enemyDiscard;


    [SerializeField] SpawnerGamePiece _spawnerGamePiece;
    
    [SerializeField] Transform discard_Player;
    [SerializeField] Transform discard_Enemy;

    public static Action<DeckSO> LoadPlayerDeck;
    public static Action<DeckSO> LoadEnemyDeck;
    public static Action DrawFullHand;

    public List<GamePiece> EnemyDeck { get => _enemyDeck; set => _enemyDeck = value; }
    public List<GamePiece> EnemyDiscard { get => _enemyDiscard; set => _enemyDiscard = value; }
    public GameHand EnemyHand { get => _enemyHand; set => _enemyHand = value; }

    private void Awake()
    {
        LoadPlayerDeck += PreparePlayerDeck;
        LoadEnemyDeck += PrepareEnemyDeck;
        DrawFullHand += DrawToFullHandPlayer;
    }



    #region Deck Preparing

    public void PreparePlayerDeck(DeckSO deckSO)
    {
        _playerDeck = new List<GamePiece>();
        _playerHand.SetNewHand();
        _playerDeck = PrepareDeck(deckSO.Cards, true);
    }
    public void PrepareEnemyDeck(DeckSO deckSO)
    {
        _enemyDeck = new List<GamePiece>();
        _enemyHand.SetNewHand();
        EnemyDeck = PrepareDeck(deckSO.Cards, false);
    }

    void PreparePlayerDeck(List<GamePieceSO> Cards)
    {
        _playerDeck = new List<GamePiece>();
        _playerHand.SetNewHand();
        _playerDeck = PrepareDeck(Cards, true);
    }

    void PrepareEnemyDeck(List<GamePieceSO> Cards)
    {
        _enemyDeck = new List<GamePiece>();
        _enemyHand.SetNewHand();
        EnemyDeck = PrepareDeck(Cards, false);
    }

    List<GamePiece> PrepareDeck(List<GamePieceSO> Cards, bool player)
    {
        List<GamePiece> result = new List<GamePiece>();
        if (player)
        {
            foreach (GamePieceSO SO in Cards)
            {
                result.Add(_spawnerGamePiece.SpawnPlayerGamePiece(SO));
            }
        }
        else
        {
            foreach (GamePieceSO SO in Cards)
            {
                result.Add(_spawnerGamePiece.SpawnEnemyGamePiece(SO));
            }
        }
        
        return result;
    }

    #endregion

    #region Drawing

    public void DrawToFullHandPlayer()
    {
        DrawCards(_playerHand.MaxCardsToGet(), true);
    }

    internal void DrawToFullHandEnemy()
    {
        Debug.Log("Enemy max to get " + _enemyHand.MaxCardsToGet());
        //DrawCards(_enemyHand.MaxCardsToGet());
        DrawCards(6, false);
    }

    void DrawCards(int cardsToDraw, bool forPlayer)
    {
        if(forPlayer)
        {
            for (int i = 0; i < cardsToDraw; i++)
            {
                DrawCardPlayer();
            }
        }
        else
        {
            for (int i = 0; i < cardsToDraw; i++)
            {
                DrawCardEnemy();
            }
        }
    }

    public void DrawCardPlayer()
    {
        Debug.Log("Draw a card for a player");
        if(_playerDeck.Count == 0 && _playerDiscard.Count > 0)
        {
            Debug.Log("Shuffle player discard to deck");
            ShuffleDiscardPlayer();
        }
        else if(_playerDiscard.Count == 0 && _playerDeck.Count == 0)
        {
            Debug.LogWarning("DECK IS EMPTY!!!");
        }

        if (_playerHand.CheckEmptySlot())
        {
            if(_playerDeck.Count > 0)
            {
                Debug.Log("Literally drawing a card");
                GamePiece drawnCard = _playerDeck[UnityEngine.Random.Range(0, _playerDeck.Count - 1)];
                _playerDeck.Remove(drawnCard);
                _playerHand.PlaceCardOnHand(drawnCard);
            }
        }
        else
        {
            Debug.Log("There is no empty space!");
        }
    }

    public void DrawCardEnemy()
    {
        if (_enemyDeck.Count == 0 && _enemyDiscard.Count > 0)
        {
            ShuffleDiscardPlayer();
        }
        else if (_enemyDiscard.Count == 0 && _enemyDeck.Count == 0)
        {
            Debug.LogWarning("DECK IS EMPTY!!!");
        }

        if (!_playerHand.CheckEmptySlot())
        {
            if (_playerDeck.Count > 0)
            {
                GamePiece drawnCard = _enemyDeck[UnityEngine.Random.Range(0, _enemyDeck.Count - 1)];
                _enemyDeck.Remove(drawnCard);
                _enemyHand.PlaceCardOnHand(drawnCard);
            }
        }
    }

    #endregion

    #region Discard

    void discardPlayerCard(GamePiece gamePiece)
    {
        _playerDiscard.Add(gamePiece);
        _playerHand.RemoveCard(Array.IndexOf(_playerHand.Cards, gamePiece));
        gamePiece.transform.position = discard_Player.position;

    }

    void discardPlayerCard(int id)
    {
        _playerHand.Cards[id].transform.position = discard_Player.position;
        _playerDiscard.Add(_playerHand.Cards[id]);
        _playerHand.RemoveCard(id);
    }

    void discardPlayer(List<GamePiece> gamePieces)
    {
        foreach (GamePiece piece in gamePieces)
        {
            discardPlayerCard(piece);
        }
    }

    void discardEnemyCard(int id)
    {
        _enemyHand.Cards[id].transform.position = discard_Enemy.position;
        _enemyDiscard.Add(_enemyHand.Cards[id]);
        _enemyHand.RemoveCard(id);
    }

    public void discardAllHands()
    {
        for(int i = 0; i < _playerHand.Cards.Length; i++)
        {
            if (_playerHand.Cards[i] != null) discardPlayerCard(i);
        }
        for (int i = 0; i < _enemyHand.Cards.Length; i++)
        {
            if (_enemyHand.Cards[i] != null) discardEnemyCard(i);
        }
    }

    void ShuffleDiscardPlayer()
    {
        if(_playerDiscard.Count != 0)
        {
            _playerDeck = _playerDiscard;
            _playerDiscard = new List<GamePiece>();
        }

        foreach(GamePiece piece in _playerDeck)
        {
            piece.transform.position = _playerDeckPlace.position;
        }

    }

    void ShuffleDiscardEnemy()
    {
        if (_enemyDiscard.Count != 0)
        {
            _enemyDeck = _enemyDiscard;
            _enemyDiscard = new List<GamePiece>();
        }
        foreach (GamePiece piece in _enemyDeck)
        {
            piece.transform.position = _enemyDeckPlace.position;
        }
    }

    #endregion
}
