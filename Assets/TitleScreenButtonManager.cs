using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreenButtonManager : MonoBehaviour {

	public void GoToConceptArt()
    {
        SceneManager.LoadScene("Concept Art");
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("Main");
    }
}
