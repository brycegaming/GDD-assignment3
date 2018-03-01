using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletUpdate : MonoBehaviour {
    public float bullet_speed = 40;
    private GameObject __bow;

    private SpriteRenderer __renderer;
    private Rigidbody2D __body;
    private GunUpdate __bow_script;

    private float __max_time;
    private float __time;
    private float __explosion_radius;

    public void set_bow(GameObject obj) {
        this.__bow = obj;
    }

    void Start()
    {
        transform.position = __bow.transform.position;
        __explosion_radius = 2;
        __max_time = 1.0f;
        __time = 0;
        __bow_script = __bow.GetComponent<GunUpdate>();
        __renderer = GetComponent<SpriteRenderer>();
        __body = GetComponent<Rigidbody2D>();

        Vector3 right = __bow.transform.right;
        __body.velocity = new Vector2(right.x, right.y) * bullet_speed;
    }
	
	// Update is called once per frame
	void Update () {
        __time += Time.deltaTime;

        if (__time > __max_time)
        {
            GameObject.Destroy(this.gameObject);
        }
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        //create an explosion object and destroy this object
        if ((other.gameObject.tag == "Player" && other.gameObject != __bow.transform.parent.gameObject) || other.gameObject.tag == "Wall" || other.gameObject.tag == "Platform")
        {
            Vector3 center = this.transform.position;
            GameObject explosion = GameObject.Instantiate(GameObject.FindGameObjectWithTag("Explosion"));
            explosion.transform.position = center;
            explosion.GetComponent<ExplosionUpdate>().enabled = true;
            explosion.GetComponent<SphereCollider>().enabled = true;
            explosion.transform.localScale = new Vector3(__explosion_radius, __explosion_radius, 0);
            explosion.GetComponent<SpriteRenderer>().enabled = true;

            GameObject.Destroy(this.gameObject);
        }
    }
}
