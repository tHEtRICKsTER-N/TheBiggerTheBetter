using System.Collections;
using UnityEngine;

public class SoundManager : MonoBehaviour
{

    #region Instance

    public static SoundManager _instance;

    // Public property to access the instance
    public static SoundManager Instance
    {
        get
        {
            // If the instance doesn't exist, find or create it
            if (_instance == null)
            {
                _instance = FindObjectOfType<SoundManager>();

                // If no instance exists in the scene, create a new one
                if (_instance == null)
                {
                    GameObject singletonObject = new GameObject("SoundManager");
                    _instance = singletonObject.AddComponent<SoundManager>();
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

    [Header("Audio Sources")]
    public AudioSource sfxAudioSource;
    public AudioSource bgmAudioSource;

    [Header("Audio Clips")]
    [SerializeField] private AudioClip[] _bgmClips;
    public AudioClip buttonSound;
    public AudioClip gameOverSound;
    public AudioClip dropSound;
    public AudioClip mergeSound;

    private int currentIndex = 0; // Index to keep track of the current clip
    private Coroutine _coroutine;

    private void OnEnable()
    {
        GameManager.Instance.OnGameEnd += GameOverSounds;
    }

    private void OnDisable()
    {
        if (!this.gameObject.scene.isLoaded)
            return;

        GameManager.Instance.OnGameEnd -= GameOverSounds;
    }

    private void Start()
    {
        bgmAudioSource.loop = true;
        PlayNextClip();
    }

    private void GameOverSounds()
    {
        StopCoroutine(_coroutine);

        sfxAudioSource.Stop();
        bgmAudioSource.Stop();

        sfxAudioSource.clip = gameOverSound;
        sfxAudioSource.Play();
    }

    private void PlayNextClip()
    {
        if (currentIndex < _bgmClips.Length)
        {
            // Assign the current clip to the AudioSource
            bgmAudioSource.clip = _bgmClips[currentIndex];
            // Play the audio
            bgmAudioSource.Play();
            // Wait for the clip to finish playing
            _coroutine = StartCoroutine(WaitForClipToEnd());
        }
        else
        {
            // All clips played, you can handle any logic here (e.g., loop back to the beginning)
            Debug.Log("All audio clips played!");
        }
    }

    private IEnumerator WaitForClipToEnd()
    {
        while (bgmAudioSource.isPlaying)
        {
            yield return null;
        }
        // Move to the next clip
        currentIndex++;
        PlayNextClip();
    }

    public void PlayButtonClickSound() => sfxAudioSource.PlayOneShot(buttonSound);

}
