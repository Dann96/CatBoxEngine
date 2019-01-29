using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public delegate void OnLevelLoad();

public class LevelManager : PersistentSingleton<LevelManager>
{
    /// <summary>
    /// The name of the persistent scene.
    /// </summary>
    public const string PERSISTENT_SCENE = "Persistent";
    /// <summary>
    /// The name of the first scene to be loaded.
    /// </summary>
    public const string INIT_SCENE = "Menu_Test";//"Level_Menu01";
    /// <summary>
    /// The name of the first scene to be loaded if in debug Mode
    /// </summary>
    public const string DEBUG_INIT_SCENE = "Level_Debug_Menu";
    /// <summary>
    /// The name of the scene to be loaded if the localization system fails
    /// </summary>
    public const string ERROR_SCENE = "Level_Error";

    public static string currentSceneName
    {
        get { return SceneManager.GetActiveScene().name; }
    }

    public static float loadProgress
    {
        get { return ((m_AsyncUnload != null ? m_AsyncUnload.progress : 1.0f) + (m_Asyncload != null ? m_Asyncload.progress : 0.0f)) / 2; }
    }

    public bool startAtInitialScene = false;

    private bool b_readyToLoadScenes = false;
    private bool b_LoadingScene = false;
    private bool b_FinishedLoadingScene = false;

    private string m_startScene = string.Empty;

    private static AsyncOperation m_AsyncUnload = null;
    private static AsyncOperation m_Asyncload = null;

    //public delegate void OnLevelLoad();

    public OnLevelLoad onLevelLoaded;
    //public OnLevelLoad onLevelTransitionStart;

    public static bool LoadingScene
    {
        get { return instance ? instance.b_LoadingScene : false; }
    }

    /// <summary>
	/// is the Level Manager ready to load and unload scenes?
	/// </summary>
	public static bool ReadyToLoadScenes
    {
        get { return instance ? instance.b_readyToLoadScenes : false; }
    }

    public static bool FinishedLoadingScene
    {
        get { return instance ? instance.b_FinishedLoadingScene : false; }
    }

    public string StartScene
    {
        get { return m_startScene; }
    }

    private IEnumerator Start()
    {
        Application.backgroundLoadingPriority = ThreadPriority.Low;
        if (SceneManager.GetActiveScene().name != PERSISTENT_SCENE)
        {
            if (!startAtInitialScene)
                m_startScene = SceneManager.GetActiveScene().name;
            SceneManager.LoadSceneAsync(PERSISTENT_SCENE);
            yield return new WaitForEndOfFrame();
        }
        b_readyToLoadScenes = true;
        b_FinishedLoadingScene = true;
        Debug.Log("Level Manager has been successfully initialized");
    }

    /// <summary>
    /// Loads a scene and sets it as active.
    /// </summary>
    public IEnumerator LoadSceneAndSetActive(string sceneName)
    {
        if (!SceneCanBeLoaded(sceneName))
        {
            Debug.Log("Error: Scene " + sceneName + " cannot be loaded as it was not found in the SceneManager Build List.");
            yield break;
        }
        if (!b_LoadingScene)
            b_LoadingScene = true;
        m_Asyncload = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

        yield return m_Asyncload;
        Scene newlyLoadedScene = SceneManager.GetSceneAt(SceneManager.sceneCount - 1);
        SceneManager.SetActiveScene(newlyLoadedScene);

        Debug.Log("New Level Loaded: " + newlyLoadedScene.name);

        if (onLevelLoaded != null)
            onLevelLoaded();

        yield return new WaitForSeconds(0.1f);
        m_Asyncload = null;
        b_LoadingScene = false;
    }

    /// <summary>
	/// Changes the scene (Coroutine).
	/// </summary>
	public IEnumerator LoadLevelCo(string sceneName)
    {
        if (!SceneCanBeLoaded(sceneName))
        {
            Debug.Log("Error: Scene " + sceneName + " cannot be loaded as it was not found in the SceneManager Build List.");
            yield break;
        }
        b_LoadingScene = true;
        m_AsyncUnload = SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().buildIndex);
        yield return m_AsyncUnload;
        m_AsyncUnload = null;
        yield return StartCoroutine(LoadSceneAndSetActive(sceneName));
    }
    /// <summary>
    /// Changes the scene.
    /// </summary>
    /// <param name="sceneName">Scene name.</param>
    public static void LoadLevel(string sceneName)
    {
        instance.StartCoroutine(instance.LoadLevelCo(sceneName));
    }

    private bool SceneCanBeLoaded(string sceneName)
    {
        return SceneUtility.GetBuildIndexByScenePath(sceneName) >= 0;
    }
}