using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class newShadowScript : MonoBehaviour
{
    [Range(0f, 3f)] public float raycastDistanceMultiplier;
    public bool checkingForEdge = true;
    public float floorHeight;
    public GameObject edge;

    //Slope Variables
    private float prevXVal;
    private float prevYVal;
    public bool onHorizontalSlope;
    private bool wasPrevOnHorizontalSlope;
    public bool onVerticalSlope;
    private bool wasPrevOnVerticalSlope;
    float slopeFH;

    private float totalAmountRisenOrSunk;

    private Renderer thisRenderer;

    public GameObject parentObj;
    public GameObject mainObj;
    public Transform landTarget;

    [SerializeField]
    int offset;

    public float mult;

    //TEMPORARY FOR TESTING:
    public float horizSlopeCheckRayXStart;
    public float horizSlopeCheckRayYStart;
    public float horizSlopeCheckRayLength;
    public float vertiSlopeCheckRayXStart;
    public float vertiSlopeCheckRayYStart;
    public float vertiSlopeCheckRayLength;
    public float platformCheckRayXStart;
    public float platformCheckRayYStart;
    public float platformCheckRayLength;

    private Vector2 prevGroundVel;
    RaycastHit2D horizontalSlopeCheckRay;
    RaycastHit2D verticalSlopeCheckRay;

    bool flag;

    private void Start()
    {
        thisRenderer = GetComponent<Renderer>();
        landTarget = transform.parent.Find("LandTarget");
        wasPrevOnHorizontalSlope = false;
        wasPrevOnVerticalSlope = false;
        totalAmountRisenOrSunk = 0;
        flag = true;
    }

    void Update()
    {
        RaycastHit2D[] platformBaseCheckRay = Physics2D.RaycastAll(transform.position + Vector3.down * raycastDistanceMultiplier, Vector2.down, 0f, (1 << 17));

        sortingOrderAdjust();


        prevXVal = transform.position.x;
        prevYVal = transform.position.y;
        prevGroundVel = transform.position;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Slope")
        {
            newSlopeScript slopeScript = collision.transform.GetComponent<newSlopeScript>();
            BaseScript platform1BaseScript = slopeScript.platform1.transform.Find("base").GetComponent<BaseScript>();
            BaseScript platform2BaseScript = slopeScript.platform2.transform.Find("base").GetComponent<BaseScript>();
            platform1BaseScript.ToggleIgnoreObjectOnSlope(landTarget.GetComponent<Collider2D>(), true);
            platform2BaseScript.ToggleIgnoreObjectOnSlope(landTarget.GetComponent<Collider2D>(), true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Slope")
        {
            newSlopeScript slopeScript = collision.transform.GetComponent<newSlopeScript>();
            BaseScript platform1BaseScript = slopeScript.platform1.transform.Find("base").GetComponent<BaseScript>();
            BaseScript platform2BaseScript = slopeScript.platform2.transform.Find("base").GetComponent<BaseScript>();
            platform1BaseScript.ToggleIgnoreObjectOnSlope(landTarget.GetComponent<Collider2D>(), false);
            platform2BaseScript.ToggleIgnoreObjectOnSlope(landTarget.GetComponent<Collider2D>(), false);
        }
    }

    public void sortingOrderAdjust()
    {
        //FIGURE OUT: Do I put the offset here or in the positionRendererSort function?
        thisRenderer.sortingOrder = -(int)transform.position.y - offset;
        return;
    }
}
