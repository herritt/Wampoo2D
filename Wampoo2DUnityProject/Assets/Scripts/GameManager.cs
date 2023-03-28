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

    public GameObject[] spaces;


    // Start is called before the first frame update
    void Start()
    {
        
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

    private void ColourSpaceAtIndex(int index)
    {
        for (int i = 0; i < spaces.Length; i++)
        {
            SpaceManager spaceManager = spaces[i].GetComponent<SpaceManager>();
            if (spaceManager.locationID == index)
            {
                spaceManager.setColour(new Color(1,0,0,0));
            }

        }
    }














    //tests

    public void RunAroundTheBoardTest()
    {
        if (timer < checkTime)
        {
            timer = timer + Time.deltaTime;
        }
        else
        {
            timer = 0;
            ColourSpaceAtIndex(index);
            index++;
            if (index > 95) index = 0;
        }
    }
}
