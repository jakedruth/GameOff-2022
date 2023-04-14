using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    [SerializeField] private bool _loadGameDataOnAwake;
    public GameData gameData;

    private CanvasGroup _pauseGroup;

    private bool _isPaused;
    public bool IsPaused
    {
        get
        {
            return _isPaused;
        }
        set
        {
            if (_isPaused == value)
                return;

            _isPaused = value;
            Time.timeScale = _isPaused ? 0 : 1;
            _pauseGroup.alpha = _isPaused ? 1 : 0;

            onIsPausedChanged.Invoke(_isPaused);
        }
    }
    public UnityEventBool onIsPausedChanged;

    public void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        gameData = new GameData();
        _pauseGroup = transform.GetComponentInChildren<CanvasGroup>();

        if (_loadGameDataOnAwake)
            LoadGameData();
    }

    public void Update()
    {
        // if (Input.GetKeyDown(KeyCode.Escape))
        // {
        //     if (SceneManager.GetActiveScene().buildIndex == 0)
        //     {
        //         QuitGame();
        //     }
        //     else
        //     {
        //         ToggleIsPaused();
        //     }
        // }
    }

    public bool ToggleIsPaused()
    {
        return IsPaused = !IsPaused;
    }

    public void LoadScene(int index)
    {
        SceneManager.LoadScene(index);
    }

    public void QuitGame()
    {
        CloseGame();
    }

    private void CloseGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }

    [System.Serializable]
    public class UnityEventBool : UnityEvent<bool> { }

    [ContextMenu("Save Game Data")]
    public void SaveGameData()
    {
        SaveLoadSystem.Save(this.gameData);
    }

    [ContextMenu("Load Game Data")]
    public void LoadGameData()
    {
        LoadGameData(true);
    }

    public void LoadGameData(bool displayLoadErrorLog = true)
    {
        gameData = SaveLoadSystem.Load(displayLoadErrorLog);
    }
}