using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    private bool _isPaused;

    [SerializeField] private GameObject _pauseMenu;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Pause(InputAction.CallbackContext ctx)
    {
        _isPaused = true;
        _pauseMenu.SetActive(true);

        Time.timeScale = 0;
    }

    public void Resume(InputAction.CallbackContext ctx)
    {
        _isPaused = false;
        _pauseMenu.SetActive(false);

        Time.timeScale = 1;
    }

    public void Toggle()
    {
        _isPaused = !_isPaused;
        _pauseMenu.SetActive(_isPaused);

        Time.timeScale = _isPaused ? 0 : 1;
    }

    public void Quit()
    {
        Debug.Log("Quiting!");
        Application.Quit();
    }

    public void LoadLevel(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void ResetLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
