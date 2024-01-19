using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraScript : MonoBehaviour
{
    [Tooltip("The GameObject that the camera will follow (presumably the player)")]
    [SerializeField]
    private GameObject gameObjectFollowing = null;

    [Tooltip("The distance that the camera will have with the GameObject when following it")]
    [SerializeField]
    private Vector3 distanceBetweenCameraAndObject = Vector3.zero;

    void LateUpdate()
    {
        //Get the rotation of the GameObject that the camera follows to know the direction its facing
        Quaternion rotation = Quaternion.Euler(0, gameObjectFollowing.transform.eulerAngles.y, 0);
        //With the direction gotten, get the camera behind the GameObject with the distance defined
        transform.position = gameObjectFollowing.transform.position + (rotation * distanceBetweenCameraAndObject);
        //Finally, look at the GameObject to make the camera rotate to look at it
        transform.LookAt(gameObjectFollowing.transform);
    }

}
