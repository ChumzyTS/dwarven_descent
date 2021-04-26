using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGame : MonoBehaviour
{
    public GameObject player;
    public ScoreManager scoreManager;

    private float waitTilDestory;
    private bool destory;

    private void Awake() {
        scoreManager.gameRunning = false;
        player.GetComponent<Movement>().enabled = false;
        player.GetComponent<Throw>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !destory) {
            
            gameObject.GetComponent<Animator>().SetTrigger("Start");
            destory = true;
            waitTilDestory = 1;
        }

        if (destory) {
            waitTilDestory -= Time.deltaTime;

            if (waitTilDestory <= 0) {
                scoreManager.gameRunning = true;
                player.GetComponent<Movement>().enabled = true;
                player.GetComponent<Throw>().enabled = true;
                Destroy(gameObject);
            }
        }
    }
}
