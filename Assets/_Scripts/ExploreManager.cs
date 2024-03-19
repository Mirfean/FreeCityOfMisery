using PixelCrushers.DialogueSystem;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class ExploreManager : MonoBehaviour
{
    [SerializeField] CurrentGameData gameData;

    [SerializeField] Camera ExploreCamera;
    [SerializeField] EventSystem eventSystemToHide;
    [SerializeField] InventoryPlayer equipmentBar;

    public static Action StartFightScene;
    public static Action HideFight;
    public static Action<DeckSO> SetEnemyDeck;
    public static Action<GridData> SetGridData;

    bool _isPaused;

    [SerializeField] RewardCards rewardCards;

    public AsyncOperation sceneOperation;

    void OnEnable()
    {
        // Make the functions available to Lua: (Replace these lines with your own.)
        Lua.RegisterFunction(nameof(PauseGameToFight), this, SymbolExtensions.GetMethodInfo(() => PauseGameToFight()));
        Lua.RegisterFunction(nameof(ShowRewardCards), this, SymbolExtensions.GetMethodInfo(() => ShowRewardCards()));

        StartFightScene += PauseGameToFight;
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
        if (_isPaused) return;
            //Debug.Log(DialogueManager.CurrentActor.gameObject.name);
            _isPaused = true;

            ExploreCamera = Camera.main;
            eventSystemToHide = EventSystem.current;

            ExploreCamera.gameObject.SetActive(false);
            eventSystemToHide.gameObject.SetActive(false);
            equipmentBar.gameObject.SetActive(false);

            sceneOperation = SceneManager.LoadSceneAsync("BattleScene", LoadSceneMode.Additive);

            StartCoroutine(WaitUntilSceneLoaded());
    }

    void ResumeGameFromFight()
    {
        Debug.Log("Unload scene");
        Debug.Log(SceneManager.GetActiveScene().name);
        SceneManager.SetActiveScene(SceneManager.GetSceneAt(0));
        SceneManager.UnloadSceneAsync(SceneManager.GetSceneAt(1));
        new WaitForEndOfFrame();
        ExploreCamera.gameObject.SetActive(true);
        eventSystemToHide.gameObject.SetActive(true);
        DialogueManager.SetDialoguePanel(true);

        _isPaused = false;
    }

    IEnumerator WaitUntilSceneLoaded()
    {
        yield return new WaitForEndOfFrame();
        Scene scene = SceneManager.GetSceneAt(1);

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
        ExploreCamera.gameObject.SetActive(true);
        eventSystemToHide.gameObject.SetActive(true);
        DialogueManager.SetDialoguePanel(true);
    }

    #endregion

    void SetNewEnemyDeck(DeckSO newDeck)
    {
        Debug.Log("Setting new deck");
        Debug.Log(gameData);
        Debug.Log(gameData.EnemyDeck);
        Debug.Log(newDeck);
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
