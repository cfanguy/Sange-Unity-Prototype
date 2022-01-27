using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    Rigidbody2D rigidbody2d;
    SpriteRenderer spriteRenderer;
    float timer = 0.1f;

    // Start is called before the first frame update
    void Awake()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        Destroy(gameObject, timer);
    }

    public void Launch(Vector2 direction, float force)
    {
        // right: 1,0,  left: -1,0, up: 0,1, down: 0,-1

        // left no flips
        if (direction.y == 0)
        {
            // right flipX = true
            if (direction.x == 1)
            {
                spriteRenderer.flipX = true;
            }
        }
        else
        {
            // up flipX = true
            if (direction.y == 1)
            {
                spriteRenderer.flipX = true;
            }
            // down flipX = true, flipY = true
            else
            {
                spriteRenderer.flipX = true;
                spriteRenderer.flipY = true;
            }
        }


        rigidbody2d.AddForce(direction * force);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Debug.Log(collision.gameObject);
        //Destroy(gameObject);

        // if projectile encounters enemy, destroy enemy object
        EnemyController e = collision.collider.GetComponent<EnemyController>();
        if (e != null)
        {
            e.Hit();
        }
    }
}
