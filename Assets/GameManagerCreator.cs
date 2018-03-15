using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerCreator : MonoBehaviour {

    [SerializeField] GameObject gameManager;
    [SerializeField] GameObject audioObject;

	// Use this for initialization
	void Awake ()
    {
        GameObject audio = GameObject.FindGameObjectWithTag("Audio");
        GameObject scenesGameManager = GameObject.Find("Game Manager");
        if (scenesGameManager == null)
        {
            Instantiate(gameManager);
        }
        if (audio == null)
        {
            Instantiate(audioObject);
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
