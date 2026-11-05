using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    public void PlayAgain()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainScene");
    }
    public void Exit()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("UIUX");
    }
}
