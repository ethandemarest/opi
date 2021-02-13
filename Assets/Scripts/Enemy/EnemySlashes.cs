using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySlashes : MonoBehaviour
{
    Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    void SpawnSlash(Vector2 slashDir)
    {
        animator.SetBool("slash1", true);
        animator.SetFloat("Last Move Horizontal", slashDir.x);
        animator.SetFloat("Last Move Vertical", slashDir.y);
    }
}
