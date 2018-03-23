using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpLocationAssigner : MonoBehaviour {

    GameObject[] powerupSpawnLocationArray;
    
        // Use this for initialization
    void Start ()
    {
        powerupSpawnLocationArray = GameObject.FindGameObjectsWithTag("PowerupSpawnPoint");

        int randomLocationInt = Random.Range(0, powerupSpawnLocationArray.Length);

        Vector3 randomLocation = powerupSpawnLocationArray[randomLocationInt].transform.position;

        transform.position = randomLocation;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
