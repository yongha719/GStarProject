using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class taweae : MonoBehaviour
{
    void Start()
    {
        GetComponent<Rigidbody2D>().velocity = new Vector2(1, 4);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
