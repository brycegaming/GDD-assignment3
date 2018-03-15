using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeathManager : MonoBehaviour {

    [SerializeField]private GameObject playerObject;

    private int deathCount;
    private string preText;
    [SerializeField]private Text text;
    int maxDeaths;

    public void resetDeaths()
    {
        deathCount = 0;
    }

    public int getDeaths()
    {
        return deathCount;
    }

    public void addDeath()
    {
        deathCount++;
        text.text = preText + " " + deathCount;
    }

	// Use this for initialization
	void Start () {
        maxDeaths = 8;
        resetDeaths();
        preText = text.text;
	}

    void Update()
    {
        if (deathCount >= maxDeaths)
        {
            if (playerObject.GetComponent<PlayerMovement>() != null)
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene("Lose");
            }
            else
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene("Win");
            }
        }
    }
}
