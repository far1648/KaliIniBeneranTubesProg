using UnityEngine;
using System.Collection;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
public class mainmenu : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadSceneAsync(1);
    }
}
