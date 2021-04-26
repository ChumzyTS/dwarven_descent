using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Enemy : HealthManager
{
    public Vector2[] pathPoints;
    public float moveSpeed;
    public bool followPlayer = true;
    public bool flyingEnemy = true;
    public float waitAroundTime;
    public float damage;

    public GameObject player;

    public AudioClip takeOff;

    private bool followingPlayer;
    private bool waitingAround;
    private float currentWaitTime;
    private int currentPoint;
    private bool collisionSetup;

    private Collider2D enemyCollider;
    private HealthManager enemyHealth;
    private bool notMoving;

    private float CalculateAngle(Vector2 pointA, Vector2 pointB) {
        float angle = (180 / Mathf.PI) * Mathf.Atan2(pointB.y - pointA.y, pointB.x - pointA.x) + 180;

        if (angle >= 360) {
            angle -= 360;
        }
        if (angle < 0) {
            angle += 360;
        }

        return angle;
    }

    private Vector2 CalculateAngleVector(Vector2 pointA, Vector2 pointB) {
        float angle = CalculateAngle(pointA, pointB);

        Vector2 angleVector = new Vector2(0, 0);

        if (angle < 90) {
            float perc = (angle) / 90;
            angleVector = new Vector2(-(1-perc), -perc);
        }
        else if (angle < 180) {
            float perc = (angle - 90) / 90;
            angleVector = new Vector2(perc, -(1-perc));
        }
        else if (angle < 270) {
            float perc = (angle - 180) / 90;
            angleVector = new Vector2(1-perc, perc);
        }
        else {
            float perc = (angle - 270) / 90;
            angleVector = new Vector2(-perc, 1-perc);
        }

        return angleVector;
    }

    private void MoveTowardsPoint(Vector2 point) {
        if ((Vector2)transform.position != point) {
            if (animator != null) {
                animator.SetBool("Moving", true);
            }
            if (flyingEnemy) {
                if (notMoving && takeOff != null) {
                    AudioSource.PlayClipAtPoint(takeOff, transform.position, 5);
                }
                Vector2 angleVector = CalculateAngleVector(transform.position, point);

                transform.position = (Vector2)transform.position + (angleVector * moveSpeed * Time.deltaTime);
            }
            else {
                Vector2 angleVector = CalculateAngleVector(transform.position, point);

                Rigidbody2D rigidbody = gameObject.GetComponent<Rigidbody2D>();
                SpriteRenderer spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
                if (angleVector.x < 0) {
                    rigidbody.velocity = new Vector2(-moveSpeed, rigidbody.velocity.y);
                    spriteRenderer.flipX = true;
                }
                else if(angleVector.x > 0) {
                    rigidbody.velocity = new Vector2(moveSpeed, rigidbody.velocity.y);
                    spriteRenderer.flipX = false;
                }
            }
            notMoving = false;
        }
        else {
            notMoving = true;
            if (animator != null) {
                animator.SetBool("Moving", false);
            }
        }
    }

    private void Awake() { 
        enemyHealth = gameObject.GetComponent<HealthManager>();
        enemyCollider = gameObject.GetComponent<Collider2D>();

        if (player == null) {
            player = GameObject.FindGameObjectWithTag("Player");
        }

        if (pathPoints.Length <= 0) {
            pathPoints = new Vector2[1];
            pathPoints[0] = transform.position;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Follow Path
        if (!followingPlayer && !waitingAround) {
            int nextPoint = currentPoint + 1;
            if (nextPoint >= pathPoints.Length) {
                nextPoint = 0;
            }

            Vector2 currentPlace = pathPoints[currentPoint];
            Vector2 nextPlace = pathPoints[nextPoint];

            float distance = Mathf.Sqrt(Mathf.Pow(nextPlace.y - transform.position.y, 2) + Mathf.Pow(nextPlace.x - transform.position.x, 2));
            float maxDistance = Mathf.Sqrt(Mathf.Pow(nextPlace.y - currentPlace.y, 2) + Mathf.Pow(nextPlace.x - currentPlace.x, 2));

            float percentage = (maxDistance - distance) / maxDistance;

            MoveTowardsPoint(nextPlace);

            if (percentage >= 0.99) {
                transform.position = nextPlace;
                currentPoint = nextPoint;
            }
        }

        bool wasFollowing = followingPlayer;
        followingPlayer = false;
        if (followPlayer) {
            if (waitingAround) {
                currentWaitTime -= Time.deltaTime;
                if (currentWaitTime <= 0) {
                    waitingAround = false;
                }
            }
            // Check if can see player
            Vector2 playerAngleVector = CalculateAngleVector(transform.position, player.transform.position);
            RaycastHit2D hit = Physics2D.Raycast(transform.position, playerAngleVector, Mathf.Infinity, LayerMask.GetMask("Player", "Ground", "Explodable"));
            if (hit.collider != null) {
                if (hit.transform.gameObject == player) {
                    followingPlayer = true;
                }
            }

            if (!followingPlayer && wasFollowing) {
                waitingAround = true;
                currentWaitTime = waitAroundTime;
            }

            if (followingPlayer) {
                MoveTowardsPoint(player.transform.position);
            }
        }

        // Hurt Player
        HealthManager playerHealth = player.GetComponent<HealthManager>();
        if (enemyCollider.IsTouching(playerHealth.hitCollider) && !enemyHealth.stunned) {
            playerHealth.Hurt(damage);
        }

        // Stunned
        ManageHealth();
    }

    private void Start() {
        SetupHealth();
    }
}
