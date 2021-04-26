using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public float score;
    public float timePlayed;
    public bool gameRunning = true;

    public float treasuresCollected = 0;
    public float totalTreasures = 0;

    public Text scoreText;
    public Text finalScoreText;
    public Text treasureText;
    public Text winScoreText;
    public Text winTreasureText;
    public Text winTimeText;

    public Animator animator;

    // Update is called once per frame
    void Update()
    {
        if (gameRunning) {
            timePlayed += Time.deltaTime;
            
            string scoreStr = score.ToString();

            float zeros = 8 - scoreStr.Length;
            if (zeros >= 1) {
                for (int i = 0; i < zeros; i++) {
                    scoreStr = "0" + scoreStr;
                }
            }
            scoreText.text = scoreStr;
            finalScoreText.text = scoreStr;

            treasureText.text = treasuresCollected + "/" + totalTreasures + " Treasures";
        }
    }

    public void AddScore(float scoreToAdd) {
        score += scoreToAdd;
    }

    public void AddTreasure() {
        if (treasuresCollected < totalTreasures) {
            treasuresCollected++;
        }
    }

    public void EndGame() {
        gameRunning = false;

        timePlayed = Mathf.Round(timePlayed);
        float winMinutes = Mathf.Floor(timePlayed/60);
        float winSeconds = Mathf.Floor(timePlayed - (winMinutes * 60));

        float timeBonus = Mathf.Round(16000 - ((timePlayed / 60) * 1000));
        if (timeBonus < 0) {
            timeBonus = 0;
        }
        if (timeBonus > 15000) {
            timeBonus = 15000;
        }

        score += timeBonus;

        string scoreStr = score.ToString();

        float zeros = 8 - scoreStr.Length;
        if (zeros >= 1) {
            for (int i = 0; i < zeros; i++) {
                scoreStr = "0" + scoreStr;
            }
        }

        winScoreText.text = scoreStr;
        winTreasureText.text = treasuresCollected + "/" + totalTreasures + " Treasures";

        string minuteString = winMinutes.ToString();
        zeros = 1 - minuteString.Length;
        if (zeros >= 1) {
            for (int i = 0; i < zeros; i++) {
                minuteString = "0" + minuteString;
            }
        }

        string secondsString = winSeconds.ToString();
        zeros = 2 - secondsString.Length;
        if (zeros >= 1) {
            for (int i = 0; i < zeros; i++) {
                secondsString = "0" + secondsString;
            }
        }

        winTimeText.text = "Time " + minuteString + ":" + secondsString + " (+" + timeBonus + " Score)";

        animator.SetTrigger("Win");
    }
}
