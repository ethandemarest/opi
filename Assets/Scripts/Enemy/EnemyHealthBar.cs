using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthBar : MonoBehaviour
{
    Animator animator;
    SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = new Color(1, 1, 1, 0);

    }

    void HealthSet()
    {
        animator.SetBool("Damage", true);
        StartCoroutine("SpriteVisible");
    }

    IEnumerator SpriteVisible()
    {
        spriteRenderer.color = new Color(1, 1, 1, 1);

        yield return new WaitForSeconds(3f);


        spriteRenderer.color = new Color(1, 1, 1, 0);

    }
}
