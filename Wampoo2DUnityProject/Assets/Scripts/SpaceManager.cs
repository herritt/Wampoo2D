using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SpaceManager : MonoBehaviour,IPointerClickHandler
{
    public int locationID;
    public Color defaultColour;
    public GameObject marbleSpriteObject;
    public int controlledByPlayer = 0;
    public bool isInStartRow = false;
    public bool isSelected = false;

    private Vector3 selectedScale = new Vector3(1.2f, 1.2f, 1.2f);
    private Vector3 normalScale;

    // Start is called before the first frame update
    void Start()
    {
        setColour(defaultColour);
        normalScale = gameObject.transform.localScale;
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

    public void OnPointerClick(PointerEventData eventData)
    {
        GameObject managers = GameObject.FindGameObjectWithTag("Manager");

        UIManager uiManager = managers.GetComponent<UIManager>();

        uiManager.OnMarbleSelected(gameObject);

    }

    public void Selected()
    {
        isSelected = true;
        gameObject.transform.localScale = selectedScale;
    }

    public void UnSelect()
    {
        isSelected = false;
        gameObject.transform.localScale = normalScale;
    }

    public bool IsControlledByUser()
    {
        return IsControlledByPlayer(1);
    }

    public bool IsControlledByPlayer(int playerNumber)
    {
        if (controlledByPlayer == playerNumber)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
