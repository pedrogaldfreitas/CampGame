using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class diagTimer : MonoBehaviour
{

    public float amountFilled = 100;

    public IEnumerator countdown(float timerSpeed)
    {
        while (GetComponent<Image>().fillAmount > 0)
        {

            GetComponent<Image>().fillAmount -= timerSpeed*0.01f;
            yield return new WaitForEndOfFrame();
        }
        GameObject.Find("Dialogue Manager").GetComponent<DialogueManager>().endDiagChoice();
        yield return new WaitForSeconds(0.4f);
        //GetComponent<Image>().fillAmount = 100;
    }
}
