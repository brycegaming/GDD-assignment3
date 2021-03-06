﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion2 : MonoBehaviour {

    float maxDist;
    public float blastForce = 275;
    CircleCollider2D collider;

    int frameTimer = 0;

    float startTime;

	// Use this for initialization
	void Start ()
    {
        collider = GetComponent<CircleCollider2D>();
        maxDist = transform.localScale.x * collider.radius * 2;
        startTime = Time.time;
	}
	
	// Update is called once per frame
	void Update ()
    {
        frameTimer++;
        if (frameTimer >= 3)
        {
            collider.enabled = false;
        }
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Rigidbody2D>() != null)
        {
            if (collision.GetComponent<Rigidbody2D>().bodyType == RigidbodyType2D.Dynamic)
            {
                Vector3 collisionPosition = collision.transform.position;

                Vector2 positionDifference = collisionPosition - transform.position;
                positionDifference.y = Mathf.Abs(positionDifference.y);

                float distance = positionDifference.magnitude;
                float forceToAdd = blastForce * (maxDist - distance);
                collision.GetComponent<Rigidbody2D>().AddForce(positionDifference.normalized * forceToAdd, ForceMode2D.Impulse);
                collider.enabled = false;
            }
        }
    }
}
