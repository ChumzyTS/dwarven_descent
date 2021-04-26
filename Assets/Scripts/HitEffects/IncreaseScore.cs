using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncreaseScore : HitEffect
{
    public float score;
    public GameObject coinEmmiter;
    public AudioClip coinSound;

    override public void OnThrowHit(GameObject other, float strength) {
        if (other.layer == LayerMask.NameToLayer("Enemy")) {
            AttemptHurt(other);
            AddScore();
        }
        if (other.tag != "Player") {
            ResetThrow();
        }
    }

    private void AddScore() {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        ScoreManager scoreManager = player.GetComponent<ScoreManager>();

        if (coinSound != null ) {
            AudioSource.PlayClipAtPoint(coinSound, transform.position, 1);
        }

        if (coinEmmiter != null) {
            GameObject coins = Instantiate(coinEmmiter);
            coins.transform.position = transform.position;
            coins.transform.parent = transform.parent;
            coins.GetComponent<ParticleSystem>().Emit(5);
        }

        scoreManager.AddScore(score);
        Destroy(gameObject);
    }
}
