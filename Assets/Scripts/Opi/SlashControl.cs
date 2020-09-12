using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlashControl : MonoBehaviour
{
    Animator animator;
    GameObject opi;
    private float lastMoveX;
    private float lastMoveY;

    // Update is called once per frame
    private void Start()
    {
        opi = GameObject.Find("Opi");
        animator = this.GetComponent<Animator>();
        StartCoroutine("SlashOne");

    }


    IEnumerator SlashOne()
    {
        animator.SetBool("slash1", true);

        lastMoveX = opi.GetComponent<PlayerController>().lastMoveX;
        lastMoveY = opi.GetComponent<PlayerController>().lastMoveY;
        animator.SetFloat("Last Move Horizontal", lastMoveX + lastMoveX);
        animator.SetFloat("Last Move Vertical", lastMoveY + lastMoveY);

        yield return new WaitForSeconds(0.8f);

        Destroy(this.gameObject);
    }

}
    