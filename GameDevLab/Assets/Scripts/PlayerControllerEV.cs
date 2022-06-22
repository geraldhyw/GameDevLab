using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerControllerEV : MonoBehaviour
{
    private float force;
    public IntVariable marioUpSpeed;
    public IntVariable marioMaxSpeed;
    public GameConstants gameConstants;
    // other components and internal state
    public bool isADKeyUp = true;
    public bool isSpacebarUp = true;
    public bool isDead = false;
    public bool onGroundState = true;
    private bool faceRightState = true;
    private bool countScoreState = false;

    private Rigidbody2D marioBody;
    private SpriteRenderer marioSprite;
    private Animator marioAnimator;
    private AudioSource marioAudio;
    
    // public Text scoreText;
    // public Text gameOverText;
    // public Button restartButton;
    // public GameObject panel;
    // private int score = 0;


    // public CustomCastEvent onCast;
    public CastEvent onCast;


    // Start is called before the first frame update
    void Start()
    {
        marioUpSpeed.SetValue(gameConstants.playerMaxJumpSpeed);
        marioMaxSpeed.SetValue(gameConstants.playerMaxSpeed);
        force = gameConstants.playerDefaultForce;

        isADKeyUp = true;
        isSpacebarUp = true;


        // GameManager.onPlayerDeath += PlayerDiesSequence;

        // Set to be 30 FPS
        Application.targetFrameRate = 30;
        marioBody = GetComponent<Rigidbody2D>();
        marioSprite = GetComponent<SpriteRenderer>();
        marioAnimator = GetComponent<Animator>();
        marioAudio = GetComponent<AudioSource>();

        // // GameOver UI
        // gameOverText.gameObject.SetActive(false);
        // restartButton.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        // new
        // isADKeyUp = true;
        // isSpacebarUp = true;

        // if (Input.GetKeyUp("a") || Input.GetKeyUp("d"))
        // {
        //     isADKeyUp = true;
        // }

        // if (Input.GetKeyUp("space"))
        // {
        //     isSpacebarUp = true;
        // }



        if (Input.GetKeyUp("a") && faceRightState == false) {
            isADKeyUp = true;
        }

        if (Input.GetKeyUp("d") && faceRightState == true) {
            isADKeyUp = true;
        }

        // update xSpeed - animator
        marioAnimator.SetFloat("xSpeed", Mathf.Abs(marioBody.velocity.x));

        // toggle state
        if (Input.GetKeyDown("a"))
        {
            isADKeyUp = false;
            if (faceRightState) {
                faceRightState = false;
                marioSprite.flipX = true;
                // update onSkid - animator
                if (Mathf.Abs(marioBody.velocity.x) > 1.0)
                    marioAnimator.SetTrigger("onSkid");
            }
        }
        if (Input.GetKeyDown("d"))
        {
            isADKeyUp = false;
            if (!faceRightState) {
                faceRightState = true;
                marioSprite.flipX = false;
                // update onSkid - animator
                if (Mathf.Abs(marioBody.velocity.x) > 1.0)
                    marioAnimator.SetTrigger("onSkid");
            }
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
            // CentralManager.centralManagerInstance.consumePowerup(KeyCode.Z, this.gameObject);

            onCast.Raise(KeyCode.Z);
        }

        if (Input.GetKeyDown("x"))
        {
            // CentralManager.centralManagerInstance.consumePowerup(KeyCode.X, this.gameObject);

            onCast.Raise(KeyCode.X);
        }

        if (Input.GetKeyDown("space")) {
            isSpacebarUp = false;
        }

        // if (Input.GetKeyUp("space")) {
        //     isSpacebarUp = true;
        // }
    }

    void FixedUpdate()
    {
        if (!isDead)
        {
            //check if a or d is pressed currently
            if (!isADKeyUp)
            {
                // isADKeyUp = true;
                float direction = faceRightState ? 1.0f : -1.0f;
                Vector2 movement = new Vector2(force * direction, 0);
                if (marioBody.velocity.magnitude < marioMaxSpeed.Value)
                    marioBody.AddForce(movement);
            }

            if (!isSpacebarUp && onGroundState)
            {
                isSpacebarUp = true;
                marioBody.AddForce(Vector2.up * marioUpSpeed.Value, ForceMode2D.Impulse);
                onGroundState = false;
                // part 2
                marioAnimator.SetBool("onGround", onGroundState);
                countScoreState = true; //check if goomba is underneath
            }
        }

        // // dynamic rigidbody
        // float moveHorizontal = Input.GetAxis("Horizontal");
        // // if (Mathf.Abs(moveHorizontal) > 0)
        // // {
        // //     Vector2 movement = new Vector2(moveHorizontal, 0);
        // //     if (marioBody.velocity.magnitude < marioMaxSpeed.Value)
        // //         marioBody.AddForce(movement);
        // // }
        // if (Input.GetKeyDown("a") || Input.GetKeyDown("d"))
        // {
        //     float direction = faceRightState ? 1.0f : -1.0f;
        //     Vector2 movement = new Vector2(force * direction, 0);
        //     if (marioBody.velocity.magnitude < marioMaxSpeed.Value)
        //         marioBody.AddForce(movement);
        // }
        // if (Input.GetKeyDown("space") && onGroundState)
        // {
        //     marioBody.AddForce(Vector2.up * marioUpSpeed.Value, ForceMode2D.Impulse);
        //     onGroundState = false;
        //     marioAnimator.SetBool("onGround", onGroundState);
        //     countScoreState = true;
        // }
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

    public void PlayerDiesSequence()
    {
        isDead = true;
        // marioAnimator.SetBool("isDead", true);
        GetComponent<Collider2D>().enabled = false;
        marioBody.AddForce(Vector3.up * 30, ForceMode2D.Impulse);
        marioBody.gravityScale = 30;
        StartCoroutine(dead());
    }

    IEnumerator dead()
    {
        yield return new WaitForSeconds(1.0f);
        marioBody.bodyType = RigidbodyType2D.Static;
    }
}
