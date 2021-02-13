using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dust : MonoBehaviour
{
    public GameObject dust;
    GameObject opiCenter;
    PlayerController pc;
    int frame;

    // Start is called before the first frame update
    void Start()
    {
        opiCenter = GameObject.Find("Opi Center");
        pc = GameObject.Find("Opi").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if(pc.movement.sqrMagnitude > 0.01f)
        {
            frame++;

            if (frame == 15)
            {
                Instantiate(dust, opiCenter.transform.position, Quaternion.Euler(0f, 0f, 0f));
                frame = 0;
            }
        }
    }

}
