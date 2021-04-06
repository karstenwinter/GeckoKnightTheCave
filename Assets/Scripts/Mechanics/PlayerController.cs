using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Platformer.Gameplay;
using static Platformer.Core.Simulation;
using Platformer.Core;

namespace Platformer.Mechanics
{
    /// <summary>
    /// This is the main class used to implement control of the player.
    /// It is a superset of the AnimationController class, but is inlined to allow for any kind of customisation.
    /// </summary>
    public class PlayerController : KinematicObject
    {
        public ParticleSystem dustParticleSystem, hitParticleSystem, gotHitParticleSystem;
        public Vector3 hitPosition;
        public AudioClip jumpAudio;
        public AudioClip respawnAudio;
        public AudioClip ouchAudio;

        /// <summary>
        /// Max horizontal speed of the player.
        /// </summary>
        public float maxSpeed = 7;
        float currentHitCounter = 0;
        public float hitFreq = 1;
        /// <summary>
        /// Initial jump velocity at the start of a jump.
        /// </summary>
        public float jumpTakeOffSpeed = 7;

        public JumpState jumpState = JumpState.Grounded;
        private bool stopJump;
        /*internal new*/ public Collider2D collider2d;
        /*internal new*/ public AudioSource audioSource;
        public Health health;
        public bool controlEnabled = true;
        public bool IsHitting = false;
        bool jump;
        Vector2 move;
        SpriteRenderer spriteRenderer;
        internal Animator animator;
        readonly PlatformerModel model = Simulation.GetModel<PlatformerModel>();

        public Bounds Bounds => collider2d.bounds;
        
        public float downRaySize = 0.8f;

        float currentDamageCooldown;
        float damageCooldownTime = 0.7f;
        float freezeCooldownTime = 0.16f;
        NPC lastNpcContact;
        Collider2D lastUpColl;

        int _shells;
        public int shells { get { return _shells; } set { _shells = value; UpdateUI(); } }

        public void UpdateUI() {
            InputCanvas.instance.UpdateValues((int) health.currentHP, currentDamageCooldown, _shells);
        }

        void Awake()
        {
            health = GetComponent<Health>();
            audioSource = GetComponent<AudioSource>();
            collider2d = GetComponent<Collider2D>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            animator = GetComponent<Animator>();
        }

        void Start() {
            UpdateUI();
        }

        protected override void Update()
        {
            if (controlEnabled)
            {
                move.x = Input2.GetAxis("Horizontal");
                if (jumpState == JumpState.Grounded && Input2.GetButtonDown("Jump"))
                    jumpState = JumpState.PrepareToJump;
                else if (Input2.GetButtonUp("Jump"))
                {
                    //Debug.Log("Jump");
                    stopJump = true;
                    Schedule<PlayerStopJump>().player = this;
                }
                //if (Input2.GetButtonDown("Fire1"))
                {
                  //  Debug.Log("Fire1");
                    //Schedule<PlayerHits>().player = this;
                }
            }
            else
            {
                move.x = 0;
                velocity.x = 0;
            }
            UpdateJumpState();
            PlayerRaycast();
            if(controlEnabled) {
                if(Input2.GetButtonDown("Fire1")) {
                    animator.SetBool("hit", true);
                    CreateHit();
                    currentHitCounter = hitFreq;
                } else {
                    animator.SetBool("hit", false);
                }
                if(currentHitCounter > 0) {
                    currentHitCounter -= Time.deltaTime;
                    Debug.Log(currentHitCounter + " / " + hitFreq);
                }
            }
            base.Update();
        }

        void enemyTouched()
        {
            if (currentDamageCooldown <= 0)
            {
                currentDamageCooldown = damageCooldownTime;
                health.Decrement();
                UpdateUI();
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
            int nonPlayerNonCameraBounds = ~((1 << 8) | (1 << 12));

            RaycastHit2D downRay = Physics2D.Raycast(this.transform.position,
                Vector2.down, downRaySize, nonPlayerNonCameraBounds);

            //Debug.DrawLine(transform.position, new Vector3(0, downRaySize, 0), Color.white, 5f, false);
            if(currentDamageCooldown > 0 && controlEnabled) {
                currentDamageCooldown -= Time.unscaledDeltaTime;
                UpdateUI();
                //if(currentDamageCooldown < freezeCooldownTime) {
                InputCanvas.instance.timeScale = 0.1f;
                //} else {
                    //InputCanvas.instance.timeScale = 1f;
                //}
                PlayGotHit();
                Bounce(new Vector2((move.x == 0 ? 1 : -Mathf.Sign(move.x)) * 8, 4));
            } else {
                InputCanvas.instance.timeScale = 1f;
            }
            if (downRay.collider != null && controlEnabled)
            {
                if (downRay.collider.name.StartsWith("AreaChange"))
                {
                    // Debug.LogWarning("downRay.collider.name=" + downRay.collider.name);
                    InputCanvas.instance.SetArea(downRay.collider.name.Replace("AreaChange",""));
                }
                // Debug.Log("coll:" + downRay.collider.gameObject);
                var enemy = downRay.collider.gameObject.GetComponent<Enemy>();
                if (enemy != null) {
                    if(currentHitCounter > 0) {
                        enemy.KillEnemy();
                    } else {
                        enemyTouched();
                    }
                }

                var npc = downRay.collider.gameObject.GetComponent<NPC>();
                if (npc != null)
                {
                    if(lastNpcContact != null && lastNpcContact != npc)
                    {
                        lastNpcContact.mark.active = false;
                        lastNpcContact = null;
                    //    Debug.Log("mark false");
                    }
                    var mark = npc.mark;
                    //void OnTriggerEnter(Collider col)
                    //{
                    // Debug.Log("mark true");
                    mark.active = true;
                    //}

                    //void OnTriggerExit(Collider col)
                    //{/
                        //Debug.Log("mark false");
                    //  mark.SetActive(false);
                    //}
                    lastNpcContact = npc;
                }
                else if(lastNpcContact != null)
                {
                    lastNpcContact.mark.SetActive(false);
                    lastNpcContact = null;
                    // Debug.Log("mark false 2");
                }
            }

            RaycastHit2D upRay = Physics2D.Raycast(this.transform.position, Vector2.up, downRaySize, nonPlayerNonCameraBounds);

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
        
        void UpdateJumpState()
        {
            jump = false;
            switch (jumpState)
            {
                case JumpState.PrepareToJump:
                    jumpState = JumpState.Jumping;
                    jump = true;
                    stopJump = false;
                    break;
                case JumpState.Jumping:
                    if (!IsGrounded)
                    {
                        Schedule<PlayerJumped>().player = this;
                        jumpState = JumpState.InFlight;
                    }
                    break;
                case JumpState.InFlight:
                    if (IsGrounded)
                    {
                        Schedule<PlayerLanded>().player = this;
                        jumpState = JumpState.Landed;
                    }
                    break;
                case JumpState.Landed:
                    jumpState = JumpState.Grounded;
                    break;
            }
        }

        protected override void ComputeVelocity()
        {
            if (jump && IsGrounded)
            {
                velocity.y = jumpTakeOffSpeed * model.jumpModifier;
                jump = false;
            }
            else if (stopJump)
            {
                stopJump = false;
                if (velocity.y > 0)
                {
                    velocity.y = velocity.y * model.jumpDeceleration;
                }
            }

            if (move.x > 0.01f)
                spriteRenderer.flipX = false;
            else if (move.x < -0.01f)
                spriteRenderer.flipX = true;

            animator.SetBool("grounded", IsGrounded);
            animator.SetFloat("velocityX", Mathf.Abs(velocity.x) / maxSpeed);

            targetVelocity = move * maxSpeed;

            if((velocity.y == 0 && Mathf.Abs(velocity.x) > 2f) || velocity.y > 4f)
                CreateDust();
        }

        void CreateDust() {
            dustParticleSystem.Play();
        }
        void PlayGotHit() {
            gotHitParticleSystem.Play();
        }
        void CreateHit() {
            hitParticleSystem.Play();
            hitParticleSystem.gameObject.transform.rotation = Quaternion.Euler(0, spriteRenderer.flipX ? -180 : 0, 0);
            hitParticleSystem.gameObject.transform.position = transform.position + (spriteRenderer.flipX ? -1 : 1) * hitPosition;
            hitParticleSystem.gameObject.GetComponent<ParticleSystemRenderer>().flip = new Vector3(spriteRenderer.flipX ? 1 : 0, 0, 0);
        }
        

        public enum JumpState
        {
            Grounded,
            PrepareToJump,
            Jumping,
            InFlight,
            Landed
        }
    }
}