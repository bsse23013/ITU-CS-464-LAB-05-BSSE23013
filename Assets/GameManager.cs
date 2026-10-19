using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

// Lecture 06, Part 4, extended: owns the score, the red-coin trigger,
// the hidden BonusCoins UI, and the win.
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Score & win")]
    [SerializeField] int targetScore = 10;
    [SerializeField] TMP_Text scoreText;
    [SerializeField] GameObject winPanel;

    [Header("TASK 1: red coin")]
    [SerializeField] Spawner spawner;
    [SerializeField] int redCoinMinScore = 1;
    [SerializeField] int redCoinMaxScore = 10;   // inclusive

    [Header("TASKS 2 + 3: bonus round")]
    [SerializeField] TMP_Text bonusText;         // starts hidden
    [SerializeField] int bonusCoinCount = 50;

    int score;
    int redCoinScore;          // the secret score that triggers the red coin
    bool redCoinSpawned;       // true once, so only ONE red coin ever appears
    bool redCoinCollected;
    bool bonusActive;
    int bonusCollected;
    bool won;

    void Awake()
    {
        Instance = this;
        // int Random.Range excludes its max, so +1 makes 10 possible
        redCoinScore = Random.Range(redCoinMinScore, redCoinMaxScore + 1);
        Debug.Log($"Red coin will spawn when score reaches {redCoinScore}");
    }

    void Start()
    {
        if (bonusText != null) bonusText.gameObject.SetActive(false);   // hidden UI
        UpdateUI();
    }

    // ---------- normal coins ----------
    public void AddScore(int amount)
    {
        score += amount;
        UpdateUI();

        // TASK 1: spawn the red coin once, when the random score is reached
        if (!redCoinSpawned && score >= redCoinScore)
        {
            redCoinSpawned = true;
            spawner.SpawnRedCoin();
        }

        TryWin();
    }

    // ---------- TASK 2 + 3: red coin collected ----------
    public void OnRedCoinCollected()
    {
        redCoinCollected = true;
        bonusActive = true;
        bonusCollected = 0;

        spawner.SpawnBonusCoins(bonusCoinCount);   // 50 blue coins around the player

        bonusText.gameObject.SetActive(true);      // reveal the hidden text
        UpdateBonusUI();
    }

    // ---------- blue coins ----------
    public void OnBlueCoinCollected()
    {
        bonusCollected++;
        UpdateBonusUI();

        if (bonusCollected >= bonusCoinCount)
        {
            bonusActive = false;
            TryWin();
        }
    }

    // ---------- UI ----------
    void UpdateUI() => scoreText.text = $"Score: {score} / {targetScore}";
    void UpdateBonusUI() => bonusText.text = $"BonusCoins: {bonusCollected}/{bonusCoinCount}";

    // ---------- win ----------
    // Don't end the game while the red coin is waiting to be collected
    // or while the bonus round is still running.
    void TryWin()
    {
        bool bonusPending = (redCoinSpawned && !redCoinCollected) || bonusActive;
        if (!won && score >= targetScore && !bonusPending) Win();
    }

    void Win()
    {
        won = true;
        winPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void Restart()
    {
        Time.timeScale = 1f;   // survives a scene load, so reset it first
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
