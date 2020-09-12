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
        hitbox.enabled = false;

        opi = GameObject.Find("Opi");
        playerController = opi.GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        lastmoveX = playerController.lastMoveX;
        lastmoveY = playerController.lastMoveY;

        gameObject.GetComponent<BoxCollider2D>().offset = new Vector2(lastmoveX / 0.9f, lastmoveY / 1.2f);


        //Attacking Up & Down
        if (lastmoveX == 0)
        {
            hitbox.size = new Vector2(4f, 2f);
        }

        //Attacking Left & Right
        if (lastmoveY == 0)
        {
            hitbox.size = new Vector2(2.5f, 3f);
        }

        //Attacking Diagonally
        if (lastmoveX != 0 && lastmoveY != 0)
        {
            hitbox.size = new Vector2(2f, 2f);
        }
    }

    public void Attack()
    {
        hitbox.enabled = true;
        StartCoroutine("hitboxDelay");
    }

    IEnumerator hitboxDelay()
    {
        yield return new WaitForSeconds(0.2f);
        hitbox.enabled = false;
    }
}
