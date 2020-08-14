using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitDetection : MonoBehaviour
{

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
    