using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerScript : MonoBehaviour
{
    [Tooltip("The rigidbody of the player (used for the movements and rotations)")]
    [SerializeField]
    private Rigidbody playerRigidbody = null;

    [Tooltip("The audio source of the player that will make a sound when being killed")]
    [SerializeField]
    private AudioSource source = null;

    [Tooltip("A gameobject that represents the ground the player is standing on (if null, the player is not on ground)")]
    private GameObject ground = null;

    [Tooltip("The initial position of the player")]
    public Vector3 initialPosition = Vector3.zero;

    [Tooltip("This vector2 saves the movement the player should do based solely on input")]
    private Vector2 movement = Vector2.zero;

    [Tooltip("The speed the player moves")]
    [SerializeField]
    private float movementSpeed = 0;

    [Tooltip("The speed the player jumps")]
    [SerializeField]
    private float jumpSpeed = 0;

    [Tooltip("The speed the player rotates")]
    [SerializeField]
    private float rotationSpeed = 0;

    [Tooltip("This defines if the player rotates and where it rotates (-1 rotates to the left, 1 rotates to the right, 0 means the player isn't rotating)")]
    private int isRotating = 0;


    private void Awake()
    {
        if (playerRigidbody == null)
        {
            playerRigidbody = gameObject.GetComponent<Rigidbody>();
        }
        if (source == null)
        {
            source = gameObject.GetComponent<AudioSource>();
        }
    }

    private void Start()
    {
        GameManager.instance.restartGame.AddListener(Restart);
        initialPosition = transform.position;
    }

    private void Restart(GameObject checkPoint)
    {
        //Move the player to either the last checkpoint, or if there isn't any, the initial position
        transform.position = checkPoint == null ? initialPosition : checkPoint.transform.position;
        transform.rotation = Quaternion.identity;
        movement = Vector2.zero;
        playerRigidbody.velocity = Vector3.zero;
        isRotating = 0;
    }

    /// <summary>
    /// This function is called when the player uses the restart button (restarting to last checkpoint)
    /// </summary>
    public void RestartButton(InputAction.CallbackContext context)
    {
        if (context.performed && Time.timeScale != 0)
        {
            GameManager.instance.Restart(true);
        }
    }

    /// <summary>
    /// This function is called when the player uses the move buttons
    /// </summary>
    public void Move(InputAction.CallbackContext context)
    {
        movement = context.ReadValue<Vector2>() * movementSpeed;
        UpdateVelocity(movement);
    }

    /// <summary>
    /// This function is called when the player uses the jump button
    /// </summary>
    public void Jump(InputAction.CallbackContext context)
    {
        if (!context.started || ground == null || Time.timeScale == 0)
        {
            return;
        }

        Vector3 newVelocity = playerRigidbody.velocity;
        newVelocity.y = jumpSpeed;
        playerRigidbody.velocity = newVelocity;
    }

    /// <summary>
    /// This function is called when the player uses the rotate buttons
    /// </summary>
    public void Rotate(InputAction.CallbackContext context)
    {
        isRotating = (int)context.ReadValue<float>();
    }

    private void Update()
    {
        //If the player is rotating do the rotation and update the velocity (the direction may change when rotating the player)
        if (isRotating != 0)
        {
            transform.Rotate(0, rotationSpeed * isRotating * Time.deltaTime, 0);
            UpdateVelocity(movement);
        }

        //If the player is falling, take the gravity and player mass into account to the player's velocity
        if (playerRigidbody.velocity.y != 0)
        {
            playerRigidbody.velocity += Vector3.up * Time.deltaTime * Physics.gravity.y * playerRigidbody.mass;
        }
    }

    /// <summary>
    /// This function is called to update the velocity the player has to move (taking into account where the player is facing right now)
    /// </summary>
    /// <param name="inputMovement"> The velocity of the player based on the input (axis y for up/down, axis x for left/right)</param>
    private void UpdateVelocity(Vector2 inputMovement)
    {
        //Convert the actualMovement variable to the actual velocity multiplying by the forward and right vectors
        Vector3 newVelocity = playerRigidbody.velocity;
        newVelocity.x = inputMovement.y * transform.forward.x;
        newVelocity.z = inputMovement.y * transform.forward.z;
        newVelocity.x += inputMovement.x * transform.right.x;
        newVelocity.z += inputMovement.x * transform.right.z;

        playerRigidbody.velocity = newVelocity;
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Check the contact points of the collision and if the normal y is 1 it means the player is on top of the other object, making the other object the ground
        ContactPoint[] contacts = new ContactPoint[collision.contactCount];
        int contactCount = collision.GetContacts(contacts);
        for (int i = 0; i < contactCount; ++i)
        {
            if (contacts[i].normal.y == 1)
            {
                ground = collision.gameObject;

                //If the ground is a moving platform, the player becomes its child (to make the player move by default the same as the moving platform)
                if (collision.gameObject.tag == "Moving Platform")
                {
                    transform.parent = collision.transform;
                }
                return;
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        //If the player exits a collision, it may happen that a movement that was limited by the collision, now it isn't, so we update the velocity to make sure this doesn't happen
        UpdateVelocity(movement);

        //If the collision object the player exits is the ground, we set the ground null
        if (collision.gameObject.Equals(ground))
        {
            ground = null;

            //If the collision was with a moving platform the player becomes the child of nothing again
            if (collision.gameObject.tag == "Moving Platform")
            {
                transform.parent = null;
            }
            return;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case "Coin":
                GameManager.instance.GetCoin();
                break;
            case "Obstacle":
                source.Play();
                GameManager.instance.Restart(true);
                break;
        }
    }
}
