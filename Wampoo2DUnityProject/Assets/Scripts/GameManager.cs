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
                bool validMove = HandleHomeRow(s, c, player);

                if (!validMove)
                {
                    InvalidMove(s, c);
                }
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
            HandleKillerCard(s, c, player);
        }
        else
        {
            InvalidMove(s, c);
            return;
        }

        EndTurn();

    }

    private void HandleKillerCard(SpaceManager s, Card c, int player)
    {
        int start = s.locationID;
        int finish = c.spaces + start;

        // edge cases
        if (EnteringHomeRow(s, c, player))
        {

            if (CanEnterHomeRow(s,c,player))
            {
                // we still need to remove the marbles between current space and the players end location
                for (int i = start; i <= board.GetLocationIdOfEndPosition(player); i++)
                {

                    SendPlayerAtSpaceLocationIDBackToStartRow(i);

                }
                HandleNonSpecialMovement(s, c, player);
                return;
            }
            else
            {
                InvalidMove(s, c);
                return;
            }
        }
        else if (finish < start)
        {
            // player is passing by the last space before the location IDs start over

            for (int i = start; i <= Board.LOCATION_ID_OF_LAST_SPACE; i++)
            {
                SendPlayerAtSpaceLocationIDBackToStartRow(i);
            }

        }
        else
        {
            for (int i = start; i <= finish; i++)
            {
                SendPlayerAtSpaceLocationIDBackToStartRow(i);
            }
        }

        //make move
        AssignPlayerToSpace(s, c, player);

    }

    private void SendPlayerAtSpaceLocationIDBackToStartRow(int locationID)
    {
        SpaceManager space = board.GetSpaceAtLocationID(locationID);
        int playerToRemove = space.controlledByPlayer;

        if (playerToRemove > 0)
        {
            SendPlayerBackToStartRow(playerToRemove);

        }
        space.controlledByPlayer = NO_ONE;

    }

    // this is handling moving inside the home row
    // this is not handling getting into the home row
    private bool HandleHomeRow(SpaceManager s, Card c, int player)
    {

        //need to consider if there is a marble already in the spot in the home row
        if (CanMakeMoveInHomeRow(s.locationID, c.spaces, player))
        {
            AssignPlayerToSpace(s, c, player);

            deck.DiscardCard(c, user.player);
            return true;

        }

        return false;

    }

    // need to look at this because I have a similar method in the board class
    public void AssignPlayerToSpace(SpaceManager s, Card c, int player)
    {
        int newLocationID = s.locationID + c.spaces;

        SpaceManager newSpace = board.GetSpaceAtLocationID(newLocationID);

        newSpace.controlledByPlayer = player;
        s.controlledByPlayer = 0;

    }

    // checking if move inside home row is possible starting from home location
    public bool CanMakeMoveInHomeRow(int locationID, int spaces, int player)
    {

        int newLocationID = locationID + spaces;

        SpaceManager newSpace = board.GetSpaceAtLocationID(newLocationID);

        if (newSpace == null)
        {
            return false;
        }

        if (newSpace.IsControlledByPlayer(player))
        {
            return false;
        }

        if (!newSpace.isInHomeRow)
        {
            return false;
        }

        //need to make sure we aren't jumping any marbles
        for (int i = locationID + 1; i < newLocationID; i++)
        {
            SpaceManager space = board.GetSpaceAtLocationID(i);

            if (space.controlledByPlayer == player)
            {
                return false;
            }
        }

        return true;
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

    public bool CanMakeMoveInHomeRow(SpaceManager s, Card c, int player)
    {
        int currentLocationId = s.locationID;
        int newSpaceLocationId = currentLocationId + c.spaces;

        if (EnteringHomeRow(s, c, player))
        {
            int endLocationId = board.GetLocationIdOfEndPosition(player);

            // subtract 1 because home row is 0 indexed
            int spacesToMove = newSpaceLocationId - endLocationId - 1;

            int homeLocation = board.GetLocationIdOfHomePosition(player);

            if (!CanMakeMoveInHomeRow(homeLocation, spacesToMove, player))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        return false;
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

        int currentLocationId = s.locationID;
        int newSpaceLocationId = currentLocationId + c.spaces;
        int endLocationId = board.GetLocationIdOfEndPosition(player);
        int spacesToMove = newSpaceLocationId - endLocationId - 1;

        if (EnteringHomeRow(s, c, player))
        {

            if (CanEnterHomeRow(s,c,player))
            {
                newSpaceLocationId = board.GetLocationIdOfHomePosition(player) + spacesToMove;
            }
            else
            {
                return;
            }

        }
        else if (IsMoveBlockedByStartingMarble(s, c, player))
        {
            return;
        }

        //set it back to no control because we are moving out of it
        s.controlledByPlayer = NO_ONE;

        SendPlayerAtSpaceLocationIDBackToStartRow(newSpaceLocationId);

        //TODO: update for AI players
        board.AssignPlayerToSpace(player, newSpaceLocationId);

        deck.DiscardCard(c, user.player);

        EndTurn();
    }

    private bool CanEnterHomeRow(SpaceManager s, Card c, int player)
    {
        int homeLocation = board.GetLocationIdOfHomePosition(player);

        SpaceManager homeStartSpace = board.GetSpaceAtLocationID(homeLocation);

        if (homeStartSpace.IsControlledByPlayer(player))
        {
            return false;
        }

        return CanMakeMoveInHomeRow(s, c, player);
    }

    private bool IsMoveBlockedByStartingMarble(SpaceManager s, Card c, int player)
    {

        //loop through player 1 to 4
        for (int i = 1; i <= 4; i++)
        {
            //for every other player
            if (i != player)
            {
                //check if this player is blocking our player
                if (board.HasMarbleInStartPosition(i))
                {
                    // has a marble so could possibly be blocking
                    int start = s.locationID;
                    int finish = s.locationID + c.spaces;

                    //check for player #1 edge case
                    if (finish < start)
                    {
                        //passing past #1
                        if (board.HasMarbleInStartPosition(1))
                        {
                            // blocked by marble in player 1 start position
                            return true;
                        }
                    }

                    int startMarbleIndex = board.GetLocationIdOfStartPosition(i);

                    if (start < startMarbleIndex && startMarbleIndex < finish)
                    {
                        return true;
                    }
                }
            }
        }

        return false;
    }

    private bool EnteringHomeRow(SpaceManager s, Card c, int player)
    {
        int endLocationId = board.GetLocationIdOfEndPosition(player);

        if (s.locationID + c.spaces > endLocationId)
        {
            return true;
        }

        return false;

    }

    // assumes the space that the player was on will be reset to the new owner outside of this method
    private void SendPlayerBackToStartRow(int player)
    {
        int startRow = board.GetLocationIdOfStartRow(player);

        for (int i = startRow; i < startRow + 4; i++)
        {
            SpaceManager space = board.GetSpaceAtLocationID(i);

            //find the first empty space and fill it
            if (space.controlledByPlayer != player)
            {
                space.controlledByPlayer = player;
                return;
            }

        }

    }

    public void EndTurn()
    {

    }
}
