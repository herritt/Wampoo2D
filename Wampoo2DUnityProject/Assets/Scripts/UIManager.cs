using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class UIManager : MonoBehaviour
{
    private float popUpTime = 4f;
    private GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = gameObject.GetComponent<GameManager>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowText(string message)
    {
        StartCoroutine(ShowMessage(message, popUpTime));

    }

    public void TellUserItsTheirTurn(int player)
    {
        ShowText("Your Turn!!");
    }

    IEnumerator ShowMessage(string message, float delay)
    {
        TMP_Text text = GameObject.FindGameObjectWithTag("TextPopUp").GetComponent<TMP_Text>();

        text.text = message;
        text.enabled = true;

        yield return new WaitForSeconds(delay);
        text.enabled = false;
    }

    public void OnCardSelected(GameObject cardObj)
    {
        Card card = cardObj.GetComponent<Card>();

        List<Card> playersHand = gameManager.user.player.playersHand;

        if (playersHand.Contains(card))
        {
            foreach (Card c in playersHand)
            {
                c.UnSelect();
            }

            card.Selected();
        }

        TryMove();
    }

    public void OnMarbleSelected(GameObject marbleObject)
    {
        foreach (SpaceManager s in gameManager.spaces)
        {
            Debug.Log(s);
            s.UnSelect();
        }
    

        SpaceManager space = marbleObject.GetComponent<SpaceManager>();

        if (space.IsControlledByUser())
        {
            space.Selected();
        }

        TryMove();
    }

    public void TryMove()
    {
        foreach (SpaceManager s in gameManager.spaces)
        {
            if (s.isSelected)
            {
                List<Card> playersHand = gameManager.user.player.playersHand;

                foreach (Card c in playersHand)
                {
                    if (c.isSelected)
                    {
                        gameManager.PlayMove(s, c);
                        c.UnSelect();
                        s.UnSelect();
                        return;
                    }
                }
                
            }
        }

        
    }
}
