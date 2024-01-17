using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    [SerializeField]
    private int numOfCoins = 0;

    [SerializeField]
    private int coinsCollected = 0;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddCoin()
    {
        numOfCoins++;
    }

    public void GetCoin()
    {
        if (++coinsCollected == numOfCoins)
        {
            Debug.Log("FINISH");
        }
    }
}
