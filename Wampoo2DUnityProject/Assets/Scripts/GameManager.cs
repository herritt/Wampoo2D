using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    private float timer = 0;
    private float checkTime = 0.25f;

    public GameObject[] spaceObjs;
    public SpaceManager[] spaces;
    public Player[] players;
    private DeckOfCards deck;
    public GameObject boardObj;
    private Board board;

    public User user;
    private UIManager hud;

    // Start is called before the first frame update
    void Start()
    {
        spaces = new SpaceManager[spaceObjs.Length];
        for (int i = 0; i < spaceObjs.Length; i++)
        {
            spaces[i] = spaceObjs[i].GetComponent<SpaceManager>();
        }

        board = boardObj.GetComponent<Board>();
        hud = gameObject.GetComponent<UIManager>();

        CreatePlayers();
        deck = GameObject.FindGameObjectWithTag("Deck").GetComponent<DeckOfCards>();
        deck.ShuffleDeck();
        deck.Deal(5, players);
        board.ResetBoard();


        // hard coded first player for now
        hud.TellUserItsTheirTurn(0);

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

    public void OnForfeitHand()
    {
        deck.ForfeitHand(user.player);
        deck.Deal(4, players);
    }


    public void PlayMove(SpaceManager s, Card c, int player)
    {
        //check to see if it is just a straight up movement
        if (!s.isInStartRow && !s.isInHomeRow && !c.isSpecialCard)
        {
            HandleNonSpecialMovement(s, c, player);
        }
        else if (s.isInStartRow)
        {
            if (c.isStarterCard)
            {
                HandleStarterRow(s, c, player);
            }
            else
            {
                InValidMove(s, c);
                return;
            }
        }
        else if (s.isInHomeRow)
        {
            //TODO
        }
        else if (c.isFour)
        {
            HandleFourCard(s, c, player);
        }
        else
        {
            InValidMove(s, c);
            return;
        }

        EndTurn();

    }

    private void InValidMove(SpaceManager s, Card c)
    {
        s.UnSelect();
        c.UnSelect();
    }

    private void HandleStarterRow(SpaceManager s, Card c, int player)
    {
        if (board.HasMarbleInStartPosition(player))
        {
            InValidMove(s, c);
        }
        else
        {
            s.controlledByPlayer = 0;
            deck.DiscardCard(c, user.player);
            SpaceManager space = board.GetStartSpaceForPlayer(player);
            space.controlledByPlayer = player;
            
        }
    }

    private void HandleFourCard(SpaceManager s, Card c, int player)
    {
        s.controlledByPlayer = 0;

        int currentLocationId = s.locationID;
        int newSpaceLocationId = currentLocationId - 4;
        //TODO: Handle moving backwards from 0 position

        //TODO: handle knocking other player off

        board.AssignPlayerToSpace(player, newSpaceLocationId);



    }



    // space is a normal space not in the home row or in the starter row
    // card is just a movement 
    private void HandleNonSpecialMovement(SpaceManager s, Card c, int player)
    {
        //set it back to no control because we are moving out of it
        s.controlledByPlayer = 0;

        int currentLocationId = s.locationID;
        int newSpaceLocationId = currentLocationId + c.spaces;



        //TODO: handle getting to home row
        //TODO: handle knocking other player off
        //TODO: handle blockers (start marbles)
        //TODO: update for AI players
        board.AssignPlayerToSpace(player, newSpaceLocationId);


        deck.DiscardCard(c, user.player);
    }



    public void EndTurn()
    {

    }
}
