using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float speed;
    public bool vertical;
    public float changeTime = 6.0f;

    new Rigidbody2D rigidbody2D;
    float timer;
    int direction = 1;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        timer = changeTime;
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;

        if(timer < 0)
        {
            direction = -direction;
            timer = changeTime;
        }

        Vector3 characterScale = transform.localScale;
        characterScale.x = direction;
        transform.localScale = characterScale;
    }

    void FixedUpdate()
    {
        Vector2 position = rigidbody2D.position;

        if(vertical)
        {
            position.y += Time.deltaTime * speed * direction;
        }
        else
        {
            position.x += Time.deltaTime * speed * direction;
        }

        rigidbody2D.MovePosition(position);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        SangeController player = other.gameObject.GetComponent<SangeController>();

        if (player != null)
        {
            player.ChangeHealth(-1);
        }
    }

    public void Hit()
    {
        Destroy(gameObject);
    }
}
