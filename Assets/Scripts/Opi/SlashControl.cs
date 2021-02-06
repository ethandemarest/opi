using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlashControl : MonoBehaviour
{
    Animator animator;
    PlayerController pc;

    private void Start()
    {
        pc = GameObject.Find("Opi").GetComponent<PlayerController>();
        animator = this.GetComponent<Animator>();
        StartCoroutine("SlashOne");
    }

    IEnumerator SlashOne()
    {
        animator.SetBool("slash1", true);
        animator.SetFloat("Last Move Horizontal", pc.lastMove.x*2);
        animator.SetFloat("Last Move Vertical", pc.lastMove.y*2);

        yield return new WaitForSeconds(0.8f);

        Destroy(this.gameObject);
    }

}
    