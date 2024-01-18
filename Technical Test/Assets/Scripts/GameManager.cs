using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    public UnityEvent restartGame = new UnityEvent();

    [SerializeField]
    private TextMeshProUGUI coinsText = null;
    
    [SerializeField]
    private GameObject victoryPanel = null;

    [SerializeField]
    private GameObject player = null;

    private List<GameObject> coins = new List<GameObject>();

    private List<GameObject> savedCoins = new List<GameObject>();


    private int coinsCollected = 0;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void Restart(bool usingCheckpoint)
    {
        victoryPanel.SetActive(false);

        foreach (GameObject coin in coins)
        {
            if (!coin.activeInHierarchy && (!usingCheckpoint || !savedCoins.Contains(coin)))
            {
                coin.SetActive(true);
                coinsCollected --;
            }
        }

        if (!usingCheckpoint)
        {
            savedCoins.Clear();
        }

        restartGame.Invoke();

        UpdateCoinText();
    }

    public void Save()
    {
        savedCoins.Clear();

        foreach (GameObject coin in coins)
        {
            if (!coin.activeInHierarchy)
            {
                savedCoins.Add(coin);
            }
        }
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
