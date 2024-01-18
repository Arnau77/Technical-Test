using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinScript : TagsScript
{
    public float rotationSpeed = 0;

    // Start is called before the first frame update
    void Start()
    {
        GameManager.instance.AddCoin(gameObject);
    }

    private void Update()
    {
        transform.Rotate(Vector3.right, rotationSpeed);
    }

    private void OnTriggerEnter(Collider other)
    {
        TagsScript tags = other.gameObject.GetComponent<TagsScript>();
        if (tags == null || tags.tags != Tags.PLAYER)
        {
            return;
        }
        gameObject.SetActive(false);
    }
}
