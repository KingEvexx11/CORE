using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenuMethods : MonoBehaviour
{
    public void StartButton(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void QuitButton()
    {
        Debug.Log("Nuke the whole Generation!");
        Application.Quit();
    }
}
