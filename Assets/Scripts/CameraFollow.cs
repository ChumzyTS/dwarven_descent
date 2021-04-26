using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Camera followCamera;

    // Update is called once per frame
    void Update()
    {
        followCamera.transform.position = new Vector3(followCamera.transform.position.x, transform.position.y, followCamera.transform.position.z);
    }
}
