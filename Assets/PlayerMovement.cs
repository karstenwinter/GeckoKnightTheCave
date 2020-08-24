using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//https://github.com/ngmgit/platformer-2d/blob/master/Assets/Scripts/Player/PlayerMovement.cs
public class PlayerMovement : MonoBehaviour
{
    public int playerSpeed = 10;
    public int jumpForce = 1250;
    public float downRaySize = 0.8f;
    //public Transform swordTransform;
    //public GameObject ledgeTrigger;
    //public GameObject maincollider;

    Rigidbody2D m_playerRb;
    SpriteRenderer m_playerSpriteRenderer;
    //public SpriteRenderer m_playerSpriteRenderer2;

    Animator m_animator;
    //GameManager gameManagerScript;
    InputController m_input;

    float m_moveX;
    Vector2 prevPosition;
    [SerializeField]
    int MAX_HEALTH = 100;
    float currentHealth;
    BoxCollider2D boxCollider;

    // Use this for initialization
    void Awake()
    {
        m_input = GetComponent<InputController>();
        m_playerRb = GetComponent<Rigidbody2D>();
        m_playerSpriteRenderer = GetComponent<SpriteRenderer>();
        m_animator = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
        //gameManagerScript = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager> ();
        prevPosition = transform.position;
        currentHealth = MAX_HEALTH;
    }

    void Update()
    {
        m_moveX = m_input.m_horizontal;
        CheckIfAirborne();
        CheckIfFalling();
        ResetIfDead();
        prevPosition = transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        MovePlayer();
        PlayerRaycast();

        if (!CheckIfGrabCorner())
        {
            ModifyGravity();
        }
    }

    void ModifyGravity()
    {
        if (m_input.isFalling)
        {
            m_playerRb.gravityScale = 2.5f;
        }

        if (m_input.isOnGround)
        {
            m_playerRb.gravityScale = 4f;
        }
    }

    public bool CheckIfGrabCorner()
    {
        return m_input.grabCorner;
        //|| m_animator.GetCurrentAnimatorStateInfo(0).IsName("CornerGrab");
    }

    void MovePlayer()
    {
        if (m_input.m_jumpPressed)
        {
            Jump();
        }

        //if (!CheckIfGrabCorner())
        {
            if (m_input.m_crouchPressed)
            {
                if (m_input.isOnGround)
                {
                    //m_playerRb.velocity = Vector2.zero;
                }
            }
            else
            {
                /*bool attack1Active = m_animator.GetCurrentAnimatorStateInfo(0).IsName("Attack State.Attack1");
			    bool attack2Active = m_animator.GetCurrentAnimatorStateInfo(0).IsName("Attack State.Attack2");

			    if (!(attack1Active || attack2Active)) {
				    m_playerRb.velocity = new Vector2(m_moveX * playerSpeed, m_playerRb.velocity.y);
			    } else {
				    m_playerRb.velocity = Vector2.zero;
			    }*/
                m_playerRb.velocity = new Vector2(m_moveX * playerSpeed, m_playerRb.velocity.y);

            }

            // Vector2 tempScale;
            // flip sprite based on direction facing
            if (m_moveX < 0.0f)
            {
                //tempScale = new Vector2(-1, 1);
                m_playerSpriteRenderer.flipX = true;
                //m_playerSpriteRenderer2.flipX = true;
                //swordTransform.localScale = tempScale;
                //ledgeTrigger.transform.localScale = tempScale;
                //maincollider.transform.localScale = tempScale;
            }
            else if (m_moveX > 0.0f)
            {
                //tempScale = new Vector2(1, 1);
                //swordTransform.localScale = tempScale;
                //ledgeTrigger.transform.localScale = tempScale;
                //maincollider.transform.localScale = tempScale;
                m_playerSpriteRenderer.flipX = false;
                //m_playerSpriteRenderer2.flipX = false;
            }

            m_animator.SetFloat("moving", Mathf.Abs(m_moveX));
        }
    }

    void Jump()
    {
        //m_input.m_jumpPressed = false;
        //JoyInputController.m_jump = false;

        //if (m_input.isOnGround)
        {
            //transform.parent = null;
            m_playerRb.velocity = Vector2.zero;
            m_playerRb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            //SetGroundStatus(false);
            m_animator.SetBool("isFalling", false);

        }


    }

    void CheckIfFalling()
    {
        if (!m_input.isOnGround)
        {
            if (transform.position.y < prevPosition.y)
            {
                m_input.isFalling = true;
            }
        }
        else
        {
            m_input.isFalling = false;
        }
    }

    void CheckIfAirborne()
    {
        if (!m_input.isOnGround)
        {
            if (transform.position.y > prevPosition.y)
            {
                m_input.isInFlight = true;
            }
        }
        else
        {
            m_input.isInFlight = false;
        }
    }

    void ResetIfDead()
    {
        if (this.transform.position.y < -7)
        {
            //SceneManager.LoadScene("SampleScene");
        }
    }

    void PlayerRaycast()
    {
        //RaycastHit2D downRayLeft = Physics2D.Raycast(this.transform.position + new Vector3(-0.35f, 0), Vector2.down, downRaySize);
        //RaycastHit2D downRayRight = Physics2D.Raycast(this.transform.position + new Vector3(0.35f, 0), Vector2.down, downRaySize);
        RaycastHit2D downRay = Physics2D.Raycast(this.transform.position, Vector2.down, downRaySize);
        Debug.DrawRay(this.transform.position, Vector2.down * downRaySize, Color.red);

        if (downRay.collider != null)
        // || downRayLeft.collider != null || downRayRight.collider != null
        {
            Debug.Log("Coll " + downRay.collider+ "/"+ downRay.collider.tag);
            //bool leftCollider = downRayLeft.collider != null && downRayLeft.collider.tag == "Ground&Obstacles";
            //bool rightCollider = downRayRight.collider != null && downRayRight.collider.tag == "Ground&Obstacles";
            //bool centerCollider = downRay.collider != null; // && downRay.collider.tag == "Ground&Obstacles";

            //if (centerCollider) // || rightCollider || leftCollider)
            {
                SetGroundStatus(true);
            }
        }
        else
        {
            SetGroundStatus(false);
        }
    }

    void SetGroundStatus(bool m_status)
    {
        m_input.isOnGround = m_status;
        m_animator.SetBool("isGrounded", m_status);
    }

    void DamagePlayer()
    {
        currentHealth -= 15f;
        float healthRatio = currentHealth / MAX_HEALTH;
        m_input.isHurt = true;

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
        m_playerRb.gravityScale = 4;
        m_playerRb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }
}