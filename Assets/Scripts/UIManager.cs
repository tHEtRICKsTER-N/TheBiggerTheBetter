using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    #region Instance

    private static UIManager _instance;

    // Public property to access the instance
    public static UIManager Instance
    {
        get
        {
            // If the instance doesn't exist, find or create it
            if (_instance == null)
            {
                _instance = FindObjectOfType<UIManager>();

                // If no instance exists in the scene, create a new one
                if (_instance == null)
                {
                    GameObject singletonObject = new GameObject("UIManager");
                    _instance = singletonObject.AddComponent<UIManager>();
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
            Destroy(gameObject);
            return;
        }
    }
    #endregion

    [Header("UI")]
    [SerializeField] private Text _scoreText;
    [SerializeField] private List<Image> _upcomingFruitsImageList;
    [SerializeField] private Text _timeLeft;
    [SerializeField] private GameObject _timeLeftImage;
    [SerializeField] private Image _gameOverImage;
    [SerializeField] private Image _gameWinImage;
    [SerializeField] private Text _goodLuckText;

    private int _currentBufferTime;
    private Coroutine _coroutine;

    private void OnEnable()
    {
        _currentBufferTime = GameManager.Instance.GetBufferTime();

        FruitHandler.Instance.OnDelayEnd += GoodLuckOff;
        GameManager.Instance.OnGameLose += SetGameOverUI;
        GameManager.Instance.OnGameWin += SetGameWinUI;
        GameManager.Instance.OnBufferTimeStart += BufferTimeON;
        GameManager.Instance.OnBufferTimeEnd += BufferTimeOFF;
        GameManager.Instance.OnScoreChanged.AddListener(UpdateScoreUI);
        FruitHandler.Instance.OnFruitListUpdated.AddListener(UpdateListUI);
    }

    private void OnDisable()
    {
        if (!this.gameObject.scene.isLoaded)
            return;

        FruitHandler.Instance.OnDelayEnd -= GoodLuckOff;
        GameManager.Instance.OnGameLose -= SetGameOverUI;
        GameManager.Instance.OnGameWin -= SetGameWinUI;
        GameManager.Instance.OnBufferTimeStart -= BufferTimeON;
        GameManager.Instance.OnBufferTimeEnd -= BufferTimeOFF;
        FruitHandler.Instance.OnFruitListUpdated.RemoveListener(UpdateListUI);
        GameManager.Instance.OnScoreChanged.RemoveListener(UpdateScoreUI);
    }

    private void Start()
    {
        _goodLuckText.gameObject.SetActive(true);
        _gameOverImage.gameObject.SetActive(false);
        _timeLeftImage.SetActive(false);
    }

    private void GoodLuckOff() => _goodLuckText.gameObject.SetActive(false);

    private void BufferTimeON()
    {
        StartCoroutine(BufferTimeCountdown());
        _currentBufferTime = GameManager.Instance.GetBufferTime();
    }

    private void SetGameOverUI()
    {
        Time.timeScale = 0;

        _gameOverImage.gameObject.SetActive(true);
        _timeLeftImage.SetActive(false);
    }

    private void SetGameWinUI()
    {
        Time.timeScale = 0;

        _gameWinImage.gameObject.SetActive(true);
        _timeLeftImage.SetActive(false);
    }

    private void BufferTimeOFF()
    {
        StopAllCoroutines();
        GameManager.Instance.SetBufferTime(GameManager.Instance.GetBufferTime());
        _timeLeftImage.SetActive(false);
    }

    private IEnumerator BufferTimeCountdown()
    {
        //Debug.Log("Countdown Started !!");
        _timeLeft.text = _currentBufferTime.ToString();

        _timeLeftImage.SetActive(true);

        while (_currentBufferTime > 0)
        {
            yield return new WaitForSeconds(1);
            _currentBufferTime -= 1;
            _timeLeft.text = _currentBufferTime.ToString();
        }

        GameManager.Instance.SetGameLoseTrue();
    }

    private void UpdateScoreUI(int _score) { _scoreText.text = _score.ToString(); }

    private void UpdateListUI(Queue<GameObject> _fruitList)
    {
        int i = 0;
        foreach (GameObject _fruit in _fruitList)
        {
            _upcomingFruitsImageList[i].sprite = _fruit.GetComponent<Fruit>().fruitSprite;
            i++;
        }
    }

}
