using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    [Tooltip("The instance of the game manager that all can access")]
    public static GameManager instance = null;

    [Tooltip("The Unity Event that will be called when restarting the game")]
    [HideInInspector]
    public UnityEvent<GameObject> restartGame = new UnityEvent<GameObject>();

    [Tooltip("The component that have the text where it says the coins collected during the game")]
    [SerializeField]
    private TextMeshProUGUI coinsText = null;
    
    [Tooltip("The game object of the victory panel that appears when collecting all coins")]
    [SerializeField]
    private GameObject victoryPanel = null;

    [Tooltip("The game object of the pause panel that appears when pushing the pause button")]
    [SerializeField]
    private GameObject pausePanel = null;

    [Tooltip("The last checkpoint the player has saved, if it's null it means the player hasn't touched a checkpoint in this game")]
    private GameObject lastCheckPoint = null;

    [Tooltip("The audio source of the sound made when collecting coins")]
    [SerializeField]
    private AudioSource source = null;

    [Tooltip("The audio source that the victory panel will activate when appearing")]
    [SerializeField]
    private AudioSource victoryPanelSource = null;

    [Tooltip("The list of all the coins that exist in the game")]
    private List<GameObject> coins = new List<GameObject>();

    [Tooltip("The list of all the coins saved, if the player hasn't saved yet, this list will be empty")]
    private List<GameObject> savedCoins = new List<GameObject>();


    private int coinsCollected = 0;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        if (source == null)
        {
            source = gameObject.GetComponent<AudioSource>();
        }
    }

    /// <summary>
    /// This function is called when restarting the game
    /// </summary>
    /// <param name="checkPoint"> The checkpoint the game will restart from (if it's null, the game restarts from the beginning)</param>
    public void Restart(bool usingCheckpoint)
    {
        //Disable the victory panel and unpause the game
        victoryPanel.SetActive(false);
        PauseGame(false);

        //For every coin, check if they are active and if they not, check whether they have been saved and we are restarting from a checkpoint. If one of those two are false, reactivate the coin and count one
        //less to the total of coins
        foreach (GameObject coin in coins)
        {
            if (!coin.activeInHierarchy && (!usingCheckpoint || !savedCoins.Contains(coin)))
            {
                coin.SetActive(true);
                coinsCollected --;
            }
        }

        //If we are not using a checkpoint to restart, delete the checkpoint and coin information to start anew
        if (!usingCheckpoint)
        {
            lastCheckPoint = null;
            savedCoins.Clear();
        }

        restartGame.Invoke(lastCheckPoint);

        UpdateCoinText();
    }

    /// <summary>
    /// This function is called to save the game after the player touches a checkpoint
    /// </summary>
    /// <param name="checkPoint"> The checkpoint the player has touched</param>
    public void Save(GameObject checkPoint)
    {
        //Clear all the save coins to avoid the same coin being repeated
        savedCoins.Clear();

        lastCheckPoint = checkPoint;

        //Save all the coins that aren't active (the ones the playe has gotten)
        foreach (GameObject coin in coins)
        {
            if (!coin.activeInHierarchy)
            {
                savedCoins.Add(coin);
            }
        }
    }

    /// <summary>
    /// This function is called to add a coin to the coin list (necessary to know the total of coins in the game and when restarting the coins)
    /// </summary>
    /// <param name="coin"> The coin that will be added to the list</param>
    public void AddCoin(GameObject coin)
    {
        coins.Add(coin);
    }

    /// <summary>
    /// This function is called when the player gets a coin
    /// </summary>
    public void GetCoin()
    {
        //Add one to the coins collected variable and play the sound of coin collected
        coinsCollected++;
        source.Play();

        UpdateCoinText();

        //Check whether all the coins have been gotten and if it's true, open the victory panel and pause the game
        if (coinsCollected == coins.Count && victoryPanel != null)
        {
            victoryPanel.SetActive(true);
            victoryPanelSource.Play();
            PauseGame(true);
        }
    }

    /// <summary>
    /// This function is called to update the text that says how coins the player has gotten
    /// </summary>
    private void UpdateCoinText()
    {
        if (coinsText != null)
        {
            coinsText.text = coinsCollected.ToString();
        }
    }

    /// <summary>
    /// This function is called to open the pause panel
    /// </summary>
    public void OpenPausePanel()
    {
        pausePanel.SetActive(true);
        PauseGame(true);
    }

    /// <summary>
    /// This function is called to pause or unpause the game
    /// </summary>
    /// <param name="isBeingPaused"> Whether the game is going to be paused (true) or unpaused (false)</param>
    public void PauseGame(bool isBeingPaused)
    {
        Time.timeScale = isBeingPaused ? 0 : 1;
    }

    /// <summary>
    /// This function is called to quit the game
    /// </summary>
    public void Quit()
    {
        Application.Quit();
    }
}
