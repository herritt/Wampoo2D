using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceManager : MonoBehaviour
{
    public string locationID;
    public Color spaceColour;
    public GameObject marbleSpriteObject;

    // Start is called before the first frame update
    void Start()
    {
        setColour();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [ContextMenu("setColor")]
    public void setColour()
    {
        Color c = spaceColour;
        c.a = 255;

        marbleSpriteObject.GetComponent<SpriteRenderer>().color = c;

    }
}
