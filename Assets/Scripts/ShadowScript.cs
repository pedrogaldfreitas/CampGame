using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowScript : MonoBehaviour
{
    private GameObject parentObject;
    private SpriteRenderer shadowSprite;
    public float height;
    private float yval;

    // Start is called before the first frame update
    void Start()
    {
        parentObject = transform.parent.gameObject;
        shadowSprite = GetComponent<SpriteRenderer>();
        //height = parentObject.GetComponent<ObjectProperties>().height;
    }

    // Update is called once per frame
    private void Update()
    {
        //QUESTIONABLE: Since shadows now have their own layer, do we need this line?
        //GetComponent<Renderer>().sortingOrder = parentObject.GetComponent<Renderer>().sortingOrder - 2;
    }

    void LateUpdate()
    {
        //yval = parentObject.GetComponent<ObjectProperties>().yval;

        if (height <= 0)
        {
            shadowSprite.enabled = false;
        } else
        {
            shadowSprite.enabled = true;
        }

        //height = parentObject.GetComponent<ObjectProperties>().height;
        transform.position = new Vector2(parentObject.transform.position.x, yval);
    }
}
