using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceManager : MonoBehaviour
{
    public int locationID;
    public Color defaultColour;
    public GameObject marbleSpriteObject;
    public int controlledByPlayer = 0;

    // Start is called before the first frame update
    void Start()
    {
        setColour(defaultColour);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [ContextMenu("setColor")]
    public void setColour(Color c)
    {
        c.a = 255;

        marbleSpriteObject.GetComponent<SpriteRenderer>().color = c;

    }

    public void ResetToDefaultColor()
    {
        marbleSpriteObject.GetComponent<SpriteRenderer>().color = defaultColour;

    }
}
