using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    public const int NO_ONE = 0;
    public const int NUM_HOME_SPACES = 4;

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

    public const int LOCATION_ID_OF_LAST_SPACE = 95;

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

        AssignPlayerToSpaces(1, PLAYER_ONE_STARTROW, PLAYER_ONE_STARTROW + NUM_HOME_SPACES -1);
        AssignPlayerToSpaces(2, PLAYER_TWO_STARTROW, PLAYER_TWO_STARTROW + NUM_HOME_SPACES -1);
        AssignPlayerToSpaces(3, PLAYER_THREE_STARTROW, PLAYER_THREE_STARTROW + NUM_HOME_SPACES -1);
        AssignPlayerToSpaces(4, PLAYER_FOUR_STARTROW, PLAYER_FOUR_STARTROW + NUM_HOME_SPACES -1);

        //for testing
        AssignPlayerToSpace(1, 17);
        AssignPlayerToSpace(2, 19);
        AssignPlayerToSpace(2, 22);
 

        AssignPlayerToSpace(1, 1000);

        AssignPlayerToSpace(0, 102);
        AssignPlayerToSpace(0, 100);

        AssignPlayerToSpace(0, 202);
        AssignPlayerToSpace(0, 201);
        AssignPlayerToSpace(0, 200);

        AssignPlayerToSpace(1, 0);

        AssignPlayerToSpace(2, 92);
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
    // if someone is already there, it sends them back to 
    // the home row
    public void AssignPlayerToSpace(int player, int locationID)
    {
        SpaceManager s = GetSpaceAtLocationID(locationID);

        if (s == null)
        {
            Debug.Log("was null " + locationID);
            return;
        }
        int currentPlayer = s.controlledByPlayer;

        if (currentPlayer != NO_ONE)
        {
            SendPlayerBackToStartRow(currentPlayer);

        }

        s.controlledByPlayer = player;

    }

    public void SendPlayerAtSpaceLocationIDBackToStartRow(int locationID)
    {
        SpaceManager space = GetSpaceAtLocationID(locationID);
        int playerToRemove = space.controlledByPlayer;

        if (playerToRemove > 0)
        {
            SendPlayerBackToStartRow(playerToRemove);

        }
        space.controlledByPlayer = NO_ONE;

    }

    // assumes the space that the player was on will be reset to the new owner outside of this method
    private void SendPlayerBackToStartRow(int player)
    {
        int startRow = GetLocationIdOfStartRow(player);

        for (int i = startRow; i < startRow + 4; i++)
        {
            SpaceManager space = GetSpaceAtLocationID(i);

            //find the first empty space and fill it
            if (space.controlledByPlayer != player)
            {
                space.controlledByPlayer = player;
                return;
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

    public int GetLocationIdOfStartRow(int player)
    {
        if (player == 1)
        {
            return PLAYER_ONE_STARTROW;
        }
        else if (player == 2)
        {
            return PLAYER_TWO_STARTROW;
        }
        else if (player == 3)
        {
            return PLAYER_THREE_STARTROW;
        }
        else if (player == 4)
        {
            return PLAYER_FOUR_STARTROW;
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
