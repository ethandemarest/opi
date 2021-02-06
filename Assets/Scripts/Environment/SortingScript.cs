using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SortingScript : MonoBehaviour
{
    SpriteRenderer sp;
    Vector3 spriteBounds;
    float difference;

    private void Start()
    {
        sp = GetComponent<SpriteRenderer>();

        if (GetComponent<CircleCollider2D>() != null){
            spriteBounds = GetComponent<CircleCollider2D>().bounds.center;
        }
        else if (GetComponent<BoxCollider2D>() != null){
            spriteBounds = GetComponent<BoxCollider2D>().bounds.center;
        }
        else if (GetComponent<CapsuleCollider2D>() != null){
            spriteBounds = GetComponent<CapsuleCollider2D>().bounds.center;
        }
    }

    void Update()
    {
        Vector3 opiBounds = GameObject.Find("Opi").GetComponent<BoxCollider2D>().bounds.center;

        difference = (opiBounds.y - spriteBounds.y);
        sp.sortingOrder = Mathf.RoundToInt(difference);
    }
}
