using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    #region Instance

    private static GameManager _instance;

    //Public property to access the instance
    public static GameManager Instance
    {
        get
        {
            // If the instance doesn't exist, find or create it
            if (_instance == null)
            {
                _instance = FindObjectOfType<GameManager>();

                // If no instance exists in the scene, create a new one
                if (_instance == null)
                {
                    GameObject singletonObject = new GameObject("GameManager");
                    _instance = singletonObject.AddComponent<GameManager>();
                    DontDestroyOnLoad(singletonObject); // Don't destroy this object when loading new scenes
                }
            }
            return _instance;
        }
    }



    private void Awake()
    {
        // Ensure there's only one instance
        if (_instance != null && _instance != this)
        {
            _instance.gameObject.SetActive(false);
            return;
        }
    }
    #endregion


    #region Variables

    private int _score;
    [SerializeField] private bool _gameOver = false;
    public int scoreIncreaseValue;
    [SerializeField] private float _bufferTime = 2.5f;

    #endregion


    #region Events

    public CustomEvents.IntEvent OnScoreChanged;
    public event Action OnGameLose;
    public event Action OnGameWin;
    public event Action OnBufferTimeStart;
    public event Action OnBufferTimeEnd;

    #endregion


    #region Functions

    private void Start()
    {
        _gameOver = false;
        SetScore(0);
    }

    public void StartBufferTime() => OnBufferTimeStart?.Invoke();

    public void StopBufferTime() => OnBufferTimeEnd?.Invoke();

    public float GetBufferTime() => _bufferTime;

    public void IncrementScore(int value)
    {
        _score += value;
        OnScoreChanged?.Invoke(_score);
    }

    public void SetScore(int value)
    {
        _score = value;
        OnScoreChanged?.Invoke(_score);
    }

    public bool IsGameOver() => _gameOver;

    public void SetGameLoseTrue()
    {
        _gameOver = true;
        OnGameLose?.Invoke();
        DestroyAllFruits();
    }

    public void SetGameWinTrue()
    {
        _gameOver = true;
        OnGameWin?.Invoke();
        DestroyAllFruits();
    }

    public void DestroyAllFruits()
    {
        var fruits = GameObject.FindGameObjectsWithTag("Fruit");

        foreach (var fruit in fruits) Destroy(fruit);
    }

    public void Restart() { SceneManager.LoadScene(1); Time.timeScale = 1; }

    public void QuitGame() => Application.Quit();

    public void MainMenu() => SceneManager.LoadScene(0);
    #endregion
}
