using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour {

    SphereCollider collider;

    float radius;
    float maxExplosionTime;
    float explosionTime;
    float timeOffset;
    float colliderRadius;
    float maxDist;

    [SerializeField] float explosiveForce;

    private Vector3 startRadius;
    private Vector3 endRadius;

	// Use this for initialization
	void Start ()
    {
        collider = GetComponent<SphereCollider>();
        colliderRadius = transform.localScale.x * collider.radius;
        maxDist = colliderRadius * 2;
        radius = transform.localScale.x;
        maxExplosionTime = .1f;
        explosionTime = 0;
        startRadius = new Vector3(0, 0, 0);
        endRadius = new Vector3(radius, radius, 0);
        timeOffset = .1f;

        transform.localScale = new Vector3(0, 0, 0);
	}
	
	// Update is called once per frame
	void Update () {
        explosionTime += Time.deltaTime;
        transform.localScale = Vector3.Lerp(transform.localScale, endRadius, explosionTime/maxExplosionTime);
        collider.radius = transform.localScale.x;

        if (explosionTime > maxExplosionTime + timeOffset)
        {
            Destroy(gameObject);
        }
	}


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Rigidbody2D>() != null)
        {
            if (collision.GetComponent<Rigidbody2D>().isKinematic == false)
            {
                Vector2 positionDifference = (Vector2)collision.transform.position - (Vector2)transform.position;

                //collision.GetComponent<Rigidbody2D>().AddForce()
            }
        }
    }
}
