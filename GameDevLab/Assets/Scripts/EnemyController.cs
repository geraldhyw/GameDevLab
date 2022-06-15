using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{

    public GameConstants gameConstants;
    private float originalX;
    private int moveRight;
    private Vector2 velocity;
    private Rigidbody2D enemyBody;
    private SpriteRenderer enemySprite;

    // Start is called before the first frame update
    void Start()
    {
        // subscribe to player event
        GameManager.onPlayerDeath += EnemyRejoice;

        enemySprite = GetComponent<SpriteRenderer>();
        enemyBody = GetComponent<Rigidbody2D>();
        // get the starting position
        originalX = transform.position.x;
        moveRight = Random.Range(0, 2) == 0 ? -1 : 1;
        ComputeVelocity();
        gameConstants.isRejoice = false;
    }

    void ComputeVelocity()
    {
        velocity = new Vector2((moveRight) * gameConstants.maxOffset / gameConstants.enemyPatroltime, 0);
    }

    void MoveGomba()
    {
        enemyBody.MovePosition(enemyBody.position + velocity * Time.fixedDeltaTime);
    }

    // Update is called once per frame
    void Update()
    {
        if (Mathf.Abs(enemyBody.position.x - originalX) < gameConstants.maxOffset)
        {
            MoveGomba();
            if (gameConstants.isRejoice)
            {
                enemySprite.flipX = !enemySprite.flipX;
                velocity = new Vector2(0, 0);
                // Time.timeScale = 0.0f;
            }
        }
        else
        {
            // change direction
            moveRight *= -1;
            ComputeVelocity();
            MoveGomba();
            if (gameConstants.isRejoice)
            {
                enemySprite.flipX = !enemySprite.flipX;
                velocity = new Vector2(0, 0);
                // Time.timeScale = 0.0f;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // check if it collides with Mario
        if (other.gameObject.tag == "Player")
        {
            float yoffset = (other.transform.position.y - this.transform.position.y);
            if (yoffset > 0.75f)
            {
                KillSelf();
            }
            else
            {
                // hurt player, implement later
                CentralManager.centralManagerInstance.damagePlayer();
            }
        }
    }

    void KillSelf()
    {
        // enemy dies
        CentralManager.centralManagerInstance.increaseScore();
        StartCoroutine(flatten());

        Debug.Log("Kill sequence ends");
    }

    IEnumerator flatten()
    {
        Debug.Log("Flatten starts");
        int steps = 5;
        float stepper = 1.0f / (float)steps;

        for (int i = 0; i < steps; i++)
        {
            this.transform.localScale = new Vector3(this.transform.localScale.x, this.transform.localScale.y - stepper, this.transform.localScale.z);

            // make sure enemy is still above ground
            this.transform.position = new Vector3(this.transform.position.x, gameConstants.groundSurface + GetComponent<SpriteRenderer>().bounds.extents.y, this.transform.position.z);
            yield return null;
        }
        Debug.Log("Flatten ends");
        this.gameObject.SetActive(false);
        Debug.Log("Enemy returned to pool");
        yield break;
    }

    void EnemyRejoice()
    {
        Debug.Log("Enemy killed Mario");
        // do whatever you want here, animate etc
        gameConstants.isRejoice = true;

    }
}