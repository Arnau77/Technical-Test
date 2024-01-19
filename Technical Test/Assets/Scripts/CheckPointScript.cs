using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointScript : MonoBehaviour
{
    [Tooltip("The mesh renderer of the checkpoint")]
    [SerializeField]
    private MeshRenderer meshRenderer = null;

    [Tooltip("The audio source of the checkpoint (that will make a sound when the player touches it)")]
    [SerializeField]
    private AudioSource source = null;

    // Start is called before the first frame update
    void Awake()
    {
        if (meshRenderer == null)
        {
            meshRenderer = gameObject.GetComponent<MeshRenderer>();
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
        //If the checkpoint that the player is respawning is this, the checkpoints disables its mesh to avoid obstructing the player's vision
        if (checkPoint.Equals(gameObject))
        {
            meshRenderer.enabled = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Player" || !meshRenderer.enabled)
        {
            return;
        }

        source.Play();

        //Call the game manager to save the game in this checkpoint
        GameManager.instance.Save(gameObject);

        //Disable the checkpoint's mesh to avoid obstructing the player's vision
        meshRenderer.enabled = false;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            //When the player exits the checkpoint, enable the mesh again to let the player know where is the checkpoint
            meshRenderer.enabled = true;
        }
    }
}
