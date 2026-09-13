using UnityEngine;
using TMPro;

public class ExitManager : MonoBehaviour
{
    public TextMeshProUGUI exitMessage;
    public GameObject saveScoreButton;

    void Start()
    {
        string playerName = PlayerPrefs.GetString("PlayerName", "Player");
        int currentScore = PlayerPrefs.GetInt("CurrentScore", 0);
        int highestScore = PlayerPrefs.GetInt("HighestScore", 0);

        exitMessage.text =
            "Game Over, " + playerName + "!\n\n" +
            "Your Score: " + currentScore + "\n" +
            "Highest Score: " + highestScore;

        // Only show Save Score if the player beat the highest score
        if (currentScore > highestScore)
        {
            saveScoreButton.SetActive(true);
        }
        else
        {
            saveScoreButton.SetActive(false);
        }
    }


    public void SaveScore()
    {
        int currentScore = PlayerPrefs.GetInt("CurrentScore", 0);

        PlayerPrefs.SetInt("HighestScore", currentScore);
        PlayerPrefs.Save();

        string playerName = PlayerPrefs.GetString("PlayerName", "Player");

        exitMessage.text =
            "Great job, " + playerName + "!\n\n" +
            "Your Score: " + currentScore + "\n" +
            "Highest Score: " + currentScore;

        saveScoreButton.SetActive(false);
    }
}