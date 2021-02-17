using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bow : MonoBehaviour
{
    PlayerController playerController;
    public GameObject opi;
    Transform delay;
    Vector2 lastMove;
    public GameObject reticleSon;

    void Start()
    {
        opi = GameObject.Find("Opi");
        playerController = opi.GetComponent<PlayerController>();
    }

    void Update()
    {
        Vector3 aim = new Vector3(playerController.lastMove.x, playerController.lastMove.y, 0.0f);
        float angle = Mathf.Atan2(aim.y, aim.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle + 90f);

    }
}
 