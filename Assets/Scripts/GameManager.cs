using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public delegate void GameDelegate();
    public static event GameDelegate OnGameStarted;
    public static event GameDelegate OnGameOverConfirmed;

    public static GameManager Instance;

    public GameObject StartPage;
    public GameObject GameOverPage;
    public GameObject CountDownPage;
    public Text scoreText;
    

    enum PageState
    {
        None,
        Start,
        GameOver,
        Countdown
    }

    int score = 0;
    bool gameOver = true;

    public bool GameOver { get { return gameOver; } }
    public int Score { get { return score; } }

    void Awake()
    {
       Instance = this;
    }

    void OnEnable()
    {
        CountdownText.OnCountdownFinished += OnCountdownFinished;
        PlayerControler.OnPlayerDied += OnPlayerDied;
        PlayerControler.OnPlayerScored += OnPlayerScored;
    }

    void OnDisable()
    {
        CountdownText.OnCountdownFinished -= OnCountdownFinished;
        PlayerControler.OnPlayerDied -= OnPlayerDied;
        PlayerControler.OnPlayerScored -= OnPlayerScored;
    }

    void OnCountdownFinished()
    {
        SetPageState(PageState.None);
        OnGameStarted();
        score = 0;
        gameOver = false;
    }

    void OnPlayerDied()
    {
        gameOver = true;
        int savedScore = PlayerPrefs.GetInt("HighScore");
        if (score > savedScore)
        {
            PlayerPrefs.SetInt("HighScore", score);
        }

        SetPageState(PageState.GameOver);
    }

        void OnPlayerScored()
        {
            score++;
            scoreText.text = score.ToString();
        }
    

   

    void SetPageState(PageState state)
    {
        switch (state)
        {
            case PageState.None:
                StartPage.SetActive(false);
                GameOverPage.SetActive(false);
                CountDownPage.SetActive(false);

                break;
            case PageState.Start:
                StartPage.SetActive(true);
                GameOverPage.SetActive(false);
                CountDownPage.SetActive(false);

                break;
            case PageState.GameOver:
                StartPage.SetActive(false);
                GameOverPage.SetActive(true);
                CountDownPage.SetActive(false);

                break;
            case PageState.Countdown:
                StartPage.SetActive(false);
                GameOverPage.SetActive(false);
                CountDownPage.SetActive(true);

                break;
        }
    }

    public void ConfirmGameOver()
    {
        OnGameOverConfirmed();
        scoreText.text = "0";
        SetPageState(PageState.Start);
    }

    public void StartGame()
    {
        SetPageState(PageState.Countdown);
    }
}
