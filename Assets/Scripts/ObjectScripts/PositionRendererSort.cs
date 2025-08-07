using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
 *                  -POSITION RENDERER SORT-
 *                  
 *    Position renderer sort goes in each object that
 *    requires it's sorting order to be adjusted depending
 *    on its shadow's sorting order.
 *    
 *    REQUIREMENTS:
 *    -A parent object holding a shadow object. (This would be the base object)
 *   
 */

public class PositionRendererSort : MonoBehaviour
{
    [SerializeField]
    private int SortingOrderBase;
    //"offset" fixes the 'origin' of the player object.
    [SerializeField]
    private int offset;
    private Renderer myRenderer;

    //Sliding Offset Code
    [SerializeField]
    bool slidingOffset;
    [SerializeField]
    float minOffset;
    [SerializeField]
    float maxOffset;
    GameObject playerLegs;

    private float height;
    [SerializeField]
    private Renderer shadowRenderer;
    //mainObj is the object with the ObjectProperties.cs file.
    //[SerializeField]
    //private GameObject mainObj;

    [SerializeField]
    Transform parent;

    //A simple object is an object that doesn't have a shadow. The sorting order is determined by itself, not its shadow (e.g front of gazebo)
    public bool simpleObject;


    private void Awake()
    {
        myRenderer = gameObject.GetComponent<Renderer>();
    }

    // Start is called before the first frame update
    void Start()
    {
        playerLegs = GameObject.Find("Player").transform.Find("Body").Find("LegsParent").Find("Legs").gameObject;
     
        shadowRenderer = null;

        if (!simpleObject)
        {
            //Find the Shadow child object of the current object.
            if (this.tag == "bodypart")   //Shadow is in (BodyPart->Player->PlayerParent)
            {
                parent = transform.parent.parent;
            } else if (this.tag == "bodypart(arm)") //Shadow is in (Arm->Arms->Player->PlayerParent)
            {
                parent = transform.parent.parent.parent;
            } else  //Normal object. (Not a person, so shadow is in Object->Parent)
            {
                parent = transform.parent;
            }
            if (parent != null)
            {
                for (int i = 0; i < parent.childCount; i++)
                {
                    Transform child = parent.GetChild(i);
                    if (child.tag == "Shadow")
                    {
                        shadowRenderer = parent.GetChild(i).gameObject.GetComponent<Renderer>();
                        break;
                    }
                }

            }
        }

    }



    void LateUpdate()
    {
        if (simpleObject)
        {
            if (slidingOffset)
            {
                myRenderer.sortingOrder = playerLegs.GetComponent<Renderer>().sortingOrder - offset;
                //Debug.Log("Offset: " + offset);
                myRenderer.sortingOrder = Mathf.Clamp(myRenderer.sortingOrder, (int)minOffset - offset, (int)maxOffset - offset);
                //offset = -(int)playerTrans.localPosition.y;
                //offset = Mathf.Clamp(offset, (int)minOffset, (int)maxOffset);
            } else
            {
                myRenderer.sortingOrder = -(int)transform.position.y - offset;
            }
        } else
        {
            //Set this object's sorting order depending on the shadow's sorting order.
            if (shadowRenderer != null)
            {
                myRenderer.sortingOrder = shadowRenderer.sortingOrder;
            }
        }

        if (this.tag == "bodypart")
        {
            myRenderer.sortingOrder = myRenderer.sortingOrder + 1;
        }


    }
}
