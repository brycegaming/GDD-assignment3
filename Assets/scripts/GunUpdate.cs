using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunUpdate : MonoBehaviour {

    public float max_charge_time;
    private float __charge_time;
    private GameObject __player_object;

    void Awake()
    {
        //the player object is the one that controls this gun/bullet
        __player_object = transform.parent.gameObject;
    }

    void Start()
    {
        __charge_time = max_charge_time;
    }

    void Update()
    {
        Vector2 dir = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        transform.position = __player_object.transform.position + new Vector3(0 ,.2f, 0);

        __charge_time += Time.deltaTime;

        if (Input.GetMouseButton(0))
        {

            if (__charge_time >= max_charge_time)
            {
                GameObject new_bullet = GameObject.Instantiate(GameObject.FindGameObjectWithTag("Arrow"));
                new_bullet.GetComponent<SpriteRenderer>().enabled = true;
                new_bullet.GetComponent<BulletUpdate>().enabled = true;
                new_bullet.GetComponent<Rigidbody2D>().gravityScale = 0.0f;
                new_bullet.GetComponent<BoxCollider2D>().enabled = true;
                new_bullet.tag = "ArrowCpy";
                new_bullet.GetComponent<BulletUpdate>().bullet_speed += Mathf.Abs(__player_object.GetComponent<Rigidbody2D>().velocity.x);
                new_bullet.GetComponent<BulletUpdate>().set_bow(__player_object.transform.GetChild(0).transform.gameObject);

                __charge_time = 0;
            }
        }
    }
}
