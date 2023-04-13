using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class UIManager : MonoBehaviour
{
    private float popUpTime = 4f;
    private GameManager gameManager;
    private bool jackSelected = false;

    // used to store selected spaces for Jack swap
    public List<SpaceManager> selectedSpaces;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = gameObject.GetComponent<GameManager>();
        selectedSpaces = new List<SpaceManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowText(string message)
    {
        TMP_Text text = GameObject.FindGameObjectWithTag("TextPopUp").GetComponent<TMP_Text>();

        text.text = message;
        text.enabled = true;
    }

    public void ShowTextPopUp(string message)
    {
        StartCoroutine(ShowMessage(message, popUpTime));

    }

    public void TellUserItsTheirTurn(int player)
    {
        ShowTextPopUp("Your Turn!!");
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

            jackSelected = card.isJack;

            if (jackSelected && selectedSpaces.Count == 2)
            {
                gameManager.HandleJackSwap(selectedSpaces);

            }

        }

        TryMove();
    }

    public void OnMarbleSelected(GameObject marbleObject)
    {
        SpaceManager space = marbleObject.GetComponent<SpaceManager>();

        // we want to handle the Jack case
        if (jackSelected)
        {
            selectedSpaces.Add(space);

            space.Selected();

            if (selectedSpaces.Count == 2)
            {
                gameManager.HandleJackSwap(selectedSpaces);
                selectedSpaces.Clear();
            }

        }
        else
        {
            selectedSpaces.Clear();

            foreach (SpaceManager s in gameManager.spaces)
            {
                s.UnSelect();
            }

            if (space.IsControlledByUser())
            {
                space.Selected();
            }

            TryMove();
        }
    
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
                        gameManager.PlayMove(s, c, 1);
                        return;
                    }
                }
                
            }
        }

        
    }
}
