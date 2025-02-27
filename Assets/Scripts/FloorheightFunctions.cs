using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class FloorheightFunctions
{
    public static float FindSlopeFloorh(Transform slopeObj, Transform slope)
    {
        float fh;
        float smallerFH = 0;
        float largerFH = 0;

        float horizontalFH = -9999;
        float verticalFH = -9999;

        Transform slopeBase = slope.parent.Find("base");
        if (slope.GetComponent<newSlopeScript>().isHorizontalSlope)
        {
            float slopeLeftSide = slopeBase.transform.position.x - slopeBase.transform.GetComponent<PolygonCollider2D>().bounds.extents.x;
            float slopeRightSide = slopeBase.transform.position.x + slopeBase.transform.GetComponent<PolygonCollider2D>().bounds.extents.x;

            float plat1FH = slope.GetComponent<newSlopeScript>().platform1.transform.Find("top").GetComponent<platformScript>().floorHeight;
            float plat2FH = slope.GetComponent<newSlopeScript>().platform2.transform.Find("top").GetComponent<platformScript>().floorHeight;

            smallerFH = plat1FH;
            largerFH = plat2FH;

            if (plat1FH > plat2FH)
            {
                smallerFH = plat2FH;
                largerFH = plat1FH;
            }

            //float objRelativeXPos = slopeLeftSide + this.transform.position.x;
            horizontalFH = (slopeObj.position.x - slopeLeftSide) / (slopeRightSide - slopeLeftSide); //This should be the % of the way up the slope.

            if (plat1FH > plat2FH)
            {
                horizontalFH = (1 - horizontalFH);
            }

            horizontalFH = horizontalFH * (largerFH - smallerFH);

            if (plat1FH < plat2FH)
            {
                horizontalFH = horizontalFH + slope.GetComponent<newSlopeScript>().platform1.transform.Find("top").GetComponent<platformScript>().floorHeight;
            }
            else
            {
                horizontalFH = horizontalFH + slope.GetComponent<newSlopeScript>().platform2.transform.Find("top").GetComponent<platformScript>().floorHeight;
            }
        }
        if (slope.GetComponent<newSlopeScript>().isVerticalSlope)
        {
            float slopeTopSide = slopeBase.transform.position.y + slopeBase.transform.GetComponent<PolygonCollider2D>().bounds.extents.y;
            float slopeBottomSide = slopeBase.transform.position.y - slopeBase.transform.GetComponent<PolygonCollider2D>().bounds.extents.y;

            float plat1FH = slope.GetComponent<newSlopeScript>().platform1.transform.Find("top").GetComponent<platformScript>().floorHeight;
            float plat2FH = slope.GetComponent<newSlopeScript>().platform2.transform.Find("top").GetComponent<platformScript>().floorHeight;

            smallerFH = plat1FH;
            largerFH = plat2FH;

            if (plat1FH > plat2FH)
            {
                smallerFH = plat2FH;
                largerFH = plat1FH;
            }

            verticalFH = ((slopeObj.position.y - slopeBottomSide) / (slopeTopSide - slopeBottomSide)); //This should be the % of the way up the slope.

            verticalFH = verticalFH * (largerFH - smallerFH);

            if (plat1FH < plat2FH)
            {
                verticalFH = verticalFH + slope.GetComponent<newSlopeScript>().platform1.transform.Find("top").GetComponent<platformScript>().floorHeight;
            }
            else
            {
                verticalFH = verticalFH + slope.GetComponent<newSlopeScript>().platform2.transform.Find("top").GetComponent<platformScript>().floorHeight;
            }

        }

        if (!slope.GetComponent<newSlopeScript>().isHorizontalSlope && !slope.GetComponent<newSlopeScript>().isVerticalSlope)
        {
            Debug.LogError("ERROR: Slope not set as horizontal or vertical.");
        }

        fh = (horizontalFH > verticalFH) ? horizontalFH : verticalFH;
        Debug.Log("PEDROLOG#4: verticalFH = " +  verticalFH + ", fh = " + fh);

        fh = Mathf.Clamp(fh, smallerFH, largerFH);

        return fh;
    }
}
