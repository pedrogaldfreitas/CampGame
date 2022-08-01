using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*  THIS SCRIPT WILL:
    -Check collision between dialogue point and dialogue option area
    -Move the point's 'focus'
    -Move the point gradually towards focus
*/
public class diagSelectorScript : MonoBehaviour
{
    [SerializeField]
    private int focus;
    public int highlightedChoice = 1;

    public GameObject Option1Text;
    public GameObject Option2Text;
    public GameObject Option3Text;
    public GameObject Option4Text;

    private void Start()
    {
        focus = 1;
    }

    void Update()
    {

        switch (focus)
        {
            case 1:
                if (this.transform.position != Option1Text.transform.position)
                {
                    transform.position = Vector2.MoveTowards(this.transform.position, Option1Text.transform.position, 175f * Time.deltaTime);
                }
                break;
            case 2:
                if (this.transform.position != Option2Text.transform.position)
                {
                    transform.position = Vector2.MoveTowards(this.transform.position, Option2Text.transform.position, 175f*Time.deltaTime);
                }
                break;
            case 3:
                if (this.transform.position != Option3Text.transform.position)
                {
                    transform.position = Vector2.MoveTowards(this.transform.position, Option3Text.transform.position, 175f * Time.deltaTime);
                }
                break;
            case 4:
                if (this.transform.position != Option4Text.transform.position)
                {
                    transform.position = Vector2.MoveTowards(this.transform.position, Option4Text.transform.position, 175f * Time.deltaTime);
                }
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        switch (collider.gameObject.name) {
            case "Option1Text":
                highlightedChoice = 1;
                break;
            case "Option2Text":
                highlightedChoice = 2;
                break;
            case "Option3Text":
                highlightedChoice = 3;
                break;
            case "Option4Text":
                highlightedChoice = 4;
                break;
            default:
                break;
        }
    }

    public void shiftFocus(int diagChoice)
    {
        focus = diagChoice;
        return;
    }
}
