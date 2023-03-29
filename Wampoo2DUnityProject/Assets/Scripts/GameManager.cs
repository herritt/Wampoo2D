using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //for testing
    private float timer = 0;
    private float checkTime = 0.25f;
    private int index = 0;

    //real variables
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


    // Start is called before the first frame update
    void Start()
    {
        CreatePlayers();
        ResetBoard();
        deck = GameObject.FindGameObjectWithTag("Deck").GetComponent<DeckOfCards>();
        deck.ShuffleDeck();
        deck.Deal(5, players);

        players[0].OutPutHandToConsole();
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
            UpdateBoard();


        }
        else
        {
            timer = timer + Time.deltaTime;
        }

        
    }






    // this method resets the board to the starting configuration
    private void ResetBoard()
    {
        for (int i = 0; i < spaces.Length; i++)
        {
            SpaceManager spaceManager = spaces[i].GetComponent<SpaceManager>();
            spaceManager.controlledByPlayer = 0;

        }

        AssignPlayerToSpaces(1, PLAYER_ONE_HOME, PLAYER_ONE_HOME + NUM_HOME_SPACES);
        AssignPlayerToSpaces(2, PLAYER_TWO_HOME, PLAYER_TWO_HOME + NUM_HOME_SPACES);
        AssignPlayerToSpaces(3, PLAYER_THREE_HOME, PLAYER_THREE_HOME + NUM_HOME_SPACES);
        AssignPlayerToSpaces(4, PLAYER_FOUR_HOME, PLAYER_FOUR_HOME + NUM_HOME_SPACES);

    }

    // assigns the player to control the spaces in the range.
    public void AssignPlayerToSpaces(int player, int startID, int stopID)
    {
        if (stopID < startID) return;

        for (int i = startID; i <= stopID; i++)
        {
            AssignPlayerToSpace(player, i);
        }
    }

    // assigns the player to given space
    public void AssignPlayerToSpace(int player, int locationID)
    {
        for (int i = 0; i < spaces.Length; i++)
        {
            SpaceManager spaceManager = spaces[i].GetComponent<SpaceManager>();
            if (spaceManager.locationID == locationID)
            {
                spaceManager.controlledByPlayer = player;
            }
        }
    }

    // updates the board based on has a marble in each space
    private void UpdateBoard()
    {
        for (int i = 0; i < spaces.Length; i++)
        {
            SpaceManager spaceManager = spaces[i].GetComponent<SpaceManager>();
            int player = spaceManager.controlledByPlayer;

            if (player == 0)
            {
                spaceManager.ResetToDefaultColor();

            }
            else if (player == 1)
            {
                spaceManager.setColour(players[0].playerColour);
            }
            else if (player == 2)
            {
                spaceManager.setColour(players[1].playerColour);
            }
            else if (player == 3)
            {
                spaceManager.setColour(players[2].playerColour);
            }
            else if (player == 4)
            {
                spaceManager.setColour(players[3].playerColour);
            }

        }
    }

    // sets the space to the given colour
    private void ColourSpaceAtIndex(int index, Color c)
    {
        c.a = 255;
        for (int i = 0; i < spaces.Length; i++)
        {
            SpaceManager spaceManager = spaces[i].GetComponent<SpaceManager>();
            if (spaceManager.locationID == index)
            {
                spaceManager.setColour(c);
            }

        }
    }

    //tests

    // loops around the board assigning red to each space
    public void RunAroundTheBoardTest()
    {
        if (timer < checkTime)
        {
            timer = timer + Time.deltaTime;
        }
        else
        {
            timer = 0;
            ColourSpaceAtIndex(index, Color.red);
            index++;
            if (index > 95) index = 0;
        }
    }
}
