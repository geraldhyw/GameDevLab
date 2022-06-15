using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public float maxSpeed = 10;
    public float upSpeed = 25;
    private Rigidbody2D marioBody;
    private bool onGroundState = true;
    private SpriteRenderer marioSprite;
    private bool faceRightState = true;
    // public Transform enemyLocation;
    public Text scoreText;
    public Text gameOverText;
    public Button restartButton;
    public GameObject panel;
    // private int score = 0;
    private bool countScoreState = false;
    private Animator marioAnimator;
    private AudioSource marioAudio;
    public GameConstants gameConstants;

    // Start is called before the first frame update
    void Start()
    {
        GameManager.onPlayerDeath += PlayerDiesSequence;

        // Set to be 30 FPS
        Application.targetFrameRate = 30;
        marioBody = GetComponent<Rigidbody2D>();
        marioSprite = GetComponent<SpriteRenderer>();
        marioAnimator = GetComponent<Animator>();
        marioAudio = GetComponent<AudioSource>();

        // GameOver UI
        gameOverText.gameObject.SetActive(false);
        restartButton.gameObject.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        // update xSpeed - animator
        marioAnimator.SetFloat("xSpeed", Mathf.Abs(marioBody.velocity.x));

        // toggle state
        if (Input.GetKeyDown("a") && faceRightState)
        {
            faceRightState = false;
            marioSprite.flipX = true;
            // update onSkid - animator
            if (Mathf.Abs(marioBody.velocity.x) > 1.0)
                marioAnimator.SetTrigger("onSkid");
        }
        if (Input.GetKeyDown("d") && !faceRightState)
        {
            faceRightState = true;
            marioSprite.flipX = false;
            // update onSkid - animator
            if (Mathf.Abs(marioBody.velocity.x) > 1.0)
                marioAnimator.SetTrigger("onSkid");
        }

        // when jumping, and Gomba is near Mario and we haven't registered our score
        if (!onGroundState && countScoreState)
        {
            // if (Mathf.Abs(transform.position.x - enemyLocation.position.x) < 0.5f)
            // {
            //     countScoreState = false;
            //     score++;
            //     Debug.Log(score);
            // }
        }

        if (Input.GetKeyDown("z"))
        {
            CentralManager.centralManagerInstance.consumePowerup(KeyCode.Z, this.gameObject);
        }

        if (Input.GetKeyDown("x"))
        {
            CentralManager.centralManagerInstance.consumePowerup(KeyCode.X, this.gameObject);
        }
    }

    void FixedUpdate()
    {
        // dynamic rigidbody
        float moveHorizontal = Input.GetAxis("Horizontal");
        if (Mathf.Abs(moveHorizontal) > 0)
        {
            Vector2 movement = new Vector2(moveHorizontal, 0);
            if (marioBody.velocity.magnitude < maxSpeed)
                marioBody.AddForce(movement * speed);
        }
        if (Input.GetKeyUp("a") || Input.GetKeyUp("d"))
        {
            // stop
            // marioBody.velocity = Vector2.zero;
            marioBody.velocity = new Vector2(0, marioBody.velocity.y);
        }
        if (Input.GetKeyDown("space") && onGroundState)
        {
            marioBody.AddForce(Vector2.up * upSpeed, ForceMode2D.Impulse);
            onGroundState = false;
            marioAnimator.SetBool("onGround", onGroundState);
            countScoreState = true;
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Ground"))
        {
            onGroundState = true; // back on ground
            marioAnimator.SetBool("onGround", onGroundState);
            countScoreState = false; // reset score state
            // scoreText.text = "Score: " + score.ToString();
        };

        if (col.gameObject.CompareTag("Obstacles") && Mathf.Abs(marioBody.velocity.y) < 0.01f)
        {
            onGroundState = true; // back on ground
            marioAnimator.SetBool("onGround", onGroundState);
        }

        if (col.gameObject.CompareTag("Pipe"))
        {
            onGroundState = true; // back on ground
            marioAnimator.SetBool("onGround", onGroundState);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Collided with Gomba!");

            // GameOver UI
            // Time.timeScale = 0;
            // gameOverText.gameObject.SetActive(true);
            // restartButton.gameObject.SetActive(true);
            // panel.gameObject.SetActive(true);
        }
    }

    void PlayJumpSound()
    {
        marioAudio.PlayOneShot(marioAudio.clip);
    }

    void PlayerDiesSequence()
    {
        // Mario dies
        Debug.Log("Mario dies");
        // do whatever you want here, animate etc
        Vector2 direction = new Vector2(Random.Range(-1.0f, 1.0f), 1);
        marioBody.AddForce(direction.normalized * 10, ForceMode2D.Impulse);
        GetComponent<Collider2D>().enabled = false;
    }
}
