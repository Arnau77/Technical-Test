using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerScript : MonoBehaviour
{
    public Rigidbody playerRigidbody = null;

    public float movementSpeed = 0;

    public float jumpSpeed = 0;

    private bool isOnGround = false;

    public void Move(InputAction.CallbackContext context)
    {
        Vector2 input = context.ReadValue<Vector2>() * movementSpeed;
        Vector3 newVelocity = Vector3.zero;
        newVelocity.x = input.x;
        newVelocity.z = input.y;
        playerRigidbody.velocity = newVelocity;
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

    private void OnCollisionEnter(Collision collision)
    {
        isOnGround = CheckContactsPoint(collision);
    }
    private void OnCollisionExit(Collision collision)
    {
        isOnGround = CheckContactsPoint(collision);
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
