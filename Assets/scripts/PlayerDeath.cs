using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeath : MonoBehaviour {

    GameManager gameManager;
    private Vector3 spawnPoint;
    private Rigidbody2D body;
    [SerializeField] bool isPlayerOne;
    [SerializeField] GameObject respawnAnimationObject;

    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("Game Manager").GetComponent<GameManager>();
        spawnPoint = transform.position;
        body = GetComponent<Rigidbody2D>();
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Respawn")
        {
            Instantiate(respawnAnimationObject, transform.position, Quaternion.identity);

            transform.position = spawnPoint;
            body.velocity = new Vector3(0, 0, 0);

            if (isPlayerOne)
            {
                gameManager.AddBlueDeath();
            }
            else
            {
                gameManager.AddRedDeath();
            }
        }
    }
}
