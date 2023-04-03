using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{

    private const int NUM_HOME_SPACES = 4;
    private const int PLAYER_ONE_HOME = 100;
    private const int PLAYER_TWO_HOME = 200;
    private const int PLAYER_THREE_HOME = 300;
    private const int PLAYER_FOUR_HOME = 400;

    public Color color_p1 = Color.red;
    public Color color_p2 = Color.green;
    public Color color_p3 = Color.blue;
    public Color color_p4 = Color.yellow;


    public GameObject[] spaces;
    private SpaceManager[] spaceManagers;

    // Start is called before the first frame update
    void Start()
    {
        spaceManagers = new SpaceManager[spaces.Length];

        for (int i = 0; i < spaces.Length; i++)
        {
            SpaceManager spaceManager = spaces[i].GetComponent<SpaceManager>();
            spaceManagers[i] = spaceManager;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void UpdateBoard()
    {
        UpdateSpacesColours();
    }

    // this method resets the board to the starting configuration
    public void ResetBoard()
    {
        for (int i = 0; i < spaceManagers.Length; i++)
        {
            SpaceManager spaceManager = spaceManagers[i];
            spaceManager.controlledByPlayer = 0;

        }

        AssignPlayerToSpaces(1, PLAYER_ONE_HOME, PLAYER_ONE_HOME + NUM_HOME_SPACES);
        AssignPlayerToSpaces(2, PLAYER_TWO_HOME, PLAYER_TWO_HOME + NUM_HOME_SPACES);
        AssignPlayerToSpaces(3, PLAYER_THREE_HOME, PLAYER_THREE_HOME + NUM_HOME_SPACES);
        AssignPlayerToSpaces(4, PLAYER_FOUR_HOME, PLAYER_FOUR_HOME + NUM_HOME_SPACES);

        //for testing
        AssignPlayerToSpace(1, 0);

    }

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
            SpaceManager spaceManager = spaceManagers[i];
            if (spaceManager.locationID == locationID)
            {
                spaceManager.controlledByPlayer = player;
            }
        }
    }

    public void UpdateSpacesColours()
    {
        for (int i = 0; i < spaces.Length; i++)
        {
            SpaceManager spaceManager = spaceManagers[i];
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

    public bool HasMarbleInStartRow(int player)
    {
        return NumberOfMarblesInStartRow(player) > 0;
    }

    public int NumberOfMarblesInStartRow(int player)
    {
        int count = 0;

        for (int i = 0; i < spaceManagers.Length; i++)
        {
            SpaceManager spaceManager = spaceManagers[i];

            if (spaceManager.isInStartRow && spaceManager.controlledByPlayer == player)
            {
                count++;
            }
        }


        return count;
    }
}
