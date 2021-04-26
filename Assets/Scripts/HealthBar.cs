using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public float maxHealth;
    public float health;

    public GameObject bar;


    // Update is called once per frame
    void Update()
    {

        float healthPerc = health / maxHealth;

        

        bar.transform.localScale = new Vector3(healthPerc, 1, 1);
        RectTransform barRectTransform = bar.GetComponent<RectTransform>();

        if (barRectTransform != null) {
            barRectTransform.anchoredPosition = new Vector2(-75f + (healthPerc / 2 * 150), 0);
        }
        else {
            bar.transform.localPosition = new Vector3(-0.5f + (healthPerc / 2), 0, 0);
            if (healthPerc == 1) {
                gameObject.GetComponent<SpriteRenderer>().enabled = false;
                bar.GetComponent<SpriteRenderer>().enabled = false;
            }
            else {
                gameObject.GetComponent<SpriteRenderer>().enabled = true;
                bar.GetComponent<SpriteRenderer>().enabled = true;
            }
            if (health <= 0) {
                gameObject.GetComponent<SpriteRenderer>().enabled = false;
                bar.GetComponent<SpriteRenderer>().enabled = false;
            }
        }
    }
}
