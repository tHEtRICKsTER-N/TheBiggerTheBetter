using System.Collections.Generic;
using UnityEngine;

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

    public CustomEvents.FruitListEvent OnFruitListGenerated;

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

    [Header("List of Upcoming Fruits")]
    private List<GameObject> _fruitList = new List<GameObject>();

    #endregion

    #region Functions

    private void Start()
    {
        GenerateRandomFruitList();
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
        int randomIndex = Random.Range(1, System.Enum.GetValues(typeof(FruitType)).Length - 5);
        return (FruitType)randomIndex;
    }

    private void GenerateRandomFruitList()
    {
        List<GameObject> list = new List<GameObject>(3);

        for (int i = 0; i < 3; i++)
        {
            list.Add(GetFruitRefByEnum(GetRandomFruitType()));
        }

        //we have a list of 3 random fruits now
        //show them in the upcoming list

        _fruitList = list;
        OnFruitListGenerated?.Invoke(_fruitList);
    }

    #endregion
}
