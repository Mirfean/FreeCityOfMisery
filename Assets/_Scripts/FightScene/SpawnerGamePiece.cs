using Assets.ScriptableObjects;
using Assets.Scripts;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class GridEffectPrefab
{
    public GridEffect gridEffect;
    public GameObject effect;
}

public class SpawnerGamePiece : MonoBehaviour
{
    [SerializeField] GameObject _gamePiecePrefab;
    [SerializeField] Vector3 _gamePieceScale;

    [SerializeField] Transform _playerDeck;
    [SerializeField] Transform _enemyDeck;
    [SerializeField] Transform _gridParentForEffects;

    [SerializeField] List<Icon_Power> icon_Powers;

    public List<GamePieceSO> piecesToSpawn;

    [SerializeField] List<GridEffectPrefab> _gridEffects;
    [SerializeField] Vector3 _gridEffectsScale;

    //TEMPORARY
    [SerializeField] GameObject testObject;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public GamePiece SpawnGamePieceFromEffectsList(GridEffect gridEffectToSpawn)
    {
        //Debug.LogWarning("SpawnGamePieceFromResources");
        //Debug.Log(_gridEffects);
        //Debug.Log(gridEffectToSpawn);
        GameObject SelectedPrefab = _gridEffects.Where(x => x.gridEffect == gridEffectToSpawn).FirstOrDefault().effect; 
        GameObject temp = Instantiate(SelectedPrefab);
        
        temp.transform.SetParent(_gridParentForEffects, true);
        temp.transform.localScale = _gridEffectsScale;
        temp.GetComponent<RectTransform>().anchoredPosition = Vector3.zero;

        return temp.GetComponent<GamePiece>();
    }

    public GamePiece SpawnPlayerGamePiece(GamePieceSO pieceSO)
    {
        GameObject temp = Instantiate(_gamePiecePrefab);
        GamePiece tempGamePiece = temp.GetComponent<GamePiece>();
        VisualSidesGP tempVFX = temp.GetComponent<VisualSidesGP>();
        tempGamePiece.Effects = pieceSO.cardEffects;

        temp.transform.SetParent(_playerDeck, true);
        temp.transform.localScale = _gamePieceScale;
        temp.GetComponent<RectTransform>().anchoredPosition = Vector3.zero;

        temp.name = pieceSO.name;
        temp.GetComponent<GamePiece>().gamePieceSO = pieceSO;
        temp.GetComponent<GamePiece>().Id = pieceSO.ID;
        temp.GetComponent<GamePiece>().PLAYERCARD = true;
        temp.GetComponent<GamePiece>().HIDE = true;

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
        temp.transform.localScale = _gamePieceScale;
        temp.GetComponent<RectTransform>().anchoredPosition = Vector3.zero;

        temp.name = pieceSO.name;
        temp.GetComponent<GamePiece>().Id = pieceSO.ID;
        temp.GetComponent<GamePiece>().PLAYERCARD = false;
        temp.GetComponent<GamePiece>().HIDE = true;

        SetPowers(tempGamePiece, pieceSO);
        UpdateAllSymbols(tempGamePiece, tempVFX);

        return temp.GetComponent<GamePiece>();
    }

    void SetPowers(GamePiece spawnedPiece, GamePieceSO pieceSO)
    {
        spawnedPiece.SetSides(pieceSO.VerticalSide, pieceSO.LeftSide, pieceSO.RightSide);
        spawnedPiece.SetBonus();
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
