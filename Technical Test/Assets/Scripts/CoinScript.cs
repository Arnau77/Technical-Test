using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinScript : TagsScript
{
    // Start is called before the first frame update
    void Start()
    {
        GameManager.instance.AddCoin();
    }

    private void OnTriggerEnter(Collider other)
    {
        TagsScript tags = other.gameObject.GetComponent<TagsScript>();
        if (tags == null || tags.tags != Tags.PLAYER)
        {
            return;
        }
        Destroy(gameObject);
    }
}
