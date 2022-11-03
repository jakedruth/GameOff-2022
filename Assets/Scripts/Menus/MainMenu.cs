using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        GameManager.Instance.LoadScene(1);
    }

    public void QuitGame()
    {
        GameManager.Instance.QuitGame();
    }
}
