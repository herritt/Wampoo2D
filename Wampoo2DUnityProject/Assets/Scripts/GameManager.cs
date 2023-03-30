using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    private float timer = 0;
    private float checkTime = 0.25f;

    public GameObject[] spaces;
    public Player[] players;
    private DeckOfCards deck;
    public GameObject boardObj;
    private Board board;

    public User user;

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



    // Update is called once per frame
    void Update()
    {
        if (timer > checkTime)
        {
            timer = 0;
            board.UpdateBoard();
            user.UpdateUsersHand();

        }
        else
        {
            timer = timer + Time.deltaTime;
        }


    }

    private void CreatePlayers()
    {
        //hard coded for now

        players = new Player[4];

        players[0] = new Player("Bob", 1, board.color_p1, true);

        players[1] = new Player("Tom", 2, board.color_p2);
        players[2] = new Player("Eugene", 3, board.color_p3);
        players[3] = new Player("Alice", 4, board.color_p4);

        //hard code player to user assignment
        user.SetPlayer(players[0]);

    }

}
