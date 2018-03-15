using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandMine : MonoBehaviour {

    [SerializeField] GameObject explosion;
    [SerializeField] float explosionSizeMultiplier = 1.5f;
    [SerializeField] float explosionForceMultiplier = 1.5f;

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            GameObject newExplosion = Instantiate(explosion, transform.position, Quaternion.identity);
            newExplosion.transform.localScale *= explosionSizeMultiplier;
            newExplosion.GetComponent<Explosion2>().blastForce *= explosionForceMultiplier;

            Destroy(gameObject);
        }
    }
}
