using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManagerWinScreen : MonoBehaviour {

    public void GoToMainMenu()
    {
        Destroy(GameObject.FindGameObjectWithTag("Game Manager"));
        Destroy(GameObject.FindGameObjectWithTag("Audio"));
        SceneManager.LoadScene("Main");
    }

    public void Replay()
    {
        Destroy(GameObject.FindGameObjectWithTag("Game Manager"));
        Destroy(GameObject.FindGameObjectWithTag("Audio"));
        SceneManager.LoadScene("Play");
    }
}
