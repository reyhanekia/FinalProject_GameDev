using UnityEngine;
using UnityEngine.SceneManagement;

public class UIActions : MonoBehaviour
{
    public void StartAction()
    {
        SceneManager.LoadScene("Game");
    }

    public void GoToExtras()
    {
        SceneManager.LoadScene("Extras");
    }

    public void OpenInfo()
    {
        SceneManager.LoadScene("Credits");
    }

    public void CloseApp()
    {
        Application.Quit();
    }

    public void GoHome()
    {
        SceneManager.LoadScene("Home");
    }
}
