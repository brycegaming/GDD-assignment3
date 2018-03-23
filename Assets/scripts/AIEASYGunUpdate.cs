using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIEASYGunUpdate : MonoBehaviour
{
    PlayerPowerupController powerups;

    [SerializeField] GameObject bullet;
    [SerializeField] float firingForce = 5f;
    float minFireCharge = .08f;

    Vector3 arrowOffset;
    float chargeTimeStart;
    float maxChargeTime;
    GameObject playerObject;
    [SerializeField] GameObject mainPlayer;
    //BowState state;
    Animator anim;

    bool start = true;

    void Awake()
    {
        //the player object is the one that controls this gun/bullet
        playerObject = transform.parent.gameObject;
    }

    void Start()
    {
        powerups = transform.GetComponentInParent<PlayerPowerupController>(); 
        anim = transform.GetChild(0).GetComponentInChildren<Animator>();
        maxChargeTime = 0.5f;
        arrowOffset = new Vector3(1f, -0.3f, 0f);
    }

    void Update()
    {
        // Handles mouse follow
        Vector3 targetPos = mainPlayer.transform.position;
        float chargeTime = Time.time - chargeTimeStart;
        chargeTime = Mathf.Clamp(chargeTime, 0f, maxChargeTime);

        targetPos.y += (chargeTime);

        Vector2 dir = targetPos - transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        bool fire = false;
        int o = Random.Range(0, 210 - 3*((int)Mathf.Abs(transform.position.x - mainPlayer.transform.position.x)));

        float minTime = minFireCharge;
        int chance = 5;

        if (powerups.hasPowerup(PowerupType.RAPID_FIRE))
        {
            minTime = .15f;
            chance = 200;
        }

        if (o >= 1 && o <= chance && chargeTime > minTime)
        {
            fire = true;
        }

        // HANDLES CLICK
        if (start)
        {
            start = false;
            anim.SetTrigger("StartDrawing");
            chargeTimeStart = Time.time;
        }

        if (fire)
        {
            anim.SetTrigger("Release");
            chargeTime = Time.time - chargeTimeStart;

            if (powerups.hasPowerup(PowerupType.RAPID_FIRE))
            {
                chargeTime *= 8;
            }

            chargeTime = Mathf.Clamp(chargeTime, 0f, maxChargeTime);

            float thisFiringForce = (chargeTime / maxChargeTime) * firingForce;

            GameObject newBullet = GameObject.Instantiate(bullet);
            newBullet.transform.rotation = transform.localRotation;
            newBullet.transform.position = transform.TransformPoint(arrowOffset);
            newBullet.GetComponent<Bullet>().setPlayerObject(playerObject);

            Vector2 directionToFire = (Vector2)(Quaternion.Euler(0, 0, transform.eulerAngles.z) * Vector2.right);
            newBullet.GetComponent<Rigidbody2D>().velocity = directionToFire.normalized * thisFiringForce;

            if (powerups.hasPowerup(PowerupType.TRIPPLE_SHOT))
            {
                newBullet = GameObject.Instantiate(bullet);
                newBullet.transform.rotation = transform.localRotation;
                newBullet.transform.position = transform.TransformPoint(arrowOffset);
                newBullet.GetComponent<Bullet>().setPlayerObject(playerObject);

                directionToFire = (Vector2)(Quaternion.Euler(0, 0, transform.eulerAngles.z + 15) * Vector2.right);
                newBullet.GetComponent<Rigidbody2D>().velocity = directionToFire.normalized * thisFiringForce;

                newBullet = GameObject.Instantiate(bullet);
                newBullet.transform.rotation = transform.localRotation;
                newBullet.transform.position = transform.TransformPoint(arrowOffset);
                newBullet.GetComponent<Bullet>().setPlayerObject(playerObject);

                directionToFire = (Vector2)(Quaternion.Euler(0, 0, transform.eulerAngles.z - 15) * Vector2.right);
                newBullet.GetComponent<Rigidbody2D>().velocity = directionToFire.normalized * thisFiringForce;
            }

            start = true;
            chargeTimeStart = Time.time;
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
