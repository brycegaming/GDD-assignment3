using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeath : MonoBehaviour {
    private Vector3 spawnPoint;
    private Rigidbody2D body;
    [SerializeField] private DeathManager deaths;

    void Awake()
    {
        spawnPoint = transform.position;
        body = GetComponent<Rigidbody2D>();
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Respawn")
        {
            transform.position = spawnPoint;
            body.velocity = new Vector3(0, 0, 0);

            deaths.addDeath();
        }
    }
}
