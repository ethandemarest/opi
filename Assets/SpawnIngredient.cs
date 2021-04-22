using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnIngredient : MonoBehaviour
{
    public GameObject spawner;
    public GameObject ingredient;
    public GameObject prompt;
    public Vector3 offset;
    public bool picked;

    Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    IEnumerator SpawnItem()
    {
        if (picked)
        {
            yield break;
        }

        prompt.SendMessage("ArrowOff");
        animator.SetBool("Pick", true);

        yield return new WaitForSeconds(1.2f);
        Instantiate(ingredient, spawner.transform.position + offset, Quaternion.Euler(0f, 0f, 0f));
        picked = true;

    }
}
    