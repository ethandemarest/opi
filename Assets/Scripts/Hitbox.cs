using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox : MonoBehaviour
{
    public GameObject currentObject = null;

    GameObject opi;
    PlayerController playerController;
    BoxCollider2D hitbox;

    public bool contact;
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
        transform.position = opi.transform.position + new Vector3(0f,1.3f);
        lastmoveX = playerController.lastMoveX;
        lastmoveY = playerController.lastMoveY;

        hitbox.offset = new Vector2(lastmoveX / 1.5f, lastmoveY / 1.2f);

        if(hitbox.enabled == false)
        {
            hitbox.offset = new Vector2(0f, 0f);
            hitbox.size = new Vector2(0f, 0f);
        }


        if (lastmoveX == 0) //Attacking Up & Down
        {
            hitbox.size = new Vector2(4f, 3f);
        }
        else if (lastmoveY == 0) //Attacking Left & Right
        {
            hitbox.size = new Vector2(3f, 4f);
        }
        else if (lastmoveX != 0 && lastmoveY != 0) //Attacking Diagonally
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
        yield return new WaitForSeconds(0.15f);
        hitbox.enabled = false;
    }

    void OnTriggerEnter2D(Collider2D other){
        if (other.CompareTag("Enemy")){
            contact = true;
            currentObject = other.gameObject;
        }
    }
    void OnTriggerExit2D(Collider2D other){
        if (other.CompareTag("Enemy")){
            contact = false;
            currentObject = null;
        }
    }
}

