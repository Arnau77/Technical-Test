using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinScript : MonoBehaviour
{
    [Tooltip("The speed that the coin will rotate when active")]
    [SerializeField]
    private float rotationSpeed = 0;

    // Start is called before the first frame update
    void Start()
    {
        //Add this coin to the list of coins of the Game Manager
        GameManager.instance.AddCoin(gameObject);
    }

    private void Update()
    {
        //Rotate this coin while its active
        transform.Rotate(Vector3.right, rotationSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        //Check if the collider that triggered is the player
        if (other.tag == "Player")
        {
            //If it is, deactivate this coin
            gameObject.SetActive(false);
        }
    }
}
