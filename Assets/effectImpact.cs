using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class effectImpact : MonoBehaviour
{
    public int duration;
    int frame;

    private void Update()
    {
        frame++;

        if (frame >= duration)
        {
            Destroy(gameObject);
        }
    }
}
