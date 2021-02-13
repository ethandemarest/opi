using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Freezer : MonoBehaviour
{

    public float duration = 1f;

    bool isFrozen = false;
    float pendingFreezeDuration = 0f;


    void Update()
    {
        if(pendingFreezeDuration > 0 && !isFrozen)
        {
            StartCoroutine(DoFreeze());
        }
    }


    public void Freeze()
    {
        pendingFreezeDuration = duration;
    }


    IEnumerator DoFreeze()
    {
        isFrozen = true;
        var original = Time.timeScale;
        Time.timeScale = 0f;

        yield return new WaitForSecondsRealtime(duration);

        Time.timeScale = original;
        pendingFreezeDuration = 0f;
        isFrozen = false;

    }
}
