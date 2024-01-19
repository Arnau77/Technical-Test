using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    public UnityEvent<bool> restartGame = new UnityEvent<bool>();

    public UnityEvent<Vector3> updatePosition = new UnityEvent<Vector3>();

    [SerializeField]
    private TextMeshProUGUI coinsText = null;
    
    [SerializeField]
    private GameObject victoryPanel = null;

    [SerializeField]
    private GameObject pausePanel = null;

    [SerializeField]
    private GameObject player = null;

    [SerializeField]
    private AudioSource source = null;

    [SerializeField]
    private AudioSource victoryPanelSource = null;

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
        PauseGame(false);

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

        restartGame.Invoke(usingCheckpoint);

        UpdateCoinText();
    }

    public void Save(Transform transform)
    {
        savedCoins.Clear();

        foreach (GameObject coin in coins)
        {
            if (!coin.activeInHierarchy)
            {
                savedCoins.Add(coin);
            }
        }

        updatePosition.Invoke(transform.position);
    }

    public void AddCoin(GameObject coin)
    {
        coins.Add(coin);
    }

    public void GetCoin()
    {
        coinsCollected++;
        source.Play();

        UpdateCoinText();

        if (coinsCollected == coins.Count && victoryPanel != null)
        {
            victoryPanel.SetActive(true);
            victoryPanelSource.Play();
            PauseGame(true);
        }
    }

    private void UpdateCoinText()
    {
        if (coinsText != null)
        {
            coinsText.text = coinsCollected.ToString();
        }
    }

    public void OpenPausePanel()
    {
        pausePanel.SetActive(true);
        PauseGame(true);
    }

    public void PauseGame(bool isBeingPaused)
    {
        Time.timeScale = isBeingPaused ? 0 : 1;
    }

    public void Quit()
    {
        Application.Quit();
    }
}
