using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SortingScript : MonoBehaviour
{
    public float positionZ = 0;

    // Update is called once per frame
    void Update()
    {
        float positionX = this.transform.position.x;
        float positionY = this.transform.position.y;


        Vector3 opiBounds = GameObject.Find("Opi").GetComponent<BoxCollider2D>().bounds.center;
        Vector3 spriteBounds = GetComponent<BoxCollider2D>().bounds.center;

        //Sorting Equation
        positionZ = -(opiBounds.y - spriteBounds.y);
        this.transform.position = new Vector3(positionX, positionY, positionZ);
    }
}
