using System.Collections.Generic;
using UnityEngine;

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


    #endregion


    #region Events

    public CustomEvents.IntEvent OnScoreChanged;

    #endregion


    #region Functions

    public void IncrementScore(int value)
    {
        _score += value;
        OnScoreChanged?.Invoke(_score);
    }

    public bool IsGameOver() => _gameOver;

    public void SetGameOver(bool gameOver) => _gameOver = gameOver;


    #endregion

}
