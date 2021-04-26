using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explode : HitEffect
{
    public GameObject smokeEmitter;
    public GameObject explosionEmiiter;
    public AudioClip explodeSound;

    override public void OnThrowHit(GameObject other, float strength) {
        if (other.layer == LayerMask.NameToLayer("Explodable")) {
            ExplodeObject(other.gameObject);
        }
        else if (other.layer == LayerMask.NameToLayer("Enemy")) {
            AttemptHurt(other);
            ExplodeObject();
        }
        if (other.tag != "Player") {
            ResetThrow();
        }
    }

    private void ExplodeObject(GameObject destroyObject = null) {
        GameObject smoke = Instantiate(smokeEmitter);
        smoke.transform.position = transform.position;
        smoke.transform.parent = transform.parent;
        smoke.GetComponent<ParticleSystem>().Emit(30);

        GameObject explosion = Instantiate(explosionEmiiter);
        explosion.transform.position = transform.position;
        explosion.transform.parent = transform.parent;
        explosion.GetComponent<ParticleSystem>().Emit(15);

        if (explodeSound != null) {
            AudioSource.PlayClipAtPoint(explodeSound, transform.position, 3);
        }

        if (destroyObject != null) {
            Destroy(destroyObject);
        }
        Destroy(gameObject);
    }
}
