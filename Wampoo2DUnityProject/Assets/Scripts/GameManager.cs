using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    private float timer = 0;
    private float checkTime = 0.25f;

    public Color color_p1 = Color.red;
    public Color color_p2 = Color.green;
    public Color color_p3 = Color.blue;
    public Color color_p4 = Color.yellow;

    private const int NUM_HOME_SPACES = 4;
    private const int PLAYER_ONE_HOME = 100;
    private const int PLAYER_TWO_HOME = 200;
    private const int PLAYER_THREE_HOME = 300;
    private const int PLAYER_FOUR_HOME = 400;

    public GameObject[] spaces;
    public Player[] players;
    private DeckOfCards deck;
    public GameObject[] userHandLocations;
    public GameObject boardObj;
    private Board board;


    // Start is called before the first frame update
    void Start()
    {
        board = boardObj.GetComponent<Board>();
        CreatePlayers();
        deck = GameObject.FindGameObjectWithTag("Deck").GetComponent<DeckOfCards>();
        deck.ShuffleDeck();
        deck.Deal(5, players);

        board.ResetBoard();
    }

    private void CreatePlayers()
    {
        //hard coded for now

        players = new Player[4];

        players[0] = new Player("Bob", 1, color_p1, true);
        players[1] = new Player("Tom", 2, color_p2);
        players[2] = new Player("Eugene", 3, color_p3);
        players[3] = new Player("Alice", 4, color_p4);

        

    }

    // Update is called once per frame
    void Update()
    {
        if (timer > checkTime)
        {
            timer = 0;
            board.UpdateBoard();
            UpdateUsersHand();


        }
        else
        {
            timer = timer + Time.deltaTime;
        }

        
    }



    public void UpdateUsersHand()
    {
        Player player = GetUser();

        if (player != null)
        {
            List<Card> cards = player.playersHand;

            int count = 0;
            while (count < cards.Count)
            {
                
                cards[count].gameObject.transform.position = userHandLocations[count].gameObject.transform.position;

                

                cards[count].faceUp();
                count++;
            }
        }
        
    }

    public Player GetUser()
    {
        foreach (Player p in players)
        {
            if (p.isUser)
            {
                return p;
            }
        }

        Debug.Log("User Not Found!");

        return null;
    }


}
