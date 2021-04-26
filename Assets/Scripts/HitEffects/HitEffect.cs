using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitEffect : MonoBehaviour
{
    public float damage = 0;

    // Base Hit Effect
    public virtual void OnThrowHit(GameObject other, float strength) {
        if (other.layer == LayerMask.NameToLayer("Enemy")) {
            AttemptHurt(other, strength);
        }
        if (other.tag != "Player") {
            ResetThrow();
        }
    }

    public void ResetThrow() {
        Throwable throwable = gameObject.GetComponent<Throwable>();
        throwable.canBePickedUp = true;
    }

    public void AttemptHurt(GameObject other, float strength = 1) {
        if (other.layer == LayerMask.NameToLayer("Enemy")) {
            Enemy enemy = other.GetComponent<Enemy>();
            enemy.Hurt(damage * strength);
        }
    }
}
