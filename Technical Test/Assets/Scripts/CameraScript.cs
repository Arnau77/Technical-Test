using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraScript : MonoBehaviour
{
    public GameObject gameObjectFollowing = null;

    public Vector3 distanceBetweenCameraAndObject = Vector3.zero;

    // Update is called once per frame
    void LateUpdate()
    {
        Quaternion rotation = Quaternion.Euler(0, gameObjectFollowing.transform.eulerAngles.y, 0);
        transform.position = gameObjectFollowing.transform.position + (rotation * distanceBetweenCameraAndObject);
        transform.LookAt(gameObjectFollowing.transform);
    }

}
