using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionUpdate : MonoBehaviour {

    private SphereCollider __collider;

    private float __radius;
    private float __max_explosion_time;
    private float __explosion_time;
    private float __time_offset;
    public float __constant_explosion_force;

    private Vector3 __start_radius;
    private Vector3 __end_radius;

	// Use this for initialization
	void Start () {
        __collider = GetComponent<SphereCollider>();
        __radius = transform.localScale.x;
        __max_explosion_time = .2f;
        __explosion_time = 0;
        __start_radius = new Vector3(0, 0, 0);
        __end_radius = new Vector3(__radius, __radius, 0);
        __time_offset = .1f;

        transform.localScale = new Vector3(0, 0, 0);
	}
	
	// Update is called once per frame
	void Update () {
        __explosion_time += Time.deltaTime;
        transform.localScale = Vector3.Lerp(transform.localScale, __end_radius, __explosion_time/__max_explosion_time);
        __collider.radius = transform.localScale.x;

        if (__explosion_time > __max_explosion_time + __time_offset)
        {
            GameObject.Destroy(this.gameObject);
        }

        GameObject[] all_players = GameObject.FindGameObjectsWithTag("Player");
        for (int i = 0; i < all_players.Length; i++)
        {
            Vector3 relative_location = all_players[i].transform.position - transform.position;
           
            //the player is within the explosion radius
            if (relative_location.magnitude < transform.localScale.x)
            {
                Rigidbody2D body = all_players[i].GetComponent<Rigidbody2D>();

                //radially away from the center of the explosion
                Vector3 direction = relative_location.normalized;
                body.AddForce(direction * __constant_explosion_force);
            }
        }
	}
}
