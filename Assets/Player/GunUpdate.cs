using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum BowState
{
    Normal, Charging, Drawn
}

public class GunUpdate : MonoBehaviour {
    
    [SerializeField] GameObject bullet;
    [SerializeField] float firingForce = 5f;

    Vector3 arrowOffset;
    float chargeTimeStart;
    float maxChargeTime;
    GameObject playerObject;
    BowState state;
    Animator anim;

    void Awake()
    {
        //the player object is the one that controls this gun/bullet
        playerObject = transform.parent.gameObject;
    }

    void Start()
    {
        anim = GameObject.Find("Bow").GetComponent<Animator>();
        maxChargeTime = 0.6666f;
        arrowOffset = new Vector3(1f, -0.3f, 0f);
    }

    void Update()
    {
        // Handles mouse follow
        Vector2 dir = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        // HANDLES CLICK
        if (Input.GetMouseButtonDown(0))
        {
            anim.SetTrigger("StartDrawing");
            chargeTimeStart = Time.time;
        }

        if (Input.GetMouseButtonUp(0))
        {
            anim.SetTrigger("Release");
            float chargeTime = Time.time - chargeTimeStart;
            chargeTime = Mathf.Clamp(chargeTime, 0f, maxChargeTime);
            float thisFiringForce = (chargeTime / maxChargeTime) * firingForce;

            GameObject newBullet = GameObject.Instantiate(bullet);
            newBullet.transform.rotation = transform.localRotation;
            newBullet.transform.position = transform.TransformPoint(arrowOffset);
            newBullet.GetComponent<Bullet>().setPlayerObject(playerObject);

            Vector2 directionToFire = (Vector2)(Quaternion.Euler(0, 0, transform.eulerAngles.z) * Vector2.right);
            newBullet.GetComponent<Rigidbody2D>().velocity = directionToFire.normalized * thisFiringForce;
        }

        //flip the arm graphic if necessary
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (mousePosition.x > transform.position.x)
        {
            transform.localScale = new Vector2(1, 1);
        }
        else
        {
            transform.localScale = new Vector2(1, -1);
        }
    }
}
