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

    private void OnEnable()
    {
        GameManager.Instance.OnScoreChanged.AddListener(UpdateScoreUI);
        FruitHandler.Instance.OnFruitListGenerated.AddListener(UpdateListUI);
    }

    private void OnDisable()
    {
        if (!this.gameObject.scene.isLoaded)
            return;

        FruitHandler.Instance.OnFruitListGenerated.RemoveListener(UpdateListUI);
        GameManager.Instance.OnScoreChanged.RemoveListener(UpdateScoreUI);
    }

    private void UpdateScoreUI(int _score) { _scoreText.text = _score.ToString(); }

    private void UpdateListUI(List<GameObject> _fruitList)
    {
        for (int i = 0; i < 3; ++i)
        {
            _upcomingFruitsImageList[i].sprite = _fruitList[i].GetComponent<Fruit>().fruitSprite;
        }
    }

}
