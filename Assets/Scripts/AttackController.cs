using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackController : MonoBehaviour
{
    GameObject hitbox;
    Animator animator;
    public int noOfClicks = 0;
    float lastClickedTime = 0;
    public float maxComboDelay = 0.5f;
    public bool hasAttacked;
    public bool attacking;

    // Start is called before the first frame update
    void Start()
    {
        animator = this.GetComponent<Animator>();
        hitbox = GameObject.Find("Hitbox");
    }

    // Update is called once per frame
    void Update()
    {

        if (Time.time - lastClickedTime > maxComboDelay)
        {
            noOfClicks = 0;
            animator.SetBool("Attack 1", false);
            animator.SetBool("Attack 2", false);

        }

        if (Input.GetButtonDown("attack"))
        {
            StartCoroutine("Attacking");
            lastClickedTime = Time.time;
            noOfClicks++;

            if (noOfClicks % 2 == 1)
            {
                hitbox.SendMessage("Attack");
                SendMessage("attackOne");
            }

            if (noOfClicks % 2 == 0)
            {
                hitbox.SendMessage("Attack");
                SendMessage("attackTwo");
            }
        }

    }

    IEnumerator Attacking()
    {
        attacking = true;

        yield return new WaitForSeconds(0.8f);

        attacking = false;

    }

    public void attackOne()
    {
        animator.SetBool("Attack 1", true);
    }

    public void attackTwo()
    {
        animator.SetBool("Attack 2", true);
    }

    private void LateUpdate()
    {
        if (this.animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            hasAttacked = false;
        }
        animator.SetBool("Has Attacked", hasAttacked);


    }

}
