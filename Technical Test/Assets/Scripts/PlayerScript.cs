using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerScript : TagsScript
{
    public Rigidbody playerRigidbody = null;

    private Vector2 movement = Vector2.zero;

    public float movementSpeed = 0;

    public float jumpSpeed = 0;

    public float rotationAngle = 0;

    private int isRotating = 0;

    private bool isOnGround = false;

    private void Start()
    {
        GameManager.instance.playerInitialPosition = transform.position;
    }

    public void Move(InputAction.CallbackContext context)
    {
        movement = context.ReadValue<Vector2>() * movementSpeed;
        UpdateMovementDirection(movement);
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (!context.performed || !isOnGround)
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
        Tags collisionTag = CheckTags(collision.gameObject);
        if (collisionTag == Tags.GROUND || collisionTag == Tags.BUTTON)
        {
            isOnGround = CheckContactsPoint(collision);
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        Tags collisionTag = CheckTags(collision.gameObject);
        if (collisionTag == Tags.GROUND || collisionTag == Tags.BUTTON)
        {
            isOnGround = CheckContactsPoint(collision);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        switch (CheckTags(other.gameObject))
        {
            case Tags.COIN:
                GameManager.instance.GetCoin();
                break;
            case Tags.ENEMY or Tags.SPIKES:
                GameManager.instance.Restart();
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
