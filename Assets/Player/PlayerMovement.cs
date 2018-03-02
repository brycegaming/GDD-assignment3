using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

	private Rigidbody2D body;
    private bool jumping;

    [SerializeField] float inAirMovementAcceleration;
    [SerializeField] float maxMoveSpeed;
    [SerializeField] float movementAcceleration;
    [SerializeField] float jumpVelocity;
    [SerializeField] float staticDecel;
    [SerializeField] float staticInAirDecel;

    Animator anim;
    float staticDecelCpy;
    float movementAccelerationCpy;

    private Vector3 spawnPoint;

	void Awake(){
		body = GetComponent<Rigidbody2D> ();
        staticDecelCpy = staticDecel;
        movementAccelerationCpy = movementAcceleration;
        spawnPoint = transform.position;
	}

	// Use this for initialization
	void Start ()
    {
        anim = GetComponent<Animator>();
        jumping = false;
	}
	
	// Update is called once per frame
	void Update () {
        if (jumping)
        {
            movementAcceleration = inAirMovementAcceleration;
            staticDecel = staticInAirDecel;
        }
        else
        {
            movementAcceleration = movementAccelerationCpy;
            staticDecel = staticDecelCpy;
        }

        if (Input.GetKey(KeyCode.A))
        {
            if ((body.velocity.x * -1) < maxMoveSpeed)
            {
                anim.SetBool("RunningLeft", true);
                anim.SetBool("RunningRight", false);
                body.velocity -= new Vector2(movementAcceleration * Time.deltaTime, 0);
            }
        }
        else if (Input.GetKey(KeyCode.D))
        {
            if (body.velocity.x < maxMoveSpeed)
            {
                anim.SetBool("RunningRight", true);
                anim.SetBool("RunningLeft", false);
                body.velocity += new Vector2(movementAcceleration * Time.deltaTime, 0);
            }
        }
        else
        {
            anim.SetBool("RunningRight", false);
            anim.SetBool("RunningLeft", false);

            if (body.velocity.x > 0)
            {
                body.velocity -= new Vector2(staticDecel * Time.deltaTime, 0);
            }
            else
            {
                body.velocity += new Vector2(staticDecel * Time.deltaTime, 0);
            }
        }

        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space))
        {
            if(!jumping)
                body.velocity += new Vector2(0, jumpVelocity);
        }
	}

    void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.tag == "Platform")
        {
            jumping = true;
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Platform")
        {
            jumping = false;
        }
    }
}
