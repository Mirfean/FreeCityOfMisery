using Assets.ScriptableObjects;
using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class SpawnerGamePiece : MonoBehaviour
{
    [SerializeField] GameObject _gamePiecePrefab;
    [SerializeField] Transform _playerDeck;
    [SerializeField] Transform _enemyDeck;

    [SerializeField] List<Icon_Power> icon_Powers;

    public List<GamePieceSO> piecesToSpawn;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public GamePiece SpawnPlayerGamePiece(GamePieceSO pieceSO)
    {
        GameObject temp = Instantiate(_gamePiecePrefab);
        GamePiece tempGamePiece = temp.GetComponent<GamePiece>();
        VisualSidesGP tempVFX = temp.GetComponent<VisualSidesGP>();
        tempGamePiece.Effects = pieceSO.cardEffects;

        temp.transform.SetParent(_playerDeck, true);
        temp.transform.localScale = Vector3.one;
        temp.GetComponent<RectTransform>().anchoredPosition = Vector3.zero;

        temp.name = pieceSO.name;
        temp.GetComponent<GamePiece>().Id = pieceSO.ID;

        SetPowers(tempGamePiece, pieceSO);
        UpdateAllSymbols(tempGamePiece, tempVFX);

        return temp.GetComponent<GamePiece>();
    }

    public GamePiece SpawnEnemyGamePiece(GamePieceSO pieceSO)
    {
        GameObject temp = Instantiate(_gamePiecePrefab);
        GamePiece tempGamePiece = temp.GetComponent<GamePiece>();
        VisualSidesGP tempVFX = temp.GetComponent<VisualSidesGP>();
        tempGamePiece.Effects = pieceSO.cardEffects;

        temp.transform.SetParent(_enemyDeck, true);
        temp.transform.localScale = Vector3.one;
        temp.GetComponent<RectTransform>().anchoredPosition = Vector3.zero;

        temp.name = pieceSO.name;
        temp.GetComponent<GamePiece>().Id = pieceSO.ID;

        SetPowers(tempGamePiece, pieceSO);
        UpdateAllSymbols(tempGamePiece, tempVFX);

        return temp.GetComponent<GamePiece>();
    }

    void SetPowers(GamePiece spawnedPiece, GamePieceSO pieceSO)
    {
        spawnedPiece.SetSides(pieceSO.VerticalSide, pieceSO.LeftSide, pieceSO.RightSide);
    }

    public void UpdateAllSymbols(GamePiece gamePiece, VisualSidesGP vfx)
    {
        ModifyIcons(vfx.Side0, gamePiece.side0);
        ModifyIcons(vfx.Side1, gamePiece.side1);
        ModifyIcons(vfx.Side2, gamePiece.side2);
    }

/*    public void UpdateSideSymbols(int sideNO, List<PowerType> newPowers)
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
    }*/

    void ModifyIcons(List<GameObject> side, List<PowerType> newPowers)
    {
        for (int i = 0; i < side.Count; i++)
        {
            if (i >= newPowers.Count)
            {
                side[i].SetActive(false);
                continue;
            }
            side[i].GetComponent<Image>().sprite = icon_Powers.Find(x => x.power == newPowers[i]).symbol;

        }
    }
}
