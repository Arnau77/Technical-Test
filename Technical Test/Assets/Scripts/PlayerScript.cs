using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerScript : TagsScript
{
    public Rigidbody playerRigidbody = null;

    public Vector3 initialPosition = Vector3.zero;

    private Vector2 movement = Vector2.zero;

    public float movementSpeed = 0;

    public float jumpSpeed = 0;

    public float rotationAngle = 0;

    private int isRotating = 0;

    private void Start()
    {
        GameManager.instance.restartGame.AddListener(Restart);
        initialPosition = transform.position;
    }

    private void Restart()
    {
        transform.position = initialPosition;
        transform.rotation = Quaternion.identity;
        movement = Vector2.zero;
        playerRigidbody.velocity = Vector3.zero;
        isRotating = 0;
    }

    public void Move(InputAction.CallbackContext context)
    {
        movement = context.ReadValue<Vector2>() * movementSpeed;
        UpdateMovementDirection(movement);
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (!context.performed || playerRigidbody.velocity.y != 0)
        {
            return;
        }
        Vector3 newVelocity = playerRigidbody.velocity;
        newVelocity.y = jumpSpeed;
        playerRigidbody.velocity = newVelocity;
    }

    public void Rotate(InputAction.CallbackContext context)
    {
        isRotating = (int)context.ReadValue<float>();
    }

    private void Update()
    {
        if (isRotating != 0)
        {
            transform.Rotate(0, rotationAngle * isRotating, 0);
            UpdateMovementDirection(movement);
        }

        if (playerRigidbody.velocity.y != 0)
        {
            playerRigidbody.velocity += Vector3.up * Time.deltaTime * Physics.gravity.y * playerRigidbody.mass;
        }
    }

    private void UpdateMovementDirection(Vector2 actualMovement)
    {
        Vector3 newVelocity = playerRigidbody.velocity;
        newVelocity.x = actualMovement.y * transform.forward.x;
        newVelocity.z = actualMovement.y * transform.forward.z;
        newVelocity.x += actualMovement.x * transform.right.x;
        newVelocity.z += actualMovement.x * transform.right.z;

        playerRigidbody.velocity = newVelocity;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (CheckTags(collision.gameObject) == Tags.MOVING_PLATFORM)
        {
            transform.parent = collision.transform;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (CheckTags(collision.gameObject) == Tags.MOVING_PLATFORM)
        {
            transform.parent = null;
        }
        UpdateMovementDirection(movement);
    }

    private void OnTriggerEnter(Collider other)
    {
        switch (CheckTags(other.gameObject))
        {
            case Tags.COIN:
                GameManager.instance.GetCoin();
                break;
            case Tags.ENEMY or Tags.SPIKES:
                GameManager.instance.Restart(true);
                break;
        }
    }

    private Tags CheckTags(GameObject gameObject)
    {
        TagsScript tags = gameObject.GetComponent<TagsScript>();
        if (tags == null)
        {
            return Tags.NONE;
        }

        return tags.tags;
    }

    private bool CheckContactsPoint(Collision collision)
    {
        ContactPoint[] contacts = new ContactPoint[collision.contactCount];
        int contactCount = collision.GetContacts(contacts);
        for (int i = 0; i < contactCount; ++i)
        {
            if (contacts[i].normal.y == 1)
            {
                return true;
            }
        }
        return false;
    }
}
