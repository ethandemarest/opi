using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox : MonoBehaviour
{
    GameObject opi;
    PlayerController playerController;
    BoxCollider2D hitbox;

    float lastmoveX;
    float lastmoveY;

    // Start is called before the first frame update
    void Start()
    {
        hitbox = this.GetComponent<BoxCollider2D>();
        opi = GameObject.Find("Opi");
        playerController = opi.GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        lastmoveX = playerController.lastMoveX;
        lastmoveY = playerController.lastMoveY;

        if(Input.GetButtonDown("attack"))
        { 
            hitbox.enabled = true;
        }
        else
        {
            hitbox.enabled = false;
        }


        gameObject.GetComponent<BoxCollider2D>().offset = new Vector2(lastmoveX, lastmoveY/1.5f);

        if(lastmoveX == 0)
        {
            hitbox.size = new Vector2(2, 1);
        }

        if (lastmoveY == 0)
        {
            hitbox.size = new Vector2(1, 2);
        }

        if (lastmoveX != 0 && lastmoveY != 0)
        {
            hitbox.size = new Vector2(2, 2);
        }
      
        



    }
}
