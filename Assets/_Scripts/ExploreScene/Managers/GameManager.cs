using PixelCrushers.DialogueSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] CurrentGameData gameData;

    [SerializeField] Camera cameraToHide;
    [SerializeField] EventSystem eventSystemToHide;

    public static Action HideFight;
    public static Action<DeckSO> SetEnemyDeck;
    public static Action<GridData> SetGridData;

    [SerializeField] RewardCards rewardCards;

    void OnEnable()
    {
        // Make the functions available to Lua: (Replace these lines with your own.)
        Lua.RegisterFunction(nameof(PauseGameToFight), this, SymbolExtensions.GetMethodInfo(() => PauseGameToFight()));
        Lua.RegisterFunction(nameof(ShowRewardCards), this, SymbolExtensions.GetMethodInfo(() => ShowRewardCards()));
        
        HideFight += ResumeDialogueSceneFromFight;
        SetEnemyDeck += SetNewEnemyDeck;
        SetGridData += SetNewGridData;
    }



    void OnDisable()
    {
        //Lua.UnregisterFunction(nameof(PauseGameToFight));
        //HideFight += ResumeDialogueSceneFromFight;
    }

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("NEW GAME MANAGER " + SceneManager.GetActiveScene().name);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #region Switching Scenes

    // Pausing Game
    void PauseGameToFight()
    {
        Debug.Log(DialogueManager.CurrentActor.gameObject.name);

        cameraToHide = Camera.main;
        eventSystemToHide = EventSystem.current;
        
        cameraToHide.gameObject.SetActive(false);
        eventSystemToHide.gameObject.SetActive(false);

        SceneManager.LoadSceneAsync("BattleScene", LoadSceneMode.Additive);

        StartCoroutine(WaitUntilSceneLoaded());
    }

    IEnumerator WaitUntilSceneLoaded()
    {
        yield return new WaitForEndOfFrame();
        Scene scene = SceneManager.GetSceneAt(1);

/*        foreach(Scene s in SceneManager.GetAllScenes())
        {
            Debug.Log("Scene " + s.name);
        }*/

        Debug.Log("Additional scene name is " + scene.name);
        while (!scene.isLoaded)
        {
            Debug.Log(scene.isLoaded);
            yield return new WaitForSeconds(0.1f);
        }
        yield return new WaitForEndOfFrame();
        Debug.Log("Current scene name " + SceneManager.GetActiveScene().name);
        SceneManager.SetActiveScene(scene);
        Debug.Log("New scene name " + SceneManager.GetActiveScene().name);
        DialogueManager.SetDialoguePanel(false, true);
        Debug.Log("Siema");

        //yield return new WaitForSeconds(2);

        //ResumeGameFromFight();
    }

    // Resuming Game
    void ResumeDialogueSceneFromFight()
    {
        Debug.Log("Unload scene");
        Debug.Log(SceneManager.GetActiveScene().name);
        SceneManager.SetActiveScene(SceneManager.GetSceneAt(0));
        SceneManager.UnloadSceneAsync(SceneManager.GetSceneAt(1));
        new WaitForEndOfFrame();
        cameraToHide.gameObject.SetActive(true);
        eventSystemToHide.gameObject.SetActive(true);
        DialogueManager.SetDialoguePanel(true);
    }

    #endregion

    void SetNewEnemyDeck(DeckSO newDeck)
    {
        gameData.EnemyDeck = newDeck;
    }

    void SetNewGridData(GridData gridData)
    {
        gameData._GridData = gridData;
    }

    void ShowRewardCards()
    {
        /*
        Activate UI
        Set certain cards as rewards
        */
    }
}
