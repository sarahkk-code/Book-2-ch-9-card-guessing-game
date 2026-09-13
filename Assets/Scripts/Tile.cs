using UnityEngine;

public class Tile : MonoBehaviour
{
    private bool tileRevealed = false;

    public Sprite originalSprite;
    public Sprite hiddenSprite;

    void Start()
    {
        HideCard();
    }

    public void OnMouseDown()
    {
        print("CARD CLICKED: " + gameObject.name);

        GameObject gameManager = GameObject.Find("gameManager");

        if (gameManager != null)
        {
            ManageCards manager = gameManager.GetComponent<ManageCards>();

            if (manager != null)
            {
                manager.CardSelected(gameObject);
            }
            else
            {
                print("ERROR: ManageCards script not found!");
            }
        }
        else
        {
            print("ERROR: gameManager not found!");
        }
    }

    public void HideCard()
    {
        if (hiddenSprite != null)
        {
            GetComponent<SpriteRenderer>().sprite = hiddenSprite;
        }

        tileRevealed = false;
    }

    public void RevealCard()
    {
        if (originalSprite != null)
        {
            GetComponent<SpriteRenderer>().sprite = originalSprite;
            tileRevealed = true;
        }
        else
        {
            print("ERROR: Original Sprite is missing!");
        }
    }

    public void SetOriginalSprite(Sprite newSprite)
    {
        originalSprite = newSprite;
    }
}