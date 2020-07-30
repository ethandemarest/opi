using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticAsset : MonoBehaviour
{
    private SpriteRenderer sprite;
    private BoxCollider2D baseCollider;
    public int orderAdjust;

    // Update is called once per frame
    void Update()
    {
        
        // Get Compenents
        sprite = GetComponent<SpriteRenderer>();
        baseCollider = GetComponent<BoxCollider2D>();

        Vector3 opiBounds = GameObject.Find("Opi").GetComponent<BoxCollider2D>().bounds.center;
        Vector3 spriteBounds = GetComponent<BoxCollider2D>().bounds.center;
        
        //Sorting Equation
        sprite.sortingOrder = Mathf.Clamp(Mathf.RoundToInt(opiBounds.y - spriteBounds.y + 1),-10,10) + orderAdjust;

        if (sprite.sortingOrder == 0)
        {
            sprite.sortingOrder --;
        }


    }
}

