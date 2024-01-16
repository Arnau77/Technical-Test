using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{

    public GameObject gameObjectFollowing = null;

    public Vector3 distanceBetweenCameraAndObject = Vector3.zero;

    // Update is called once per frame
    void Update()
    {
        transform.position = gameObjectFollowing.transform.position + distanceBetweenCameraAndObject;
        transform.LookAt(gameObjectFollowing.transform);
    }
}
