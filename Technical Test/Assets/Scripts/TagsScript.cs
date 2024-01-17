using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TagsScript : MonoBehaviour
{
    public enum Tags{
        NONE=-1,
        PLAYER,
        GROUND,
        COIN,
        SPIKES,
        ENEMY,
        MOVING_PLATFORM_CHECKPOINT
    }

    public Tags tags = Tags.NONE;
}
