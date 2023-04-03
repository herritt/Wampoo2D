using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class User : MonoBehaviour
{
    public Player player;
    public GameObject[] userHandLocations;

    public void Update()
    {
        

    }

    public void SetPlayer(Player player)
    {
        this.player = player;
    }

    public void UpdateUsersHand()
    {
        if (player != null)
        {
            List<Card> cards = player.playersHand;

            int count = 0;
            while (count < cards.Count)
            {

                cards[count].gameObject.transform.position = userHandLocations[count].gameObject.transform.position;



                cards[count].faceUp();
                count++;
            }
        }

    }

    public void HandleUserTurn(GameObject cardObj, GameObject spaceObj)
    {

    }

}
