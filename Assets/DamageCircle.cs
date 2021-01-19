using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageCircle : MonoBehaviour
{
    int frame;
    public int duration = 10;

    void Update()
    {
        frame++;
        if(frame >= duration)
        {
            Destroy(this.gameObject);
        }
    }
}
