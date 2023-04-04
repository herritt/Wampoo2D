using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    private float timer = 0;
    private float checkTime = 0.25f;
    public const int NO_ONE = 0;

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
                InvalidMove(s, c);
                return;
            }
        }
        else if (s.isInHomeRow)
        {
            if (c.spaces > 3)
            {
                InvalidMove(s, c);
                return;
            }
            else
            {
                HandleHomeRow(s, c, player);
            }
        }
        else if (c.isJack)
        {
            //TODO - maybe give option to select another marble to swap or to move, and a button to cancel - need to handle selecting start row, home row, same colour, etc.
        }
        else if (c.isFour)
        {
            HandleFourCard(s, c, player);
        }
        else if (c.isKillerCard)
        {
            //TODO after getting other players moving
        }
        else
        {
            InvalidMove(s, c);
            return;
        }

        EndTurn();

    }

    // this is handling moving inside the home row
    // this is not handling getting into the home row
    private void HandleHomeRow(SpaceManager s, Card c, int player)
    {
        //need to consider if there is a marble already in the spot in the home row
        int newLocationID = s.locationID + c.spaces;

        SpaceManager newSpace = board.GetSpaceAtLocationID(newLocationID);

        if (newSpace == null)
        {
            InvalidMove(s,c);
            return;
        }

        if (newSpace.IsControlledByPlayer(player))
        {
            InvalidMove(s, c);
            return;
        }

        if (!newSpace.isInHomeRow)
        {
            InvalidMove(s, c);
            return;
        }

        //need to make sure we aren't jumping any marbles
        for (int i = s.locationID + 1; i < newLocationID; i++)
        {
            SpaceManager space = board.GetSpaceAtLocationID(i);

            if (space.controlledByPlayer == player)
            {
                InvalidMove(s, c);
                return;
            }
        }

        //should be good to make the move
        newSpace.controlledByPlayer = player;
        s.controlledByPlayer = 0;

        deck.DiscardCard(c, user.player);

        EndTurn();

    }

    private void InvalidMove(SpaceManager s, Card c)
    {
        s.UnSelect();
        c.UnSelect();
    }

    private void HandleStarterRow(SpaceManager s, Card c, int player)
    {
        if (board.HasMarbleInStartPosition(player))
        {
            InvalidMove(s, c);
        }
        else
        {
            s.controlledByPlayer = 0;
            deck.DiscardCard(c, user.player);
            SpaceManager space = board.GetStartSpaceForPlayer(player);
            space.controlledByPlayer = player;

            deck.DiscardCard(c, user.player);


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

        deck.DiscardCard(c, user.player);


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

        EndTurn();
    }



    public void EndTurn()
    {

    }
}
