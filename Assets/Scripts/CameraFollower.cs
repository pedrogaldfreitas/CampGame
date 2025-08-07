using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollower : MonoBehaviour
{
    [SerializeField]
    private float xMaxVal;
    [SerializeField]
    private float yMaxVal;
    [SerializeField]
    private float xMinVal;
    [SerializeField]
    private float yMinVal;

    [SerializeField]
    private Transform target;

    // Start is called before the first frame update
    void Start()
    {
        //target = GameObject.Find("Player").transform;
    }

    void LateUpdate()
    {
        //target = GameObject.Find("Player").transform;
        transform.position = new Vector3(Mathf.Clamp(target.position.x, xMinVal, xMaxVal), Mathf.Clamp(target.position.y, yMinVal, yMaxVal), transform.position.z);
    }
}
