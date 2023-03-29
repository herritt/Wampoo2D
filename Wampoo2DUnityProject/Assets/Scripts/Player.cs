using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player
{
    public string playerName;
    public int playerNumber;
    public Color playerColour;
    public bool isUser = false;

    public List<Card> playersHand = new List<Card>();

    public Player(string playerName, int playerNumber, Color playerColour, bool isUser)
    {
        this.playerName = playerName;
        this.playerNumber = playerNumber;
        this.playerColour = playerColour;
        this.isUser = isUser;
    }

    public Player(string playerName, int playerNumber, Color playerColour)
    {
        this.playerName = playerName;
        this.playerNumber = playerNumber;
        this.playerColour = playerColour;
    }

    public void AddCardToPlayersHand(Card card)
    {
        playersHand.Add(card);
    }

    public void RemoveCardFromPlayersHand(Card card)
    {
        playersHand.Remove(card);
    }

    public void OutPutHandToConsole()
    {
        foreach  (Card card in playersHand)
        {
            Debug.Log(card);
        }
    }

}
