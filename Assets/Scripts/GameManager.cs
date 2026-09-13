using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject letter, cen;
    string wordToGuess = "";
    int lengthOfWordToGuess;
    char[] lettersToGuess;
    bool[] lettersGuessed;
    string[] wordsToGuess = { "car", "elephant", "autocar" };
    int nbAttempts, maxNbAttempts;
    int score = 0;

    string PickAWordFromFile()

    {

        TextAsset t1 = (TextAsset)Resources.Load("words", typeof(TextAsset));

        string s = t1.text;

        string[] words = s.Split ("\n"[0]);

        int randomWord = Random.Range (0, words.Length + 1);

        return (words[randomWord]);

    }

    void UpdateScore()
    {
        GameObject.Find("scoreUI").GetComponent<TextMeshProUGUI>().text = "Score" + score;

    }
    void Start()
    {
        nbAttempts = 0;
        maxNbAttempts = 10;
        UpdateNbAttempts();
        cen = GameObject.Find("centerOfScreen");
        InitGame();
        InitLetters();
        UpdateScore();



    }

    void CheckKeyboard()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            for (int i = 0; i < lengthOfWordToGuess; i++)
            {
                if (!lettersGuessed[i])
                {
                    if (lettersToGuess[i] == 'A')
                    {
                        lettersGuessed[i] = true;
                        GameObject.Find("letter" + (i + 1)).GetComponent<TextMeshProUGUI>().text = "A";
                    }
                }

            }


        }

    }
    void InitGame()
    {

        // wordToGuess = "Elephant";
        int randomNumber = Random.Range(0, wordToGuess.Length);
        //wordToGuess = wordsToGuess[randomNumber];
        wordToGuess = PickAWordFromFile();
        lengthOfWordToGuess = wordToGuess.Length;
        wordToGuess = wordToGuess.ToUpper();
        lettersToGuess = new char[lengthOfWordToGuess];
        lettersGuessed = new bool[lengthOfWordToGuess];
        lettersToGuess = wordToGuess.ToCharArray();

    }

    void InitLetters()
    {
        int nbLetters = lengthOfWordToGuess;//5;
        for (int i = 0; i < nbLetters; i++)
        {
            Vector3 newPosition;
            newPosition = new Vector3(cen.transform.position.x + ((i - nbLetters / 2.0f) * 100), cen.transform.position.y, cen.transform.position.z);
            GameObject l = Instantiate(letter, newPosition, Quaternion.identity);
            l.name = "letter" + (i + 1);
            l.transform.SetParent(GameObject.Find("Canvas").transform);


        }


    }
    // Update is called once per frame
    void Update()
    {
        CheckKeyboard2();
    }


    void UpdateNbAttempts()
    {
        GameObject.Find("nbAttempts").GetComponent<TextMeshProUGUI>().text = nbAttempts + "/" + maxNbAttempts;


    }
    void CheckKeyboard2()

    {

        if (Input.anyKeyDown && !Input.GetMouseButtonDown(0))

        {

            char letterPressed = Input.inputString.ToCharArray()[0];

            int letterPressedAsInt = System.Convert.ToInt32(letterPressed);

            if (letterPressedAsInt >= 97 && letterPressed <= 122)

            {
                nbAttempts++;
                UpdateNbAttempts();
                CheckIfWordWasFound();
                if (nbAttempts > maxNbAttempts)
                {
                    SceneManager.LoadScene("wordGameEnd");

                }
                for (int i = 0; i < lengthOfWordToGuess; i++)

                {

                    if (!lettersGuessed[i])

                    {

                        letterPressed = System.Char.ToUpper(letterPressed);

                        if (lettersToGuess[i] == letterPressed)

                        {

                            lettersGuessed[i] = true;

                            GameObject.Find("letter" + (i + 1)).GetComponent<TextMeshProUGUI>().text = letterPressed.ToString();
                            score = PlayerPrefs.GetInt("score");
                            score++;
                            PlayerPrefs.SetInt("score", score);
                            UpdateScore();
                            CheckIfWordWasFound();
                        }

                    }

                }

            }

        }
    }
    
    void CheckIfWordWasFound()

{

                bool condition = true;

                for (int i = 0; i < lengthOfWordToGuess; i++)

                {

                                condition = condition && lettersGuessed [i];

                }

                if (condition)

                {

                                PlayerPrefs.SetString ("lastWordGuessed", wordToGuess);

                                SceneManager.LoadScene ("wordGameWin");

                }

}
}
