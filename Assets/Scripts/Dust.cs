using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dust : MonoBehaviour
{
    public GameObject dust;
    GameObject opiCenter;

    private void Start()
    {
        opiCenter = GameObject.Find("Opi Center");
    }

    public void Step()
    {
        Instantiate(dust, opiCenter.transform.position, Quaternion.Euler(0f, 0f, 0f));

        string[] opiSound = new string[4];
        opiSound[0] = ("Footstep 1");
        opiSound[1] = ("Footstep 2");
        opiSound[2] = ("Footstep 3");
        opiSound[3] = ("Footstep 4");
        FindObjectOfType<AudioManager>().Play(opiSound[Random.Range(0, 4)]);
    }
}
