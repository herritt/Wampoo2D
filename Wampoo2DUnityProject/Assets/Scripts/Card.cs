using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    public int spaces = 0;
    public bool isSpecialCard = false;
    public bool isKillerCard = false;
    public bool isStarterCard = false;
    public bool isJoker = false;
    public bool isFour = false;
    public bool isJack = false;
    public bool isFlipped = false;
    private Sprite faceUpSprite;
    private Sprite backOfDeckSprite;

    // Start is called before the first frame update
    void Start()
    {
        faceUpSprite = gameObject.GetComponent<SpriteRenderer>().sprite;
        backOfDeckSprite = gameObject.GetComponentInParent<DeckOfCards>().backOfDeck;
        gameObject.GetComponent<SpriteRenderer>().sprite = backOfDeckSprite;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [ContextMenu("flip")]
    public void flip()
    {
        if (isFlipped)
        {
            isFlipped = false;
            gameObject.GetComponent<SpriteRenderer>().sprite = gameObject.GetComponentInParent<DeckOfCards>().backOfDeck;
        }
        else
        {
            isFlipped = true;
            gameObject.GetComponent<SpriteRenderer>().sprite = faceUpSprite;
        }
    }


}
