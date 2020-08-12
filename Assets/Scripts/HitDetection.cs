using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitDetection : MonoBehaviour
{
    public float bounce = 2;
    public float bounceSpeed = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
   
    }

    // Update is called once per frame
    void Update()
    {
        //spiritPosition = this.GetComponent<Transform>().position;
        //opiPosition = opi.GetComponent<Transform>().position;
    }

    void OnTriggerEnter2D(Collider2D other)
    {

        if (other.CompareTag("Attack"))
        {

            this.SendMessage("Hit");
        }
    }



    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Attack"))
        {

        }
    }
}
    