using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

	private Rigidbody2D __body;
    private bool __jumping;

    public float in_air_movement_acceleration;
    public float max_movement_speed;
    public float movement_acceleration;
    public float jump_velocity;
    public float static_decel;

    private float __movement_acceleration_cpy;

	void Awake(){
		__body = GetComponent<Rigidbody2D> ();
        __movement_acceleration_cpy = movement_acceleration;
	}

	// Use this for initialization
	void Start () {
        __jumping = false;
	}
	
	// Update is called once per frame
	void Update () {
        if (__jumping)
        {
            movement_acceleration = in_air_movement_acceleration;
        }
        else
        {
            movement_acceleration = __movement_acceleration_cpy;
        }

        if (Input.GetKey(KeyCode.A))
        {
            if ((__body.velocity.x * -1) < max_movement_speed)
            {
                __body.velocity -= new Vector2(movement_acceleration * Time.deltaTime, 0);
            }
        }
        else if (Input.GetKey(KeyCode.D))
        {
            if (__body.velocity.x < max_movement_speed)
            {
                __body.velocity += new Vector2(movement_acceleration * Time.deltaTime, 0);
            }
        }
        else
        {
            if (__body.velocity.x > 0)
            {
                __body.velocity -= new Vector2(static_decel * Time.deltaTime, 0);
            }
            else
            {
                __body.velocity += new Vector2(static_decel * Time.deltaTime, 0);
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            __body.velocity += new Vector2(0, jump_velocity);
        }
	}

    void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.tag == "Platform")
        {
            __jumping = true;
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Platform")
        {
            __jumping = false;
        }
    }
}
