using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heal : HitEffect
{
    public float healAmount;
    public GameObject heartEmitter;

    override public void OnThrowHit(GameObject other, float strength) {
        if (strength > 0) {
            GameObject hearts = Instantiate(heartEmitter);
            hearts.transform.position = transform.position;
            hearts.transform.parent = transform.parent;
            hearts.GetComponent<ParticleSystem>().Emit(5);
            
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            player.GetComponent<HealthManager>().Hurt(-healAmount);

            Destroy(gameObject);
        }
    }
}
