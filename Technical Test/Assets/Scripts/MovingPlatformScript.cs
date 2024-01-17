using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatformScript : TagsScript
{
    [SerializeField]
    private Transform finalTransform = null;

    private Vector3 initialPosition = Vector3.zero;

    private Vector3 finalPosition = Vector3.zero;

    [SerializeField]
    private float speed = 0;

    private bool isActivated = false;

    private bool isReturning = false;

    // Start is called before the first frame update
    void Start()
    {
        initialPosition = transform.position;
        finalPosition = finalTransform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isActivated)
        {
            return;
        }

        Vector3 newPosition = isReturning ? initialPosition : finalPosition;
        newPosition -= transform.position;
        newPosition.Normalize();
        newPosition *= speed;
        transform.position += newPosition;
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

    public void Activate()
    {
        if (isActivated)
        {
            return;
        }

        isActivated = true;
    }

    private bool CheckIfPlatformHasArrived(Vector3 goal)
    {
        Vector3 difference = transform.position - goal;

        return Mathf.Abs(difference.x) < 0.1 && Mathf.Abs(difference.y) < 0.1 && Mathf.Abs(difference.z) < 0.1;
    }
}
