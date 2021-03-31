using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//https://github.com/ngmgit/platformer-2d/blob/master/Assets/Scripts/Player/PlayerMovement.cs
public class PlayerMovement : MonoBehaviour
{
    public int playerSpeed = 10;
    public float crouchSpeedMult = 0.5f;
    public int jumpForce = 1250;
    public float downRaySize = 0.8f;
    public float countAsFallingThreshold = 0.2f;

    //public Transform swordTransform;
    //public GameObject ledgeTrigger;
    //public GameObject maincollider;
    public Vector2 colliderCrouchSize, colliderSize;
    public Vector2 colliderCrouchOffset, colliderOffset;

    public Vector2 startPosition;

    NPC lastNpcContact;
    Rigidbody2D playerRb;
    SpriteRenderer playerSpriteRenderer;
    //public SpriteRenderer m_playerSpriteRenderer2;

    Animator animator;
    //GameManager gameManagerScript;
    InputController input;
    Collider2D lastUpColl;

    float moveX;
    Vector2 prevPosition;
    [SerializeField]
    int MAX_HEALTH = 5;
    float currentHealth;

    float cooldown;
    float hitCooldownTime = 0.7f;

    CapsuleCollider2D collider;
    private bool isCrouching;
    float stepCounter; 
    public float checkStepSound = 14;

    // Use this for initialization
    void Awake()
    {
        input = GetComponent<InputController>();
        playerRb = GetComponent<Rigidbody2D>();
        playerSpriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        collider = GetComponent<CapsuleCollider2D>();
        //gameManagerScript = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager> ();
        prevPosition = transform.position;
        currentHealth = MAX_HEALTH;

        input.isFalling = true;
        startPosition = transform.position;
    }

    void Update()
    {
        moveX = input.m_horizontal;
        CheckIfAirborne();
        CheckIfFalling();
        ResetIfDead();
        prevPosition = transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (cooldown > 0)
        {
            cooldown -= Time.deltaTime;
        }

        MovePlayer();
        PlayerRaycast();

        if (!CheckIfGrabCorner())
        {
            ModifyGravity();
        }
    }
    
    void PauseGame ()
    {
        Time.timeScale = 0;
    }

    void ResumeGame ()
    {
        Time.timeScale = 1;
    }

    void ModifyGravity()
    {
        if (input.isFalling)
        {
            playerRb.gravityScale = 2.5f;
        }

        if (input.isOnGround)
        {
            playerRb.gravityScale = 4f;
        }
    }

    public bool CheckIfGrabCorner()
    {
        return input.grabCorner;
        //|| m_animator.GetCurrentAnimatorStateInfo(0).IsName("CornerGrab");
    }

    void MovePlayer()
    {
        if (input.jumpPressed)
        {
            Jump();
        }

        //if (!CheckIfGrabCorner())
        {
            if (input.m_crouchPressed)
            {
                //if (input.isOnGround)
                {
                    //m_playerRb.velocity = Vector2.zero;
                    setCrouching(true);
                }
                //else
                {
                  //  setCrouching(false);
                }
            }
            else
            {
                setCrouching(false);
                /*bool attack1Active = m_animator.GetCurrentAnimatorStateInfo(0).IsName("Attack State.Attack1");
			    bool attack2Active = m_animator.GetCurrentAnimatorStateInfo(0).IsName("Attack State.Attack2");

			    if (!(attack1Active || attack2Active)) {
				    m_playerRb.velocity = new Vector2(m_moveX * playerSpeed, m_playerRb.velocity.y);
			    } else {
				    m_playerRb.velocity = Vector2.zero;
			    }*/

            }
            playerRb.velocity = new Vector2(
            moveX * (isCrouching ? crouchSpeedMult * playerSpeed : playerSpeed),
                playerRb.velocity.y);

            // Vector2 tempScale;
            // flip sprite based on direction facing
            if (moveX < 0.0f)
            {
                //tempScale = new Vector2(-1, 1);
                playerSpriteRenderer.flipX = true;
                //m_playerSpriteRenderer2.flipX = true;
                //swordTransform.localScale = tempScale;
                //ledgeTrigger.transform.localScale = tempScale;
                //maincollider.transform.localScale = tempScale;
            }
            else if (moveX > 0.0f)
            {
                //tempScale = new Vector2(1, 1);
                //swordTransform.localScale = tempScale;
                //ledgeTrigger.transform.localScale = tempScale;
                //maincollider.transform.localScale = tempScale;
                playerSpriteRenderer.flipX = false;
                //m_playerSpriteRenderer2.flipX = false;
            }

        }

        animator.SetFloat("moving", Mathf.Abs(moveX));
        animator.SetBool("isGrounded", input.isOnGround);
        animator.SetBool("isCrouching", isCrouching);
        animator.SetBool("isJumpPressed", input.jumpPressed);
        animator.SetBool("isFalling", input.isFalling);
        animator.SetBool("isInFlight", input.isInFlight);

        if (moveX != 0 && input.isOnGround)
        {
            stepCounter += Math.Abs(moveX);
            //Debug.Log("")
            //InputCanvas.instance.SetTextDebug("step "+stepCounter +"/"+ checkStepSound);
            if(stepCounter > checkStepSound)
            {
                stepCounter = 0;
                InputCanvas.instance.PlaySound(
                UnityEngine.Random.value > 0.5f
                    ? "Right_Step_Stone" : "Left_Step_Stone");
            }
        }
    }

    private void setCrouching(bool v)
    {
        isCrouching = v;
        collider.size = v ? colliderCrouchSize : colliderSize;
        collider.offset = v ? colliderCrouchOffset : colliderOffset;
    }

    void Jump()
    {
        //JoyInputController.m_jump = false;

        if (InputCanvas.instance.jumpFreely || input.isOnGround)
        {
            //transform.parent = null;
            input.jumpPressed = false;
            playerRb.velocity = Vector2.zero;
            playerRb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            SetGroundStatus(false);

            InputCanvas.instance.PlaySound("Jump1");
        }
    }

    void CheckIfFalling()
    {
        if (!input.isOnGround)
        {
            if (transform.position.y < prevPosition.y
                && Mathf.Abs(transform.position.y - prevPosition.y) > countAsFallingThreshold)
            {
                input.isFalling = true;
            }
        }
        else
        {
            input.isFalling = false;
        }
    }

    void CheckIfAirborne()
    {
        if (!input.isOnGround)
        {
            if (transform.position.y > prevPosition.y
                    && Mathf.Abs(transform.position.y - prevPosition.y) > countAsFallingThreshold)
            {
                input.isInFlight = true;
            }
        }
        else
        {
            input.isInFlight = false;
        }
    }
    void enemyTouched()
    {
        if (cooldown <= 0)
        {
            cooldown = hitCooldownTime;
            currentHealth--;
        }
    }

    void ResetIfDead()
    {
        //if (this.transform.position.y < -7)
        {
            //SceneManager.LoadScene("SampleScene");
        }
    }

    void PlayerRaycast()
    {
        //RaycastHit2D downRayLeft = Physics2D.Raycast(this.transform.position + new Vector3(-0.35f, 0), Vector2.down, downRaySize);
        //RaycastHit2D downRayRight = Physics2D.Raycast(this.transform.position + new Vector3(0.35f, 0), Vector2.down, downRaySize);
        int nonPlayer = ~(1 << 8);

        RaycastHit2D downRay = Physics2D.Raycast(this.transform.position,
             Vector2.down, downRaySize, nonPlayer);
        //Debug.DrawRay(transform.position, Vector2.down * downRaySize, Color.red, 1, false);
        SetGroundStatus(downRay.collider != null);

        if (downRay.collider != null)
        {
            if (downRay.collider.name.StartsWith("AreaChange"))
            {
             //Debug.LogWarning("downRay.collider.name=" + downRay.collider.name);
             InputCanvas.instance.SetArea(downRay.collider.name.Replace("AreaChange",""));
            }
            //Debug.Log("coll:" + downRay.collider.gameObject);
            var enemy = downRay.collider.gameObject.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemyTouched();
            }

            InputCanvas.instance.SetHealth(currentHealth, cooldown);

            var npc = downRay.collider.gameObject.GetComponent<NPC>();
            if (npc != null)
            {
                if(lastNpcContact != null && lastNpcContact != npc)
                {
                    lastNpcContact.mark.SetActive(false);
                    lastNpcContact = null;
                //    Debug.Log("mark false");
                }
                var mark = npc.mark;
                //void OnTriggerEnter(Collider col)
                {
                 // Debug.Log("mark true");
                   mark.SetActive(true);
                }

                //void OnTriggerExit(Collider col)
                {
                    //Debug.Log("mark false");
                  //  mark.SetActive(false);
                }
                lastNpcContact = npc;
            }
            else if(lastNpcContact != null)
            {
                lastNpcContact.mark.SetActive(false);
                lastNpcContact = null;
               // Debug.Log("mark false 2");
            }
        }

        RaycastHit2D upRay = Physics2D.Raycast(this.transform.position, Vector2.up, downRaySize, nonPlayer);

        if (upRay.collider != null && lastUpColl == null)
        {
            InputCanvas.instance.PlaySound("Bump_Head");
            lastUpColl = upRay.collider;
        }

        //if (false)
        // || downRayLeft.collider != null || downRayRight.collider != null
        //{
        //Debug.Log("Coll " + downRay.collider + "/" + downRay.collider.tag);
        //bool leftCollider = downRayLeft.collider != null && downRayLeft.collider.tag == "Ground&Obstacles";
        //bool rightCollider = downRayRight.collider != null && downRayRight.collider.tag == "Ground&Obstacles";
        //bool centerCollider = downRay.collider != null; // && downRay.collider.tag == "Ground&Obstacles";

        //if (centerCollider) // || rightCollider || leftCollider)
        //  {

        //}
        //}
        //else
        //{
        //SetGroundStatus(false);
        //}
    }

    void SetGroundStatus(bool m_status)
    {
        var old = input.isOnGround;
        input.isOnGround = m_status;
        if(!old && input.isOnGround)
        {
            InputCanvas.instance.PlaySound("Landing_Stone");
        }
    }

    void DamagePlayer()
    {
        currentHealth -= 15f;
        float healthRatio = currentHealth / MAX_HEALTH;
        input.isHurt = true;

        //gameManagerScript.SetPlayerHealth(healthRatio);

        if (currentHealth <= 0)
        {
            currentHealth = MAX_HEALTH;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "EnemyWeaponTrigger")
        {
            DamagePlayer();
        }
    }

    public void SetOnGrabStay()
    {
        /*if (!m_input.isOnGround && m_input.jumpGrabCornerPressed)
        {
            if (!m_animator.GetCurrentAnimatorStateInfo(0).IsName("CornerClimb"))
            {
                m_input.grabCorner = true;
                m_playerRb.gravityScale = 0;
                m_playerRb.velocity = Vector3.zero;
            }
        }*/
    }

    // Animation Event: On the first keyframe of CornerClimb
    public void ClimbWall()
    {
        playerRb.gravityScale = 4;
        playerRb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }
}