using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Win : Treasure
{

    private GameObject player;
    private ScoreManager scoreManager;

    private void Awake() {
        player = GameObject.FindGameObjectWithTag("Player");
        scoreManager = player.GetComponent<ScoreManager>();

        scoreManager.totalTreasures += 1;
    }

    // Update is called once per frame
    void Update()
    {
        Collider2D collider = gameObject.GetComponent<Collider2D>();
        if (collider.IsTouchingLayers(LayerMask.GetMask("Player"))) {
            AddScore();
        }
    }

    private void AddScore() {
        scoreManager.AddScore(score);
        scoreManager.AddTreasure();

        if (collectSound != null) {
            AudioSource.PlayClipAtPoint(collectSound, Camera.main.transform.position, 1);
        }

        if (coinEmmiter != null) {
            GameObject coins = Instantiate(coinEmmiter);
            coins.transform.position = transform.position;
            coins.transform.parent = transform.parent;
            coins.GetComponent<ParticleSystem>().Emit(30);
        }

        player.GetComponent<ScoreManager>().EndGame();

        Destroy(gameObject);
    }

}
