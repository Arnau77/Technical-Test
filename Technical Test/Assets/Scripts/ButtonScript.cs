using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonScript : TagsScript
{
    [SerializeField]
    private MovingPlatformScript platformToActivate = null;

    private MeshRenderer buttonMeshRenderer = null;

    [SerializeField]
    private Material buttonDeactivatedMaterial = null;

    [SerializeField]
    private Material buttonActivatedMaterial = null;

    [SerializeField]
    private AudioSource source = null;

    private bool isButtonActivated = false;

    private void Awake()
    {
        buttonMeshRenderer = gameObject.GetComponent<MeshRenderer>();
    }

    private void Start()
    {
        GameManager.instance.restartGame.AddListener(Restart);
    }

    private void Restart(bool isUsingCheckPoint)
    {
        isButtonActivated = false;
        ChangeMaterial(buttonDeactivatedMaterial);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!CheckIfCollisionIsPlayer(collision.gameObject) && !isButtonActivated)
        {
            return;
        }

        isButtonActivated = true;
        source.Play();
        ChangeMaterial(buttonActivatedMaterial);
        platformToActivate.Activate();
    }

    private void OnCollisionExit(Collision collision)
    {
        if (!CheckIfCollisionIsPlayer(collision.gameObject)){
            return;
        }

        isButtonActivated = false;
        ChangeMaterial(buttonDeactivatedMaterial);
    }

    private bool CheckIfCollisionIsPlayer(GameObject collisionObject)
    {
        TagsScript tags = collisionObject.GetComponent<TagsScript>();

        if (tags == null || tags.tags != Tags.PLAYER)
        {
            return false;
        }

        return true;
    }

    private void ChangeMaterial(Material newMaterial)
    {
        if (buttonMeshRenderer == null || newMaterial == null)
        {
            return;
        }
        buttonMeshRenderer.material = newMaterial;
    }
}
