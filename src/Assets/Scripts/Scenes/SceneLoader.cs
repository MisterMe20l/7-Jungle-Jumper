using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneHistoryEntry
{
    public Scene scene;
    public LoadSceneMode loadSceneMode;
}

public class SceneLoader : MonoBehaviour
{
    [SerializeField] GameObject m_loadingScreen;
    [SerializeField] Image m_progressImage;
    [SerializeField] float m_minStartLoading_s;
    [SerializeField] float m_minGameLoading_s;
    [SerializeField] AnimationCurve m_animationCurve;
    public static SceneLoader instance;

    public Stack<SceneHistoryEntry> sceneHistory = new();
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Start()
    {
        LoadSceneAsync(true, "Main Menu");
    }

    public void LoadScene(string _sceneName, LoadSceneMode _mode = LoadSceneMode.Single)
    {
        SceneManager.LoadScene(_sceneName, _mode);
    }

    public void LoadSceneAsync(bool _loadingScreen, string _sceneName, LoadSceneMode _mode = LoadSceneMode.Single)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(_sceneName, _mode);

        var minSeconds = (_sceneName == "In Game") ? m_minGameLoading_s : m_minStartLoading_s;

        if (_loadingScreen)
        {
            StartCoroutine(ProgressRoutine(operation, minSeconds));
        }
    }

    public void UnloadSceneAsync(string _sceneName)
    {
        SceneManager.UnloadSceneAsync(_sceneName);
    }

    public void OnSceneLoaded(Scene _loadedScene, LoadSceneMode _loadSceneMode)
    {
        sceneHistory.Push(new SceneHistoryEntry { scene = _loadedScene, loadSceneMode = _loadSceneMode });
    }

    public void Return()
    {
        SceneHistoryEntry lastScene = sceneHistory.Pop();
        SceneManager.UnloadSceneAsync(lastScene.scene);
        if (sceneHistory.Peek().loadSceneMode == LoadSceneMode.Single && lastScene.loadSceneMode == LoadSceneMode.Additive)
        {
            return;
        }
        
    }

    //public void PushScene(string _sceneName, LoadSceneMode _loadSceneMode = LoadSceneMode.Single)
    //{
    //    var currentScene = SceneManager.GetActiveScene().name;
    //    sceneHistory.Push(currentScene);
    //    SceneManager.UnloadSceneAsync(currentScene);
    //    SceneManager.LoadScene(_sceneName, _loadSceneMode);
    //    SceneManager.SetActiveScene(SceneManager.GetSceneByName(_sceneName));
    //}

    //public void PopScene()
    //{
    //    var currentScene = SceneManager.GetActiveScene().name;
    //    SceneManager.UnloadSceneAsync(currentScene);
    //    SceneManager.LoadScene(sceneHistory.Pop());
    //}

    private IEnumerator ProgressRoutine(AsyncOperation _operation, float _minSeconds)
    {
        m_loadingScreen.SetActive(true);
        m_progressImage.fillAmount = 0.0f;
        _operation.allowSceneActivation = false;
        var counter = 0.0f;

        while (!Mathf.Approximately(_operation.progress, 0.9f) || counter < _minSeconds)
        {
            yield return new WaitForEndOfFrame();
            counter += Time.deltaTime;
            float progress = Mathf.Min(_operation.progress, m_animationCurve.Evaluate(counter / _minSeconds));
            m_progressImage.fillAmount = progress;
        }

        _operation.allowSceneActivation = true;
        m_progressImage.fillAmount = 1.0f;
        m_loadingScreen.SetActive(false);
    }
}
