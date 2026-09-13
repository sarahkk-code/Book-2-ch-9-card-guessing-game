using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class ManageCards : MonoBehaviour
{
    public GameObject card;
    public TextMeshProUGUI gameInfo;

    // Put the 10 card sprites in this list in the Inspector
    public Sprite[] cardSprites;

    bool firstCardSelected = false;
    bool secondCardSelected = false;
    bool isProcessing = false;

    GameObject card1;
    GameObject card2;

    string rowForCard1;
    string rowForCard2;

    float matchTimer = 0;

    float gameTimer = 0;
    int gameTime;
    int remainingTime;

    int nbMatch = 0;

    string playerName;


    // =========================
    // START
    // =========================

    void Start()
    {
        playerName = PlayerPrefs.GetString("PlayerName", "Player");
        gameTime = PlayerPrefs.GetInt("GameTime", 30);

        remainingTime = gameTime;
        gameTimer = 0;

        DisplayCards();
        UpdateGameInfo();
    }


    // =========================
    // CARD CLICK
    // =========================

    public void CardSelected(GameObject selectedCard)
    {
        if (isProcessing)
            return;

        // FIRST CARD
        if (!firstCardSelected)
        {
            string row = selectedCard.name.Substring(0, 1);

            rowForCard1 = row;
            card1 = selectedCard;

            firstCardSelected = true;

            card1.GetComponent<Tile>().RevealCard();

            return;
        }


        // SECOND CARD
        else if (!secondCardSelected)
        {
            string row = selectedCard.name.Substring(0, 1);

            // Don't allow two cards from the same row
            if (row == rowForCard1)
                return;

            rowForCard2 = row;
            card2 = selectedCard;

            secondCardSelected = true;

            card2.GetComponent<Tile>().RevealCard();

            isProcessing = true;
            matchTimer = 0;
        }
    }


    // =========================
    // CREATE CARD
    // =========================

    void AddACard(int row, int rank, int value)
    {
        // Spacing between cards
        float xSpacing = 1.55f;

        // Spacing between rows
        float ySpacing = 2.2f;

        GameObject cen = GameObject.Find("centerOfScreen");

        Vector3 newPosition = new Vector3(
            cen.transform.position.x +
            ((rank - 4.5f) * xSpacing),

            cen.transform.position.y +
            ((row - 0.5f) * ySpacing) +
            1.0f,

            cen.transform.position.z
        );

        GameObject newCard = Instantiate(
            card,
            newPosition,
            Quaternion.identity
        );

        newCard.tag = "" + (value + 1);
        newCard.name = "" + row + "_" + value;

        if (cardSprites != null &&
            value >= 0 &&
            value < cardSprites.Length &&
            cardSprites[value] != null)
        {
            newCard.GetComponent<Tile>()
                .SetOriginalSprite(cardSprites[value]);
        }
        else
        {
            Debug.LogError(
                "Card sprite missing for value: " + value
            );
        }
    }


    // =========================
    // DISPLAY 20 CARDS
    // =========================

    public void DisplayCards()
    {
        int[] shuffledArray = CreateShuffledArray();
        int[] shuffledArray2 = CreateShuffledArray();

        for (int i = 0; i < 10; i++)
        {
            AddACard(0, i, shuffledArray[i]);
            AddACard(1, i, shuffledArray2[i]);
        }
    }


    // =========================
    // SHUFFLE
    // =========================

    public int[] CreateShuffledArray()
    {
        int[] newArray =
            new int[]
            {
                0, 1, 2, 3, 4,
                5, 6, 7, 8, 9
            };

        for (int t = 0; t < 10; t++)
        {
            int r = Random.Range(t, 10);

            int tmp = newArray[t];

            newArray[t] = newArray[r];
            newArray[r] = tmp;
        }

        return newArray;
    }


    // =========================
    // UPDATE
    // =========================

    void Update()
    {
        gameTimer += Time.deltaTime;

        if (gameTimer >= 1)
        {
            gameTimer = 0;

            remainingTime--;

            UpdateGameInfo();

            if (remainingTime <= 0)
            {
                remainingTime = 0;

                UpdateGameInfo();

                PlayerPrefs.SetInt(
                    "CurrentScore",
                    nbMatch
                );

                PlayerPrefs.Save();

                SceneManager.LoadScene("Exit");

                return;
            }
        }


        // MATCH TIMER
        if (isProcessing)
        {
            matchTimer += Time.deltaTime;

            if (matchTimer >= 1)
            {
                CheckMatch();

                matchTimer = 0;
            }
        }
    }


    // =========================
    // CHECK MATCH
    // =========================

    void CheckMatch()
    {
        if (card1 == null || card2 == null)
        {
            ResetSelection();
            return;
        }

        if (card1.CompareTag(card2.tag))
        {
            Destroy(card1);
            Destroy(card2);

            nbMatch++;

            if (nbMatch >= 10)
            {
                PlayerPrefs.SetInt(
                    "CurrentScore",
                    nbMatch
                );

                PlayerPrefs.Save();

                SceneManager.LoadScene("Exit");

                return;
            }
        }
        else
        {
            card1.GetComponent<Tile>().HideCard();
            card2.GetComponent<Tile>().HideCard();
        }

        ResetSelection();
    }


    // =========================
    // RESET
    // =========================

    void ResetSelection()
    {
        firstCardSelected = false;
        secondCardSelected = false;
        isProcessing = false;

        card1 = null;
        card2 = null;

        rowForCard1 = "";
        rowForCard2 = "";

        matchTimer = 0;
    }


    // =========================
    // GAME INFO
    // =========================

    void UpdateGameInfo()
    {
        if (gameInfo != null)
        {
            gameInfo.text =
                "Player: " + playerName +
                "    Time: " + remainingTime;
        }
    }


    // =========================
    // STOP GAME
    // =========================

    public void StopGame()
    {
        PlayerPrefs.SetInt(
            "CurrentScore",
            nbMatch
        );

        PlayerPrefs.Save();

        SceneManager.LoadScene("Exit");
    }
}