using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Localization;

public class GameManager : PersistentSingleton<GameManager>
{
    /// <summary>
    /// delegate used to call when a level has been completely loaded. (i.e. including loading screen displays)
    /// </summary>
    public delegate void OnLevelFullyLoaded();

    private int m_gameState = 0;
    private IGameState m_ActiveState;
    /// <summary>
    /// instance of OnLevelFullyLoaded within the GameManager Instance
    /// </summary>
    public OnLevelLoad onLevelFullyLoaded;
    public OnLevelLoad onLevelTransitionStart;
    /// <summary>
    /// toggle for activating debug mode
    /// </summary>
    public bool m_DebugMode;

    private static bool m_FullyInitialized = false;

    private static float m_OriginalTimeScale = 1.0f;

    public static float OriginalTimeScale
    {
        get { return m_OriginalTimeScale; }
    }

    public static IGameState lastKnownState;

    public static bool FullyInitialized
    {
        get { return m_FullyInitialized; }
    }

    public static int GameState
    {
        get { return instance.m_gameState; }
    }

    public static IGameState activeState
    {
        get { return instance.m_ActiveState; }
    }

    void Update()
    {
        if (m_ActiveState != null)
        {
            m_ActiveState.StateUpdate();
        }
    }

    void FixedUpdate()
    {
        if (m_ActiveState != null)
        {
            m_ActiveState.StateFixedUpdate();
        }
    }

    private IEnumerator Start()
    {
        SetState(new LoadingState());
        //setup the Level manager
        while (!LevelManager.ReadyToLoadScenes)
        {
            yield return new WaitForEndOfFrame();
        }
        //setup Player Data
        while (!PlayerDataManager.instance.dataLoaded)
        {
            yield return new WaitForEndOfFrame();
        }

        //setup Localization Data
        if (LocalizationManager.localizationStatus == LocalizationStatus.validating)
        {
            yield return new WaitForEndOfFrame();
        }
        if (LocalizationManager.localizationStatus == LocalizationStatus.success)
        {
            LocalizationManager.instance.SetLocalizedText(PlayerDataManager.instance.PlayerData.gameSettings.localizationFileIndex);
        }
        //settings are to be initialized at this point:
        m_FullyInitialized = true;

        if (LocalizationManager.localizationStatus == LocalizationStatus.success)
        {
            if ((Debug.isDebugBuild && !Application.isEditor) || (Application.isEditor && m_DebugMode && LevelManager.instance.startAtInitialScene))
            {
                yield return LevelManager.instance.LoadSceneAndSetActive(LevelManager.DEBUG_INIT_SCENE);
            }
            else
            {
                if (LevelManager.instance.startAtInitialScene)
                {
                    yield return LevelManager.instance.LoadSceneAndSetActive(LevelManager.INIT_SCENE);
                }
                else
                {
                    yield return LevelManager.instance.LoadSceneAndSetActive(LevelManager.instance.StartScene);
                }
            }
            
        }
        else
        {
            yield return LevelManager.instance.LoadSceneAndSetActive(LevelManager.ERROR_SCENE);
        }
        //GUIManager.instance.loadingModal.FadeIn();
        //while (GUIManager.instance.loadingModal.fadeMode == FadeMode.In)
        //    yield return new WaitForEndOfFrame();
    }

    /// <summary>
    /// Changes the in-game time
    /// </summary>
    /// <param name="scale"></param>
    public void ChangeTimeScale(float scale)
    {
        Time.timeScale = scale;
    }

    /// <summary>
	/// sets the state.
	/// </summary>
	/// <param name="newState">New state.</param>
	public static void SetState(IGameState newState)
    {
        //Stop the current state if one is running.
        if (instance == null)
            return;
        if (instance.m_ActiveState != null)
            instance.m_ActiveState.StateStop();
        instance.m_ActiveState = newState;
        instance.m_gameState = instance.m_ActiveState.GetState();
        //Start the new state.
        instance.m_ActiveState.StateStart();
        Debug.Log("Current state: " + instance.m_ActiveState.GetState());
    }

    public static IGameState GetState(int index)
    {
        IGameState state;
        switch (index)
        {
            case 0:
                state = new LoadingState();
                break;
            case 1:
                state = new MenuState();
                break;
            case 2:
                state = new OverworldState();
                break;
            case 3:
                state = new CutsceneState();
                break;
            default:
                state = new LoadingState();
                break;
        }
        return state;
    }

    private IEnumerator LoadLevelCo(string sceneName)
    {
        SetState(new LoadingState());
        if (onLevelTransitionStart != null)
            onLevelTransitionStart();
        //GUIManager.instance.loadingModal.FadeOut();
        //while (!GUIManager.instance.loadingModal.transitionIsReady)
        //    yield return new WaitForEndOfFrame();
        //GUIManager.instance.DisableModalsForLoad();
        yield return StartCoroutine(LevelManager.instance.LoadLevelCo(sceneName));
        //GUIManager.instance.loadingModal.FadeIn();
        //while (GUIManager.instance.loadingModal.fadeMode == FadeMode.In)
        //    yield return new WaitForEndOfFrame();
        if (onLevelFullyLoaded != null)
            onLevelFullyLoaded();
    }

    public void LoadLevel(string sceneName)
    {
        StartCoroutine(LoadLevelCo(sceneName));
    }

    public static int LoadingState
    {
        get { return 0; }
    }

    public static int MenuState
    {
        get { return 1; }
    }

    public static int OverworldState
    {
        get { return 2; }
    }

    public static int CutsceneState
    {
        get { return 3; }
    }
}
