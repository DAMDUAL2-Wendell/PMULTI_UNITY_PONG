using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaddleScript : MonoBehaviour
{
    public float speed = 15f;
    public string axis = "Vertical";
    private void FixedUpdate()
    {
        float v = Input.GetAxisRaw(axis);
        gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0,v) * speed;
    }
}
