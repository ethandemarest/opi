using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bow : MonoBehaviour
{

    PlayerController playerController;
    public GameObject opi;
    public GameObject[] sprite;
    public float animTime;
    bool bowDrawn;

    public float targetOpac;
    public float targetPos;
    static float t = 0.0f;
    float tempAngle;

    void Start()
    {
        opi = GameObject.Find("Opi");
        playerController = opi.GetComponent<PlayerController>();
    }

    void Update()
    {
        if(bowDrawn == true)
        {
            Vector3 aim = new Vector3(playerController.lastMove.x, playerController.lastMove.y, 0.0f);
            float angle = Mathf.Atan2(aim.y, aim.x) * Mathf.Rad2Deg;
            tempAngle = Mathf.Atan2(aim.y, aim.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle + 90f);

            t += animTime * Time.deltaTime;
            targetOpac = Mathf.Lerp(0, 1, t);
            targetPos = Mathf.Lerp(-1, -2, t);
            foreach (GameObject s in sprite)
            {
                s.transform.GetComponent<SpriteRenderer>().material.color = new Color(1, 1, 1, targetOpac);
            }
            sprite[0].transform.localPosition = new Vector3(0, targetPos, 0);
            sprite[1].transform.localPosition = new Vector3(0, targetPos + 1f, 0);

        }
        else if(bowDrawn == false)
        {
            transform.rotation = Quaternion.Euler(0, 0, tempAngle + 90f);

            t += animTime * 1.5f * Time.deltaTime;
            targetOpac = Mathf.Lerp(1, 0, t);
            targetPos = Mathf.Lerp(-2, -3, t);

            foreach (GameObject s in sprite)
            {
                s.transform.GetComponent<SpriteRenderer>().material.color = new Color(1, 1, 1, targetOpac);
            }
            sprite[0].transform.localPosition = new Vector3(0, targetPos, 0);
            sprite[1].transform.localPosition = new Vector3(0, targetPos, 0);


        }
    }

    void BowDraw()
    {
        t = 0f;
        bowDrawn = true;
    }

    void BowDone()
    {
        t = 0f;
        bowDrawn = false;
    }
}



