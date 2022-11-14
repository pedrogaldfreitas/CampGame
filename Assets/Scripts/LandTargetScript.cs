using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandTargetScript : MonoBehaviour
{
    private newShadowScript shadowScript;

    private void Start()
    {
        shadowScript = transform.parent.Find("Shadow").GetComponent<newShadowScript>();
    }

    private void Update()
    {
        this.transform.position = new Vector2(transform.position.x, shadowScript.transform.position.y - shadowScript.floorHeight);
    }

}