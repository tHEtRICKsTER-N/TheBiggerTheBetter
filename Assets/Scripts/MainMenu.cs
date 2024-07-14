using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private AudioSource _sfxSource;
    [SerializeField] private AudioClip _btnSound;

    public void StartGame() => SceneManager.LoadScene(1);

    public void QuitGame() => Application.Quit();

    public void PlayButtonSound() => _sfxSource.PlayOneShot(_btnSound);

    public void KnowMe() => Application.OpenURL("https://naimish.netlify.app");
}
