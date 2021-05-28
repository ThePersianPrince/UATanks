using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverManager : MonoBehaviour
{
    public GameObject gameOver;
    public GameObject start;
    public List<Text> highScores;
    public Text player1ScoreText;
    public Text player2ScoreText;

    public void Return()
    {
        gameOver.SetActive(false);
        start.SetActive(true);
    }

    public void Update()
    {
        UpdateHighScores();
        UpdatePlayerScores();
    }

    public void UpdateHighScores()
    {
        for (int x = 0; x < highScores.Count; x++)
        {
            string highScoreKey = "High_Score_" + (x + 1).ToString();
            if (PlayerPrefs.HasKey(highScoreKey))
            {
                int score = PlayerPrefs.GetInt(highScoreKey);
                if (score > 0)
                {
                    highScores[x].text = (x + 1).ToString() + ". " + score.ToString();
                }
                else
                {
                    highScores[x].text = (x + 1).ToString() + ". ";
                }
            }
            else
            {
                highScores[x].text = (x + 1).ToString() + ". ";
            }
        }
    }

    public void UpdatePlayerScores()
    {
        if (PlayerPrefs.HasKey("NumPlayers"))
        {
            if (PlayerPrefs.GetInt("NumPlayers") == 1)
            {
                if (PlayerPrefs.HasKey("PlayerOneScore"))
                {
                    player1ScoreText.text = "Player 1 Score - " + PlayerPrefs.GetInt("PlayerOneScore");
                }
            }
            else if (PlayerPrefs.GetInt("NumPlayers") == 2)
            {
                if (PlayerPrefs.HasKey("PlayerOneScore"))
                {
                    player1ScoreText.text = "Player 1 Score - " + PlayerPrefs.GetInt("PlayerOneScore");
                }

                if (PlayerPrefs.HasKey("PlayerTwoScore"))
                {
                    player2ScoreText.text = "Player 2 Score - " + PlayerPrefs.GetInt("PlayerTwoScore");
                }
            }
        }
    }
}
