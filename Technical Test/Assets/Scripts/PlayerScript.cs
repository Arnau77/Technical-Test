using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerScript : TagsScript
{
    public Rigidbody playerRigidbody = null;

    public float movementSpeed = 0;

    public float jumpSpeed = 0;

    private bool isOnGround = false;

    private void Start()
    {
        GameManager.instance.playerInitialPosition = transform.position;
    }

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
        if (CheckTags(collision.gameObject) == Tags.GROUND)
        {
            isOnGround = CheckContactsPoint(collision);
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (CheckTags(collision.gameObject) == Tags.GROUND)
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
