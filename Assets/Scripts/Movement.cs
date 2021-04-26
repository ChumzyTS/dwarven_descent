using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float movementSpeed;
    public float jumpStrength;

    public float jumpPrepTime;
    public float jumpCoyoteTime;
    public float prepTime;
    public float coyoteTime;

    public AudioClip jumpSound;

    public Collider2D groundCollider;
    public Animator animator;
    public ParticleSystem walkParticles;

    private Rigidbody2D rigidBody;

    void Awake() {
        rigidBody = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // Check If On Ground
        bool onGround = groundCollider.IsTouchingLayers(LayerMask.GetMask("Ground", "Explodable"));

        // Adjust Jump Times
        if (coyoteTime > 0) {
            coyoteTime -= Time.deltaTime;
        }
        if (prepTime > 0) {
            prepTime -= Time.deltaTime;
        }

        // Jump
        if (Input.GetKeyDown(KeyCode.Space)) {
            prepTime = jumpPrepTime;
        }

        if (onGround) {
            coyoteTime = jumpCoyoteTime;
        }
        float xMovement = Input.GetAxisRaw("Horizontal") * movementSpeed;

        float jump = 0;
        if ((Input.GetKeyDown(KeyCode.Space) || prepTime > 0) && (onGround || coyoteTime > 0)) {
            animator.SetBool("Jumped", true);

            if (jumpSound != null) {
                AudioSource.PlayClipAtPoint(jumpSound, Camera.main.transform.position, 10);
            }

            jump = jumpStrength;
            prepTime = 0;
            coyoteTime = 0;
            

            rigidBody.velocity = new Vector2(xMovement, jump);
        }

        rigidBody.velocity = new Vector2(xMovement, rigidBody.velocity.y);
        
        // Animation + Particles
        Throw throwScript = gameObject.GetComponent<Throw>();
        if (Input.GetAxisRaw("Horizontal") != 0) {
            if (Input.GetAxisRaw("Horizontal") < 0) {
                transform.GetComponent<SpriteRenderer>().flipX = true;
                if (throwScript.holding != null) {
                    throwScript.holding.GetComponent<SpriteRenderer>().flipX = true;
                }
            }
            else if (Input.GetAxisRaw("Horizontal") > 0) {
                transform.GetComponent<SpriteRenderer>().flipX = false;
                if (throwScript.holding != null) {
                    throwScript.holding.GetComponent<SpriteRenderer>().flipX = false;
                }
            }

            animator.SetBool("Walking", true);
        }
        else {
            animator.SetBool("Walking", false);
        }

        if (rigidBody.velocity.y <= 0) {
            animator.SetBool("Jumped", false);
        }

        if (onGround) {
            var main = walkParticles.main;
            main.simulationSpeed = 1;
        }
        else {
            var main = walkParticles.main;
            main.simulationSpeed = 0;
        }
        
    }
}
