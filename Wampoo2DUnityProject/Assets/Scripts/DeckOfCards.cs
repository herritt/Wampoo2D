using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckOfCards : MonoBehaviour
{
    public List<GameObject> cards;
    public Sprite backOfDeck;
    private Vector3 discardLocation;

    // Start is called before the first frame update
    void Start()
    {
        discardLocation = GameObject.FindGameObjectWithTag("DiscardLocation").transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShuffleDeck()
    {
        for (int i = 0; i < cards.Count - 1; i++)
        {
            int rnd = Random.Range(i, cards.Count);
            GameObject obj = cards[rnd];
            cards[rnd] = cards[i];
            cards[i] = obj;
        }
    }

    public void ShowTopCard()
    {
        GameObject topCard = cards[0];

        Vector3 orginalPosition = topCard.transform.position;

        topCard.transform.position = new Vector3(orginalPosition.x + 10, orginalPosition.y, orginalPosition.z);

        topCard.gameObject.GetComponent<Card>().flip();

    }

    public void Deal(int numCardsEach, Player[] players)
    {
        for (int i = 0; i < numCardsEach; i++)
        {
            //lets start by just sending some cards to
            //the human player
            SendCardsToPlayersHand(players[0], cards[i]);
        }
    }

    public void SendCardsToPlayersHand(Player player, GameObject card)
    {
        GameObject topCardObject = cards[0];
        cards.RemoveAt(0);
        player.playersHand.Add(topCardObject.GetComponent<Card>());
    }

    public void ForfeitHand(Player player)
    {

        foreach (Card card in player.playersHand)
        {
            Debug.Log(discardLocation);
            card.gameObject.transform.position = discardLocation;
        }
        
        player.playersHand.Clear();
    }

    public void DiscardCard(Card card, Player player)
    {
        player.RemoveCardFromPlayersHand(card);
        card.gameObject.transform.position = discardLocation;
        card.isSelected = false;
    }
}
