using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Throwable : MonoBehaviour
{
    public float weight = 1;
    public HitEffect hitEffect;
    public AudioClip hitSound;

    [HideInInspector]
    public float thrownStrength = 0;
    [HideInInspector]
    public bool collisionSetup = false;
    [HideInInspector]
    public bool canBePickedUp = true;
    [HideInInspector]
    public bool beingHeld;
    [HideInInspector]
    public Collider2D collision;
    [HideInInspector]
    public Rigidbody2D rigidBody;

    private void Awake() {
        rigidBody = gameObject.GetComponent<Rigidbody2D>();
        collision = gameObject.GetComponent<Collider2D>();

        if (weight <= 0) {
            Debug.LogWarning(gameObject.transform.name + " has an invalid weight of " + weight);
            weight = 1;
        }

        if (hitEffect == null) {
            hitEffect = gameObject.AddComponent<HitEffect>();
        }
    }

    private void Update() {
        if (beingHeld) {
            rigidBody.simulated = false;
        }
        else {
            rigidBody.simulated = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (!beingHeld) {
            if (hitEffect != null) {
                hitEffect.OnThrowHit(other.gameObject, thrownStrength);
            }
            if (hitSound != null) {
            AudioSource.PlayClipAtPoint(hitSound, transform.position, thrownStrength * 2);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (!beingHeld) {
            if (hitEffect != null) {
                hitEffect.OnThrowHit(other.gameObject, thrownStrength);
            }
        }
    }
}
