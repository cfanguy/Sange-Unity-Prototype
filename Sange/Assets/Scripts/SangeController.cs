using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SangeController : MonoBehaviour
{
    public float speed = 3.0f;

    Rigidbody2D rigidbody2d;
    float horizontal;
    float vertical;

    public GameObject projectilePrefab;

    Animator animator;
    Vector2 lookDirection = new Vector2(0, -1);

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        rigidbody2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        Vector2 move = new Vector2(horizontal, vertical);

        if (!Mathf.Approximately(move.x, 0.0f) ||
            !Mathf.Approximately(move.y, 0.0f))
        {
            lookDirection.Set(move.x, move.y);
            lookDirection.Normalize();
        }

        animator.SetFloat("Look X", lookDirection.x);
        animator.SetFloat("Look Y", lookDirection.y);
        animator.SetFloat("Speed", move.magnitude);

        if (Input.GetKeyDown(KeyCode.F))
        {
            Launch();
        }
    }

    void FixedUpdate() {
        Vector2 position = rigidbody2d.position;
        position.x += speed * horizontal * Time.deltaTime;
        position.y += speed * vertical * Time.deltaTime;
        
        rigidbody2d.MovePosition(position);
    }

    void Launch()
    {
        // right: 1,0,  left: -1,0, up: 0,1, down: 0,-1

        // set projectile's start position based on the direction the
        //   character is facing based on the above numbers for each direction
        Vector2 projectileStartPosition;

        // if x = 0, then direction is up or down
        if(lookDirection.x == 0)
        {
            // sets object slightly above(0.5) or below(-0.5) character
            projectileStartPosition = 1.0f * lookDirection.y * Vector2.up;
        }
        // else means y = 0 , so direction is left or right
        else
        {
            // sets object slightly to the right(0.5) or left(-0.5) of the character
            projectileStartPosition = 0.6f * lookDirection.x * Vector2.right;
        }

        GameObject projectileObject = Instantiate(projectilePrefab,
            rigidbody2d.position + projectileStartPosition, Quaternion.identity);

        // Debug.Log(lookDirection.x + " " + lookDirection.y);


        Projectile projectile = projectileObject.GetComponent<Projectile>();
        projectile.Launch(lookDirection, 0);
    }
}
