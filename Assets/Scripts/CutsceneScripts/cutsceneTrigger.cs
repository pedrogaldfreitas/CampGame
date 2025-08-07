using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class cutsceneTrigger : MonoBehaviour
{
    [Header("Setup Variables")]
    [Tooltip("This value should be set to know where the Interact button should appear. This should be the LOCAL position, not GLOBAL position.")]
    public Vector2 interactButtonPosition;

    [Header("Test Variables")]
    Transform player;

    private bool isNPC;
    [SerializeField]
    public string cutsceneToTrigger;
    public bool triggeredByProximity;   
    private float proximityRadius;
    public bool triggeredByInteraction;
    public bool triggeredByCollision;

    public bool cutsceneTriggered;


    // Start is called before the first frame update
    void Start()
    {
        cutsceneTriggered = false;
        player = GameObject.Find("Player").transform;
        isNPC = transform.tag == "NPC";
    }

    // Update is called once per frame
    void Update()
    {
        if (triggeredByProximity == true)
        {
            if ((Vector2.Distance(player.position, transform.position) <= proximityRadius) && (cutsceneTriggered == false)) {
                cutsceneTriggered = true;
                GameObject.Find("UI").GetComponent<CutsceneManager>().playCutsceneUsingID(cutsceneToTrigger);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, proximityRadius);
    }
}
