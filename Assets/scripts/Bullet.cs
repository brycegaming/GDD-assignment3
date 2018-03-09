using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    [SerializeField] GameObject explosion;

    public float bulletSpeed = 40;
    GameObject bow;
    
    Rigidbody2D rb2d;
    GunUpdate bowScript;

    float maxTime;
    float time;

    GameObject playerObject;

    public void setPlayerObject(GameObject player)
    {
        playerObject = player;
    }

    void Start()
    {
        maxTime = 5.0f;
        time = 0;
        rb2d = GetComponent<Rigidbody2D>();
    }
	
	// Update is called once per frame
	void Update () {
        time += Time.deltaTime;

        if (time > maxTime)
        {
            GameObject.Destroy(this.gameObject);
        }
        if (true)
        {

        }

        if (rb2d.velocity.y >= 0f)
        {
            float angleToBe = Vector2.Angle(Vector2.right, rb2d.velocity);
            transform.rotation = Quaternion.Euler(0, 0, angleToBe);
        }
        else
        {
            float angleToBe = -1f * Vector2.Angle(Vector2.right, rb2d.velocity);
            transform.rotation = Quaternion.Euler(0, 0, angleToBe);
        }
	}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //create an explosion object and destroy this object

        if (collision.gameObject != playerObject)
        {
            GameObject newExplosion = Instantiate(explosion);
            newExplosion.transform.position = transform.position;
            Destroy(gameObject);
        }
    }
    
}
