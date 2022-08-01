using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hitRecoveryTime : MonoBehaviour
{

    IEnumerator hitRecovery()
    {
        yield return new WaitForSeconds(3);
    }
}
