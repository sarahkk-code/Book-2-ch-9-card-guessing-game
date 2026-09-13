using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PreferencesManager : MonoBehaviour
{
    public TMP_InputField playerNameInput;
    public TMP_Dropdown cardsDropdown;
    public TMP_Dropdown timeDropdown;

    public void StartGame()
    {
        // Get the player's name
        string playerName = playerNameInput.text;

        // If no name was entered, use "Player"
        if (string.IsNullOrWhiteSpace(playerName))
        {
            playerName = "Player";
        }

        // Save the player's name
        PlayerPrefs.SetString("PlayerName", playerName);

        // Save the number of cards selected
        PlayerPrefs.SetInt("NumberOfCards", cardsDropdown.value);

        // Save the selected time
        int selectedTime = GetSelectedTime();
        PlayerPrefs.SetInt("GameTime", selectedTime);

        // Save PlayerPrefs
        PlayerPrefs.Save();

        // Go to the Game scene
        SceneManager.LoadScene("Game");
    }


    int GetSelectedTime()
    {
        switch (timeDropdown.value)
        {
            case 0:
                return 30;

            case 1:
                return 45;

            case 2:
                return 60;

            case 3:
                return 90;

            case 4:
                return 120;

            default:
                return 30;
        }
    }


    public void GoBack()
    {
        SceneManager.LoadScene("Intro");
    }
}