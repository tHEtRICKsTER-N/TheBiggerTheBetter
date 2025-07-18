using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.VisualScripting;

public class FruitHandler : MonoBehaviour
{
    #region Instance

    public static FruitHandler _instance;

    // Public property to access the instance
    public static FruitHandler Instance
    {
        get
        {
            // If the instance doesn't exist, find or create it
            if (_instance == null)
            {
                _instance = FindObjectOfType<FruitHandler>();

                // If no instance exists in the scene, create a new one
                if (_instance == null)
                {
                    GameObject singletonObject = new GameObject("FruitHandler");
                    _instance = singletonObject.AddComponent<FruitHandler>();
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

    #region Events

    public CustomEvents.FruitListEvent OnFruitListUpdated;
    public event Action OnDelayEnd;

    #endregion

    #region Variables

    [Header("Fruit Prefabs")]
    [SerializeField] private GameObject _appleFruit;
    [SerializeField] private GameObject _berryFruit;
    [SerializeField] private GameObject _bananaFruit;
    [SerializeField] private GameObject _pineappleFruit;
    [SerializeField] private GameObject _cherryFruit;
    [SerializeField] private GameObject _guavaFruit;
    [SerializeField] private GameObject _orangeFruit;
    [SerializeField] private GameObject _pearFruit;
    [SerializeField] private GameObject _strawberryFruit;
    [SerializeField] private GameObject _tangerineFruit;

    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private int _upcomingSize;
    [SerializeField] private float _delayForNextFruit = 0.75f;

    public GameObject spawnEffect;

    [Header("Queue of Upcoming Fruits")]
    private Queue<GameObject> _fruitList = new Queue<GameObject>();

    private Fruit _currentFruit;
    Vector2 mousePos;
    private bool _canTakeMouseInput = true;

    #endregion

    #region Functions

    private void OnEnable()
    {
        GameManager.Instance.OnGameLose += OnGameLose;
        GameManager.Instance.OnGameWin += OnGameWin;
    }

    private void OnDisable()
    {
        if (!this.gameObject.scene.isLoaded)
            return;
        GameManager.Instance.OnGameLose -= OnGameLose;
        GameManager.Instance.OnGameWin -= OnGameWin;
    }

    private void Start()
    {
        StartCoroutine(DelayStart());
    }

    private void Update()
    {
        if (_currentFruit != null)
        {
            if (!_currentFruit.isDropped)
            {
                //we will try to find a location to drop it
                //fruit will follow the player's finger position

                if (_canTakeMouseInput)
                {
                    if (Input.GetMouseButton(0))
                    {
                        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                        mousePos = new Vector2(Mathf.Clamp(mousePos.x, -2.45f, 2.45f), 3.87f);
                        _currentFruit.transform.localPosition = mousePos;

                    }
                    if (Input.GetMouseButtonUp(0))
                    {
                        //that means the player has released the fruit, so it will fall

                        _canTakeMouseInput = false;
                        _currentFruit.AddComponent<Rigidbody2D>();

                        //play the sound
                        SoundManager.Instance.sfxAudioSource.PlayOneShot(SoundManager.Instance.dropSound);

                        StartCoroutine(SetIsDroppedTrueAndTakeAnotherFruit());
                    }
                }
            }
        }
    }

    private IEnumerator DelayStart()
    {
        yield return new WaitForSeconds(1);
        OnDelayEnd?.Invoke();
        GenerateRandomFruitList();
    }

    public void SetCanTakeMouseInput(bool canTakeMouseInput) => _canTakeMouseInput = canTakeMouseInput;

    private void OnGameLose() => _canTakeMouseInput = false;

    private void OnGameWin() => _canTakeMouseInput = false;

    private IEnumerator SetIsDroppedTrueAndTakeAnotherFruit()
    {
        yield return new WaitForSeconds(_delayForNextFruit);
        _currentFruit.isDropped = true;

        _canTakeMouseInput = true;

        //we will update the list
        SpawnLatestFruit();
    }

    public GameObject GetFruitRefByEnum(FruitType fruitType)
    {
        switch (fruitType)
        {
            case FruitType.None:
                return null;
            case FruitType.Apple:
                return _appleFruit;
            case FruitType.Berry:
                return _berryFruit;
            case FruitType.BIGBanana:
                return _bananaFruit;
            case FruitType.BIGPineapple:
                return _pineappleFruit;
            case FruitType.Cherry:
                return _cherryFruit;
            case FruitType.Guava:
                return _guavaFruit;
            case FruitType.OrangeFruit:
                return _orangeFruit;
            case FruitType.Pear:
                return _pearFruit;
            case FruitType.Strawberry:
                return _strawberryFruit;
            case FruitType.Tangerine:
                return _tangerineFruit;
            default: return null;
        }
    }

    public static FruitType GetRandomFruitType()
    {
        //just to get till apple
        int randomIndex = UnityEngine.Random.Range(1, System.Enum.GetValues(typeof(FruitType)).Length - 5);
        return (FruitType)randomIndex;
    }

    private void GenerateRandomFruitList()
    {
        Queue<GameObject> list = new Queue<GameObject>(_upcomingSize);

        for (int i = 0; i < _upcomingSize; i++)
        {
            list.Enqueue(GetFruitRefByEnum(GetRandomFruitType()));
        }

        //we have a list of upcoming size random fruits now
        //show them in the upcoming list

        _fruitList = list;
        SpawnLatestFruit();
        OnFruitListUpdated?.Invoke(_fruitList);
    }

    private void SpawnLatestFruit()
    {
        var frontFruit = _fruitList.Dequeue();
        _currentFruit = Instantiate(frontFruit, _spawnPoint.position, Quaternion.identity).GetComponent<Fruit>();
        GenerateNewFruit();
    }

    public void GenerateNewFruit()
    {
        _fruitList.Enqueue(GetFruitRefByEnum(GetRandomFruitType()));
        OnFruitListUpdated?.Invoke(_fruitList);
    }
    #endregion
}
