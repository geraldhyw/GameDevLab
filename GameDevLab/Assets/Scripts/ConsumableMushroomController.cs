using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsumableMushroomController : MonoBehaviour
{
    private Rigidbody2D mushroomBody;
    private Vector2 velocity;
    private int moveRight = 1;
    public GameObject mushroomObject;
    private bool collected = false;

    // Start is called before the first frame update
    void Start()
    {
        mushroomBody = GetComponent<Rigidbody2D>();
        ComputeVelocity();
        mushroomBody.AddForce(new Vector2(0, mushroomBody.mass * 20), ForceMode2D.Impulse);
    }

    // Update is called once per frame
    void Update()
    {
        if (!collected)
        {
            moveMushroom();
        }
    }

    void ComputeVelocity()
    {
        velocity = new Vector2((moveRight) * 5, 0);
    }

    void moveMushroom()
    {
        mushroomBody.MovePosition(mushroomBody.position + velocity * Time.fixedDeltaTime);
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Pipe"))
        {
            moveRight *= -1;
            ComputeVelocity();
        };

        if (col.gameObject.CompareTag("Player"))
        {
            collected = true;
            moveRight = 0;
            ComputeVelocity();
        };
    }

    void onBecameInvisible()
    {
        Destroy(mushroomObject);
    }
}
