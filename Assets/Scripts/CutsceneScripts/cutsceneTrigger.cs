using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class cutsceneTrigger : MonoBehaviour
{
    [SerializeField]
    public string cutsceneToTrigger;
    public bool triggeredByProximity;
    [Range(0f, 40f)]
    public float proximityRadius;
    public bool triggeredByInteraction;
    public bool triggeredByCollision;

    public bool cutsceneTriggered;

    // Start is called before the first frame update
    void Start()
    {
        cutsceneTriggered = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (triggeredByProximity == true)
        {
            if ((Vector2.Distance(GameObject.Find("Player").transform.position, this.transform.position) <= proximityRadius)&&(cutsceneTriggered == false)) {
                cutsceneTriggered = true;
                GameObject.Find("UI").GetComponent<CutsceneManager>().playCutsceneUsingID(cutsceneToTrigger);
            }
        }
        if (triggeredByInteraction == true)
        {
            if ((Input.GetKeyDown(KeyCode.K)) && (Vector2.Distance(GameObject.Find("Player").transform.position, this.transform.position) <= proximityRadius) && (cutsceneTriggered == false))
            {
                cutsceneTriggered = true;
                GameObject.Find("UI").GetComponent<CutsceneManager>().playCutsceneUsingID(cutsceneToTrigger);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(this.transform.position, proximityRadius);
    }
}
