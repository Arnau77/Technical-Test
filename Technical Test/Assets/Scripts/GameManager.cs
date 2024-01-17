using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    [SerializeField]
    private TextMeshProUGUI coinsText = null;
    
    [SerializeField]
    private GameObject victoryPanel = null;

    private int numOfCoins = 0;

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
        coinsCollected++;

        if (coinsText != null)
        {
            coinsText.text = coinsCollected.ToString();
        }

        if (coinsCollected == numOfCoins && victoryPanel != null)
        {
            victoryPanel.SetActive(true);
        }
    }
}
