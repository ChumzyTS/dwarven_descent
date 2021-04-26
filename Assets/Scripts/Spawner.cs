using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Throwable itemToSpawn;

    private Throwable spawnedItem;

    // Update is called once per frame
    void Update()
    {
        if (spawnedItem == null) {
            spawnedItem = Instantiate(itemToSpawn);
            spawnedItem.transform.position = transform.position;
        }
    }
}
