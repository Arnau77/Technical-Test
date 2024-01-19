using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonScript : MonoBehaviour
{
    [Tooltip("The platform that will activate when the button is pressed")]
    [SerializeField]
    private MovingPlatformScript platformToActivate = null;

    [Tooltip("The mesh renderer of the button (needed to change materials)")]
    [SerializeField]
    private MeshRenderer buttonMeshRenderer = null;

    [Tooltip("The material of the button when is deactivated")]
    [SerializeField]
    private Material buttonDeactivatedMaterial = null;

    [Tooltip("The material of the button when is activated")]
    [SerializeField]
    private Material buttonActivatedMaterial = null;

    [Tooltip("The audio source of the button that will make a sound when the button activates it")]
    [SerializeField]
    private AudioSource source = null;

    [Tooltip("Whether the button is activated or not")]
    private bool isButtonActivated = false;

    private void Awake()
    {
        if (buttonMeshRenderer == null)
        {
            buttonMeshRenderer = gameObject.GetComponent<MeshRenderer>();
        }

        if (source == null)
        {
            source = gameObject.GetComponent<AudioSource>();
        }
    }

    private void Start()
    {
        GameManager.instance.restartGame.AddListener(Restart);
    }

    /// <summary>
    /// This function is called when restarting the game
    /// </summary>
    /// <param name="checkPoint"> The checkpoint the game will restart from (if it's null, the game restarts from the beginning)</param>
    private void Restart(GameObject checkPoint)
    {
        isButtonActivated = false;
        ChangeMaterial(buttonDeactivatedMaterial);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag != "Player" && !isButtonActivated)
        {
            return;
        }

        //If the player is entering the button and it's deactivated, we activate it, play the activation sound, activate the moving platform (if is deactivated) and change its material to the activated one
        isButtonActivated = true;
        source.Play();
        ChangeMaterial(buttonActivatedMaterial);
        platformToActivate.Activate();
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag != "Player")
        {
            return;
        }

        //If the player is exiting the button, we deactivate it and change its material to the deactivated one
        isButtonActivated = false;
        ChangeMaterial(buttonDeactivatedMaterial);
    }

    /// <summary>
    /// This function is called to change the material of the button
    /// </summary>
    /// <param name="newMaterial"> The new material the button will be having</param>
    private void ChangeMaterial(Material newMaterial)
    {
        if (buttonMeshRenderer == null || newMaterial == null)
        {
            return;
        }
        buttonMeshRenderer.material = newMaterial;
    }
}
