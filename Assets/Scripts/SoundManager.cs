using System.Collections;
using UnityEditor;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [Header("Audio Sources")]
    [SerializeField] private AudioSource _sfxAudioSource;
    [SerializeField] private AudioSource _bgmAudioSource;

    [Header("Audio Clips")]
    [SerializeField] private AudioClip[] _bgmClips;
    [SerializeField] private AudioClip _buttonSound;
    [SerializeField] private AudioClip _gameOverSound;
    [SerializeField] private AudioClip _dropSound;
    [SerializeField] private AudioClip _mergeSound;

    private int currentIndex = 0; // Index to keep track of the current clip


    private void Start()
    {
        _bgmAudioSource.loop = true;
        PlayNextClip();
    }

    private void PlayNextClip()
    {
        if (currentIndex < _bgmClips.Length)
        {
            // Assign the current clip to the AudioSource
            _bgmAudioSource.clip = _bgmClips[currentIndex];
            // Play the audio
            _bgmAudioSource.Play();
            // Wait for the clip to finish playing
            StartCoroutine(WaitForClipToEnd());
        }
        else
        {
            // All clips played, you can handle any logic here (e.g., loop back to the beginning)
            Debug.Log("All audio clips played!");
        }
    }

    private IEnumerator WaitForClipToEnd()
    {
        while (_bgmAudioSource.isPlaying)
        {
            yield return null;
        }
        // Move to the next clip
        currentIndex++;
        PlayNextClip();
    }

    public void PlayButtonClickSound()
    {
        if (!_sfxAudioSource.isPlaying)
        {
            _sfxAudioSource.PlayOneShot(_buttonSound);
        }
    }
}
