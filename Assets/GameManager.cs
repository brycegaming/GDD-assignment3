using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    Text textComponentPlayer1;
    Text textComponentPlayer2;
    Text textComponentWin;
    int redDeaths;
    int blueDeaths;
    int killsToWin = 5;
    int redLevelsWon = 0;
    int blueLevelsWon = 0;
    bool gameWon;

	// Use this for initialization
	void Start ()
    {
        if (SceneManager.GetActiveScene().name == "Win")
        {
            Text winText = GameObject.Find("Win Text").GetComponent<Text>();

            if (redLevelsWon > blueLevelsWon)
            {
                winText.text = "RED WINS";
            }
            else if (redLevelsWon < blueLevelsWon)
            {
                winText.text = "BLUE WINS";
            }
            else
            {
                winText.text = "TIE GAME";
            }
        }

        Debug.Log("called start in game manager, dawg");

        DontDestroyOnLoad(gameObject);
        
        redDeaths = 0;
        blueDeaths = 0;
        gameWon = false;

        textComponentPlayer1 = GameObject.Find("Blue Death Counter").GetComponent<Text>();
        textComponentPlayer2 = GameObject.Find("Red Death Counter").GetComponent<Text>();
        textComponentWin = GameObject.Find("Win Text").GetComponent<Text>();
    }

    void OnLevelWasLoaded(int level)
    {
        if (SceneManager.GetActiveScene().name != "Play")
        {
            Start();
        }
    }

    // Update is called once per frame
    void Update ()
    {
		
	}





    public void AddBlueDeath()
    {
        if (!gameWon)
        {
            blueDeaths++;
            textComponentPlayer1.text = "Blue Deaths: " + blueDeaths.ToString();
            if (blueDeaths >= killsToWin)
            {
                textComponentWin.text = "RED WINS";
                redLevelsWon++;
                gameWon = true;
                Invoke("GoToNextLevel", 2);
            }
        }
    }
    public void AddRedDeath()
    {

        if (!gameWon)
        {
            redDeaths++;
            textComponentPlayer2.text = "Red Deaths: " + redDeaths.ToString();
            if (redDeaths >= killsToWin)
            {
                textComponentWin.text = "BLUE WINS";
                blueLevelsWon++;
                gameWon = true;
                Invoke("GoToNextLevel", 2);
            }
        }
    }

    private void GoToNextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
