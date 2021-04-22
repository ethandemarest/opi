﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntertactionPrompt : MonoBehaviour
{
    GameObject opi;
    SpriteRenderer sp;

    public float maxRange;
    public float minRange;

    public float distance;
    bool arrowOn;
    bool inside;

    public float offset;
    public float travelDistance;
    static float t = 0.0f;
    static float tt = 0.0f;
    public float startSpeed;
    public float accelRate;
    float acceleration;
    float targetOpac;

    void Start()
    {
        opi = GameObject.Find("Opi");
        sp = GetComponent<SpriteRenderer>();
        ArrowAnimate();
    }

    void Update()
    {
        distance = Vector3.Distance(transform.parent.transform.position, opi.transform.position);

        if (distance <= maxRange && distance > minRange && !inside)
        {
            inside = true;
            arrowOn = true;
            TimeReset();
        }
        if (distance <= minRange && inside)
        {
            inside = false;
            arrowOn = false;
            TimeReset();
        }
        else if (distance > maxRange && inside)
        {
            inside = false;
            arrowOn = false;
            TimeReset();
        }

        acceleration -= accelRate * Time.deltaTime;
        if(acceleration <= -startSpeed)
        {
            ArrowAnimate();
        }

        t += acceleration * Time.deltaTime;

        transform.localPosition = Vector3.Lerp(new Vector3(0f, offset, 0f), new Vector3(0f, travelDistance+offset, 0f), t);


        
        if (arrowOn)
        {
            tt += 3f * Time.deltaTime;
            targetOpac = Mathf.Lerp(0, 1, tt);
            sp.material.color = new Color(1, 1, 1, targetOpac);
        }
        if (!arrowOn)
        {
            tt += 3f * Time.deltaTime;
            targetOpac = Mathf.Lerp(1, 0, tt);
            sp.material.color = new Color(1, 1, 1, targetOpac);
        }
        


    }

    void ArrowAnimate()
    {
        t = 0f;
        acceleration = startSpeed;
    }
    void TimeReset()
    {
        tt = 0f;
    }
    void ArrowOff()
    {
        print("bye bye");
        Destroy(gameObject);
    }
}