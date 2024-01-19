using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointScript : TagsScript
{
    private MeshRenderer meshRenderer = null;

    [SerializeField]
    private AudioSource source = null;

    // Start is called before the first frame update
    void Awake()
    {
        if (meshRenderer == null)
        {
            meshRenderer = GetComponent<MeshRenderer>();
        }
    }

    private void Start()
    {
        GameManager.instance.restartGame.AddListener(Restart);
    }

    private void Restart(GameObject checkPoint)
    {
        if (checkPoint == gameObject)
        {
            meshRenderer.enabled = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!CheckIfCollisionIsPlayer(other.gameObject) || !meshRenderer.enabled){
            return;
        }

        source.Play();

        GameManager.instance.Save(gameObject);

        meshRenderer.enabled=false;
    }

    private void OnTriggerExit(Collider other)
    {
        if (!CheckIfCollisionIsPlayer(other.gameObject)){
            return;
        }
        meshRenderer.enabled = true;
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
}
