using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloud : MonoBehaviour
{
    public float speed = 1f;
    Vector2 offset;
    
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        offset = new Vector3(+1.0f, 0.0f, 0.0f);
        //transform.position = Vector3.MoveTowards(transform.position, offset, speed * Time.deltaTime);
        
    }
}
