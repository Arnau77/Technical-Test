using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatformScript : MonoBehaviour
{
    [Tooltip("Where the moving platform will be moving to when activated")]
    [SerializeField]
    private Transform finalTransform = null;

    [Tooltip("The initial position of the moving platform")]
    private Vector3 initialPosition = Vector3.zero;

    [Tooltip("The final position (or goal when activated) of the moving platform")]
    private Vector3 finalPosition = Vector3.zero;

    [Tooltip("The speed that the moving platform will be moving")]
    [SerializeField]
    private float speed = 0;

    [Tooltip("Whether the moving platform is activated or not")]
    private bool isActivated = false;

    [Tooltip("Whether the moving platform is returning to its inital position (after arriving to the final position) or not (it it's inactive, this bool is also false)")]
    private bool isReturning = false;

    // Start is called before the first frame update
    void Start()
    {
        initialPosition = transform.position;
        finalPosition = finalTransform.position;
        GameManager.instance.restartGame.AddListener(Restart);
    }

    /// <summary>
    /// This function is called when restarting the game
    /// </summary>
    /// <param name="checkPoint"> The checkpoint the game will restart from (if it's null, the game restarts from the beginning)</param>
    private void Restart(GameObject checkPoint)
    {
        //Return the moving platform to its initial position and deactivate it
        transform.position = initialPosition;
        isActivated = false;
        isReturning = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isActivated)
        {
            return;
        }

        //Update the platform's position by getting the normalize difference between the platform's position and its goal and multiplying it by the speed and delta time
        Vector3 newPosition = isReturning ? initialPosition : finalPosition;
        newPosition -= transform.position;
        newPosition.Normalize();
        newPosition *= speed * Time.deltaTime;
        transform.position += newPosition;

        //Check if the platform has arrived to its goals and if it has determine if it returns or if it deactivates
        if (CheckIfPlatformHasArrived(isReturning ? initialPosition : finalPosition))
        {
            if (isReturning)
            {
                isReturning = false;
                isActivated = false;
            }
            else
            {
                isReturning = true;
            }
        }
    }

    /// <summary>
    /// This function is called to activate the moving platform
    /// </summary>
    public void Activate()
    {
        if (isActivated)
        {
            return;
        }

        isActivated = true;
    }

    /// <summary>
    /// This function is called to check whether the moving platform has arrived to its goal or not
    /// </summary>
    /// <param name="goal"> The position of its goal (where the moving platform is moving to)</param>
    /// <returns> Whether the moving platform has arrived to its goal (true if it has arrived, false if not)</returns>
    private bool CheckIfPlatformHasArrived(Vector3 goal)
    {
        Vector3 difference = transform.position - goal;

        return Mathf.Abs(difference.x) < 0.1 && Mathf.Abs(difference.y) < 0.1 && Mathf.Abs(difference.z) < 0.1;
    }
}
