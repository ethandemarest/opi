using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bow : MonoBehaviour
{
    public GameObject reticle;
    public GameObject opi;
    public Vector2 aimDirection;

    void Start()
    {
        opi = GameObject.Find("Opi");
        reticle = GameObject.Find("Reticle");
    }

    void Update()
    {
        aimDirection.x = (reticle.transform.position.x - opi.transform.position.x);
        aimDirection.y = (reticle.transform.position.y - opi.transform.position.y);
        float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0, 0, angle + 90f);
    }
}
