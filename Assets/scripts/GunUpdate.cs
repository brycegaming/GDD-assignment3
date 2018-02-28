using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunUpdate : MonoBehaviour {

    void Update()
    {
        var dir = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        transform.position = GameObject.FindGameObjectWithTag("Player").transform.position + new Vector3(0 ,.2f, 0);
    }
}
