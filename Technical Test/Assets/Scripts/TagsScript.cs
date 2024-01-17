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
        ENEMY
    }

    public Tags tags = Tags.NONE;
}
