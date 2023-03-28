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


    // Start is called before the first frame update
    void Start()
    {
        ResetBoard();
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
                spaceManager.setColour(color_p1);
            }
            else if (player == 2)
            {
                spaceManager.setColour(color_p2);
            }
            else if (player == 3)
            {
                spaceManager.setColour(color_p3);
            }
            else if (player == 4)
            {
                spaceManager.setColour(color_p4);
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
