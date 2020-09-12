using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackController : MonoBehaviour
{
    public GameObject opiSlashOne;
    public GameObject opiSlashTwo;

    GameObject hitbox;
    Animator animator;
    public int noOfClicks = 0;
    float lastClickedTime = 0;
    public float maxComboDelay = 0.5f;
    public bool hasAttacked;
    public bool attacking;
    bool rolling;

    // Start is called before the first frame update
    void Start()
    {
        animator = this.GetComponent<Animator>();
        hitbox = GameObject.Find("Hitbox");
    }

    // Update is called once per frame
    void Update()
    {
        rolling = this.GetComponent<PlayerController>().rolling;

        if (Time.time - lastClickedTime > maxComboDelay)
        {
            noOfClicks = 0;
            animator.SetBool("Attack 1", false);
            animator.SetBool("Attack 2", false);
        }
        if (Input.GetButtonDown("attack") && rolling == false)  
        {
            lastClickedTime = Time.time;
            noOfClicks++;
            if (noOfClicks % 2 == 1)
            {
                hitbox.SendMessage("Attack");
                StartCoroutine("attackOne");
            }
            if (noOfClicks % 2 == 0)
            {
                hitbox.SendMessage("Attack");
                StartCoroutine("attackTwo");
            }
        }
    }

    IEnumerator attackOne()
    {
        SendMessage("slashOne");
        attacking = true;
        animator.SetBool("Attack 1", true);

        yield return new WaitForSeconds(0.9f);

        attacking = false;
    }

    IEnumerator attackTwo()
    {
        SendMessage("slashTwo");
        attacking = true;
        animator.SetBool("Attack 2", true);

        yield return new WaitForSeconds(0.9f);

        attacking = false;
    }

    void slashOne()
    {
        Instantiate(opiSlashOne, this.transform.position + new Vector3(0f, 1.5f), Quaternion.Euler(transform.position.x, transform.position.y, 0f));
    }
    void slashTwo()
    {
        Instantiate(opiSlashTwo, this.transform.position + new Vector3(0f, 1.5f), Quaternion.Euler(transform.position.x, transform.position.y, 0f));
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
