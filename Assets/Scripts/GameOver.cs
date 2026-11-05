using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    public void PlayAgain()
    {
        SceneManager.LoadScene("MainScene");
        Time.timeScale = 1;
    }
    public void Exit()
    {
        SceneManager.LoadScene("UIUX");
        Time.timeScale = 1;
    }
}
