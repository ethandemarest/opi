using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageEffects : MonoBehaviour
{
    public GameObject damage;
    GameObject opiCenter;
    Vector2 aim;
    Vector2 spawner;
    float angle;

    // Start is called before the first frame update
    void Start()
    {
        opiCenter = GameObject.Find("Opi Center");
    }

    // Update is called once per frame
    void Update()
    {
        aim.x = transform.position.x - opiCenter.transform.position.x;
        aim.y = transform.position.y - opiCenter.transform.position.y;
        angle = Mathf.Atan2(aim.y, aim.x) * Mathf.Rad2Deg;
        spawner = transform.position + Vector3.ClampMagnitude(aim.normalized*10, 2f) + new Vector3(0f, 1f, 0f);
    }

    void DamageEffect()
    {
        Instantiate(damage, spawner, Quaternion.Euler(0f, 0f, angle));
    }
}
 