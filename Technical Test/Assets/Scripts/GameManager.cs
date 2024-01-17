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

    private List<GameObject> coins = new List<GameObject>();

    private int coinsCollected = 0;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void Restart()
    {
        foreach(GameObject coin in coins)
        {
            coin.SetActive(true);
        }

        coinsCollected = 0;

        UpdateCoinText();
    }

    public void AddCoin(GameObject coin)
    {
        coins.Add(coin);
    }

    public void GetCoin()
    {
        coinsCollected++;

        UpdateCoinText();

        if (coinsCollected == coins.Count && victoryPanel != null)
        {
            victoryPanel.SetActive(true);
        }
    }

    private void UpdateCoinText()
    {
        if (coinsText != null)
        {
            coinsText.text = coinsCollected.ToString();
        }
    }
}
