using EasyTransition;
using UnityEngine;

public class UIController : MonoBehaviour
{
    #region Instance

    public static UIController instance;

    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    #endregion


    [Header("Buttons Refs")]
    [SerializeField] private AnimatedButton _playButton;
    [SerializeField] private AnimatedButton _knowTheDevButton;
    [SerializeField] private AnimatedButton _quitButton;
    [SerializeField] private AnimatedButton _instructionsButton;
    [Space]
    [SerializeField] private TransitionSettings[] _transitions;

    private void Start()
    {
        SetButtonListeners();
    }

    private void SetButtonListeners()
    {
        _playButton.onClick.AddListener(() =>
        {
            TransitionManager.Instance().Transition(1, _transitions[Random.Range(0, _transitions.Length - 1)], .1f);
        });

        _knowTheDevButton.onClick.AddListener(() => { Application.OpenURL("https:://naimish.framer.ai"); });
        _instructionsButton.onClick.AddListener(() =>
        {

        });

        _quitButton.onClick.AddListener(() => { Application.Quit(); });
    }
}
