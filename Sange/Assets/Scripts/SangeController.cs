using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SangeController : MonoBehaviour
{
    public float speed = 3.0f;

    public int maxHealth = 5;
    public float timeInvincible = 2.0f;

    public int health { get { return currentHealth; } }
    public int currentHealth;

    bool isInvincible;
    float invincibleTimer;

    Rigidbody2D rigidbody2d;
    float horizontal;
    float vertical;

    public GameObject swordPrefab;
    public GameObject projectilePrefab;

    Animator animator;
    Vector2 lookDirection = new Vector2(0, -1);

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        rigidbody2d = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentHealth > 0)
        {
            horizontal = Input.GetAxis("Horizontal");
            vertical = Input.GetAxis("Vertical");
        }
        else
        {
            horizontal = 0f;
            vertical = 0f;
        }

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

        if (isInvincible)
        {
            invincibleTimer -= Time.deltaTime;
            if (invincibleTimer < 0)
            {
                isInvincible = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            SwingWeapon();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Launch();
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            RaycastHit2D hit = Physics2D.Raycast(rigidbody2d.position +
                Vector2.up * 0.2f, lookDirection, 1.5f, LayerMask.GetMask("NPC"));
            if (hit.collider != null)
            {
                NonPlayerCharacter character = hit.collider.GetComponent<NonPlayerCharacter>();
                if (character != null)
                {
                    character.DisplayDialog();
                }
            }
        }
    }

    void FixedUpdate() {
        Vector2 position = rigidbody2d.position;
        position.x += speed * horizontal * Time.deltaTime;
        position.y += speed * vertical * Time.deltaTime;
        
        rigidbody2d.MovePosition(position);
    }

    void SwingWeapon()
    {
        Vector2 projectileStartPosition = SetDirection(lookDirection,
            out projectileStartPosition);

        GameObject projectileObject = Instantiate(swordPrefab,
            rigidbody2d.position + projectileStartPosition, Quaternion.identity);

        Projectile projectile = projectileObject.GetComponent<Projectile>();
        projectile.SwingWeapon(lookDirection, 0);
    }

    void Launch()
    {
        Vector2 projectileStartPosition = SetDirection(lookDirection,
            out projectileStartPosition);

        GameObject projectileObject = Instantiate(projectilePrefab,
            rigidbody2d.position + projectileStartPosition, Quaternion.identity);

        Projectile projectile = projectileObject.GetComponent<Projectile>();
        projectile.Launch(lookDirection, 300);
    }

    Vector2 SetDirection(Vector2 direction, out Vector2 projectileStartPosition)
    {
        // right: 1,0,  left: -1,0, up: 0,1, down: 0,-1
        // set projectile's start position based on the direction the
        //   character is facing based on the above numbers for each direction

        // if x = 0, then direction is up or down
        if (lookDirection.x == 0)
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

        return projectileStartPosition;
    }

    public void ChangeHealth(int amount)
    {
        if (amount < 0)
        {
            if (isInvincible)
            {
                return;
            }

            isInvincible = true;
            invincibleTimer = timeInvincible;
        }

        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        UIHealthBar.instance.SetValue(currentHealth / (float)maxHealth);

        if (currentHealth == 0)
        {
            UIHealthBar.instance.GameOver();
        }
    }
}
