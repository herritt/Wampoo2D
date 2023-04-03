using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Card : MonoBehaviour,IPointerClickHandler
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

    private Vector3 selectedScale = new Vector3(1.2f, 1.2f, 1.2f);
    private Vector3 normalScale;

    // Start is called before the first frame update
    void Start()
    {
        faceUpSprite = gameObject.GetComponent<SpriteRenderer>().sprite;
        backOfDeckSprite = gameObject.GetComponentInParent<DeckOfCards>().backOfDeck;
        gameObject.GetComponent<SpriteRenderer>().sprite = backOfDeckSprite;

        normalScale = gameObject.transform.localScale;
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
    

    public void faceUp()
    {
        isFlipped = false;
        flip();
    }

    public void Selected()
    {


        gameObject.transform.localScale = selectedScale;
    }

    public void UnSelect()
    {
        gameObject.transform.localScale = normalScale;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        GameObject managers = GameObject.FindGameObjectWithTag("Manager");

        UIManager uiManager = managers.GetComponent<UIManager>();

        uiManager.OnCardSelected(gameObject);
    }


}
