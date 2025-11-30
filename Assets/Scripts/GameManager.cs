using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public enum GameState
{
    Title,
    Instructions,
    Playing,
    GameOver
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public GameObject titleScreenPanel;
    public GameObject instructionsPanel;
    public GameObject hudPanel;
    public GameObject gameOverPanel;
    public TextMeshProUGUI scoreText;

    private int score = 0;
    public GameState currentState = GameState.Title;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        UpdateGameState(GameState.Title);
    }

    public void UpdateGameState(GameState newState)
    {
        currentState = newState;

        titleScreenPanel.SetActive(newState == GameState.Title);
        instructionsPanel.SetActive(newState == GameState.Instructions);
        hudPanel.SetActive(newState == GameState.Playing);
        gameOverPanel.SetActive(newState == GameState.GameOver);

        Time.timeScale = (newState == GameState.Playing) ? 1f : 0f;
    }

    // UI Button Functions
    public void StartGame()
    {
        score = 0;
        scoreText.text = "Score: 0";
        UpdateGameState(GameState.Playing);
    }

    public void ShowInstructions()
    {
        UpdateGameState(GameState.Instructions);
    }

    public void BackToTitle()
    {
        UpdateGameState(GameState.Title);
    }

    public void GameOver()
    {
        UpdateGameState(GameState.GameOver);
    }

    public void PlayAgain()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    // For scoring later
    public void AddScore(int amount)
    {
        score += amount;
        scoreText.text = "Score: " + score;
    }
}
