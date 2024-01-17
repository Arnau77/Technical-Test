using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonScript : TagsScript
{
    [SerializeField]
    private MovingPlatformScript platformToActivate = null;

    private bool isButtonActivated = false;

    private void OnCollisionEnter(Collision collision)
    {
        if (!CheckIfCollisionIsPlayer(collision.gameObject) && !isButtonActivated)
        {
            return;
        }

        isButtonActivated = true;
        platformToActivate.Activate();
    }

    private void OnCollisionExit(Collision collision)
    {
        if (!CheckIfCollisionIsPlayer(collision.gameObject)){
            return;
        }

        isButtonActivated = false;
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
