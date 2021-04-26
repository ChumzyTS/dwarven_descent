using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class HealthManager : MonoBehaviour
{
    public float maxHealth = 3;
    public float health;
    public float deathTime = 1;
    public float invincibleTime = 0;
    public float stunTime = 0;

    public Throwable dropThrowable;

    public Collider2D hitCollider;
    public HealthBar healthBar;

    public Animator animator;
    private float currentInvincibleTime;
    private bool invincible = false;

    private float currentStunTime;
    public bool stunned = false;

    private bool setToDestroy;
    private float currentDeathTime;
    public bool died;

    public AudioSource audioSource;
    public AudioClip hurtSound;

    void Start()
    {
        SetupHealth();
    }

    private void Update() {
        ManageHealth();
    }   

    public void SetupHealth() {
        health = maxHealth;


        healthBar.maxHealth = maxHealth;
        healthBar.health = health;
    }

    public void ManageHealth() {
        if (invincible) {
            currentInvincibleTime -= Time.deltaTime;
            if (currentInvincibleTime <= 0) {
                invincible = false;
            }
        }

        if (stunned) {
            currentStunTime -= Time.deltaTime;
            if (currentStunTime <= 0) {
                stunned = false;
            }
        }

        if (setToDestroy) {
            currentDeathTime -= Time.deltaTime;
            if (currentDeathTime <= 0) {
                if (dropThrowable != null) {
                    GameObject newThrowable = Instantiate(dropThrowable.gameObject);
                    newThrowable.transform.parent = transform.parent;
                    newThrowable.transform.position = transform.position;
                }
                Destroy(gameObject);
            }
        }
    } 

    public bool Hurt(float amount) {
        if (!invincible) {
            AudioSource.PlayClipAtPoint(hurtSound, transform.position, 1);
            health -= amount;
            if (health > maxHealth) {
                health = maxHealth;
            }
            if (health < 0) {
                health = 0;
            }
            healthBar.health = health;

            if (health <= 0) {
                GameObject player = GameObject.FindGameObjectWithTag("Player");
                if (gameObject != player) {
                    player.GetComponent<ScoreManager>().AddScore(maxHealth * 100);
                }
                if (animator == null) {
                    Destroy(gameObject);
                }
                else {
                    if (!died) {
                        animator.SetTrigger("Died");
                        died = true;
                        stunned = true;
                        currentStunTime = 100000f;
                        if (gameObject.tag == "Player") {
                            if (gameObject.GetComponent<Throw>().holding != null) {
                                Destroy(gameObject.GetComponent<Throw>().holding.gameObject);
                            }
                        }
                        else {
                            setToDestroy = true;
                            currentDeathTime = deathTime;
                        }
                    }
                }
                return true;
            }
            else {
                if (animator != null) {
                    animator.SetTrigger("Hurt");
                }
                if (invincibleTime > 0) {
                    invincible = true;
                    currentInvincibleTime = invincibleTime;
                }
                if (stunTime > 0) {
                    stunned = true;
                    currentStunTime = stunTime;
                }
                return false;
            }
        }
        else {
            return false;
        }
    }
}
