using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestoryAfterTime : MonoBehaviour
{
    public float destroyTime;

    private float timePassed = 0;
    private void Update() {
        if (timePassed >= destroyTime) {
            Destroy(gameObject);
        }
        timePassed += Time.deltaTime;
    }
}
