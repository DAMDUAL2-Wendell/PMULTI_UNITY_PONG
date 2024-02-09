using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ball : MonoBehaviour
{
    public float speed = 15f;

    public int bluescore;
    public int redscore;
    public Text redTextScore;
    public Text blueTextScore;
    void Start()
    {
        gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.right * speed;
    }

    float hitFactor(System.Numerics.Vector2 ballPos, System.Numerics.Vector2 racketPos, float racketHeight)
    {
        return (ballPos.Y - racketPos.Y) / racketHeight;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.name == "paddleBlue")
        {
            System.Numerics.Vector2 position1 = new System.Numerics.Vector2(transform.position.x, transform.position.y);
            System.Numerics.Vector2 position2 = new System.Numerics.Vector2(other.transform.position.x, other.transform.position.y);
            float y = hitFactor(position1, position2, other.collider.bounds.size.y);

            Vector2 dir = new Vector2(1,y).normalized;

            GetComponent<Rigidbody2D>().velocity = dir * speed;
        }
        
        if (other.gameObject.name == "paddleRed")
        {
            System.Numerics.Vector2 position1 = new System.Numerics.Vector2(transform.position.x, transform.position.y);
            System.Numerics.Vector2 position2 = new System.Numerics.Vector2(other.transform.position.x, other.transform.position.y);
            float y = hitFactor(position1, position2, other.collider.bounds.size.y);

            Vector2 dir = new Vector2(-1,y).normalized;

            GetComponent<Rigidbody2D>().velocity = dir * speed;
        }

        if (other.gameObject.name == "LeftWall")
        {
            redscore++;
            redTextScore.text = "" + redscore;
        }
        
        if (other.gameObject.name == "RightWall")
        {
            bluescore++;
            blueTextScore.text = "" + bluescore;
        }
        

    }
}
