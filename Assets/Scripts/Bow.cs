using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bow : MonoBehaviour
{
    PlayerController playerController;
    public GameObject opi;
    public GameObject reticleSon;
    public GameObject[] sprite;
    public GameObject newSprite;
    public float transition;
    Vector3 destination;

    void Start()
    {
        transition = 0;
        opi = GameObject.Find("Opi");
        playerController = opi.GetComponent<PlayerController>();
    }

    void Update()
    {
        Vector3 aim = new Vector3(playerController.lastMove.x, playerController.lastMove.y, 0.0f);
        float angle = Mathf.Atan2(aim.y, aim.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle + 90f);

        //newSprite.transform.GetComponent<SpriteRenderer>().material.color = new Color(1, 1, 1, transition);

        foreach (GameObject s in sprite)
        {
            s.transform.GetComponent<SpriteRenderer>().material.color = new Color(1, 1, 1, transition);
        }

    }

    void BowDraw()
    {
        //destination = newSprite.transform.position;

        print("draw");
        StartCoroutine(FadeTo(1f, 0.2f));
        //StartCoroutine(Move(1));
    }

    void BowDone()
    {
        //destination = newSprite.transform.position;

        print("done");
        StartCoroutine(FadeTo(0f, 0.2f));
        ///StartCoroutine(Move(-1));
    }

    IEnumerator FadeTo(float aValue, float aTime)
    {
        float alpha = newSprite.transform.GetComponent<SpriteRenderer>().material.color.a;
        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / aTime)
        {
            transition = Mathf.Lerp(alpha, aValue, t);
            yield return null;
        }
    }


    /*
    IEnumerator Move(float distance)
    {
        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / 1f)
        {
            newSprite.transform.position = Vector3.Lerp(destination, destination + new Vector3(0f, 4f, 0f), distance);
            yield return null;
        }
    }
    */
}



