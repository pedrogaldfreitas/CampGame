using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bodyPartScript : MonoBehaviour
{
    private Transform parentTransform;
    private Renderer thisRenderer;
    private Renderer legsRenderer;

    private void Start()
    {
        thisRenderer = GetComponent<Renderer>();
        parentTransform = transform.parent;
        legsRenderer = GameObject.Find("Player").transform.Find("Body").Find("LegsParent").Find("Legs").gameObject.GetComponent<Renderer>();
        /*
        if (this.gameObject.tag == "bodypart")
        {
            legsRenderer = parentTransform.parent.Find("LegsParent").Find("Legs").gameObject.GetComponent<Renderer>();
        }
        if (this.gameObject.tag == "bodypart(arm)")
        {
            legsRenderer = parentTransform.parent.parent.Find("LegsParent").Find("Legs").gameObject.GetComponent<Renderer>();
        }*/
    }

    private void Update()
    {
        if (this.gameObject.tag == "bodypart" || this.gameObject.tag == "bodypart(arm)")
        {
            thisRenderer.sortingOrder = legsRenderer.sortingOrder + 1;
            //FIX AND USE THIS EVENTUALLY: thisRenderer.sortingOrder = legsRenderer.sortingOrder +1 + (int)parentTransform.Find("Shadow").GetComponent<newShadowScript>().floorHeight;
        }
    }
}
