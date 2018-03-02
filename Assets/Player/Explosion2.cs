using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion2 : MonoBehaviour {

    float maxDist;
    [SerializeField] float forceMultiplier = 100;
    CircleCollider2D collider;

    int frameTimer = 0;

    float startTime;
    float maxTimeTilDeath = .2f;

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

        if (Time.time - startTime >= maxTimeTilDeath)
        {
            Destroy(gameObject);
        }
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        print("triggered");
        if (collision.GetComponent<Rigidbody2D>() != null)
        {
            if (collision.GetComponent<Rigidbody2D>().isKinematic == false)
            {
                Vector2 positionDifference = (Vector2)collision.transform.position - (Vector2)transform.position;
                float distance = positionDifference.magnitude;
                float forceToAdd = forceMultiplier * (maxDist - distance);
                collision.GetComponent<Rigidbody2D>().AddForce(positionDifference.normalized * forceToAdd, ForceMode2D.Impulse);
                collider.enabled = false;
            }
        }
    }
}
