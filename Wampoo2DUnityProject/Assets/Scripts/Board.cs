using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{

    private const int NUM_HOME_SPACES = 4;

    private const int PLAYER_ONE_STARTROW = 100;
    private const int PLAYER_TWO_STARTROW = 200;
    private const int PLAYER_THREE_STARTROW = 300;
    private const int PLAYER_FOUR_STARTROW = 400;

    private const int PLAYER_ONE_HOME_ROW = 1000;
    private const int PLAYER_TWO_HOME_ROW = 2000;
    private const int PLAYER_THREE_HOME_ROW = 3000;
    private const int PLAYER_FOUR_HOME_ROW = 4000;

    private const int PLAYER_ONE_START = 0;
    private const int PLAYER_TWO_START = 24;
    private const int PLAYER_THREE_START = 48;
    private const int PLAYER_FOUR_START = 72;

    private const int PLAYER_ONE_END = 92;
    private const int PLAYER_TWO_END = 20;
    private const int PLAYER_THREE_END = 44;
    private const int PLAYER_FOUR_END = 68;

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

        AssignPlayerToSpaces(1, PLAYER_ONE_STARTROW, PLAYER_ONE_STARTROW + NUM_HOME_SPACES);
        AssignPlayerToSpaces(2, PLAYER_TWO_STARTROW, PLAYER_TWO_STARTROW + NUM_HOME_SPACES);
        AssignPlayerToSpaces(3, PLAYER_THREE_STARTROW, PLAYER_THREE_STARTROW + NUM_HOME_SPACES);
        AssignPlayerToSpaces(4, PLAYER_FOUR_STARTROW, PLAYER_FOUR_STARTROW + NUM_HOME_SPACES);

        //for testing
        AssignPlayerToSpace(1, 90);
        AssignPlayerToSpace(1, 1000);

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

    public SpaceManager GetStartSpaceForPlayer(int player)
    {
        int locationID = GetLocationIdOfStartPosition(player);

        SpaceManager space = GetSpaceAtLocationID(locationID);
        
        return space;
    }
    public bool HasMarbleInStartPosition(int player)
    {
        SpaceManager space = GetStartSpaceForPlayer(player);

        if (space.controlledByPlayer == player)
        {
            return true;
        }

        return false;
    }

    public int GetLocationIdOfHomePosition(int player)
    {
        if (player == 1)
        {
            return PLAYER_ONE_HOME_ROW;
        }
        else if (player == 2)
        {
            return PLAYER_TWO_HOME_ROW;
        }
        else if (player == 3)
        {
            return PLAYER_THREE_HOME_ROW;
        }
        else if (player == 4)
        {
            return PLAYER_FOUR_HOME_ROW;
        }

        return -1;
    }

    public int GetLocationIdOfStartPosition(int player)
    {
        if (player == 1)
        {
            return PLAYER_ONE_START;
        }
        else if (player == 2)
        {
            return PLAYER_TWO_START;
        }
        else if (player == 3)
        {
            return PLAYER_THREE_START;
        }
        else if (player == 4)
        {
            return PLAYER_FOUR_START;
        }

        return -1;
    }

    public int GetLocationIdOfEndPosition(int player)
    {
        if (player == 1)
        {
            return PLAYER_ONE_END;
        }
        else if (player == 2)
        {
            return PLAYER_TWO_END;
        }
        else if (player == 3)
        {
            return PLAYER_THREE_END;
        }
        else if (player == 4)
        {
            return PLAYER_FOUR_END;
        }

        return -1;
    }

    public SpaceManager GetSpaceAtLocationID(int locationID)
    {
        for (int i = 0; i < spaces.Length; i++)
        {
            if (spaceManagers[i].locationID == locationID)
            {
                return spaceManagers[i];
            }
        }

        return null;
    }
}
