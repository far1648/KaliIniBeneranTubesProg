using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    public void PlayAgain()
    {
        SceneManager.LoadScene("MainScene");
    }
    public void Exit()
    {
        SceneManager.LoadScene("UIUX");
    }
}
