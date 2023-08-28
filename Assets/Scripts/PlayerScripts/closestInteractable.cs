using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class closestInteractable : MonoBehaviour
{
    public List<Transform> possibleInteractables;
    public Transform highlightedInteractable;
    public Transform prevHighlightedInteractable;
    private Vector3 targetButtonScreenPoint;
    public float interactButtonInterpolation;
    private bool targetReached;

    private Transform interactButton;
    private bool interactButtonActive;

    private Coroutine coroutineRunning;

    Transform interactableItemBody;
    cutsceneTrigger highlightedInteractableCutsceneTrigger;

    void Start()
    {
        highlightedInteractable = null;
        prevHighlightedInteractable = null;
        targetButtonScreenPoint = new Vector2(0,0);
        interactButtonActive = false;
        possibleInteractables = new List<Transform>();
        interactButton = GameObject.Find("InteractButton").transform;
        targetReached = true;
    }

    private void Update()
    {
        if (possibleInteractables.Count > 0)
        {
            Transform closestInteractable = possibleInteractables[0];
            foreach (Transform interactable in possibleInteractables)
            {
                if (Vector2.Distance(transform.position, interactable.position) < Vector2.Distance(transform.position, closestInteractable.position))
                {
                    closestInteractable = interactable;
                }
            }
            highlightedInteractable = closestInteractable;
        }

        //Hide the interact button.
        if (highlightedInteractable == null && interactButtonActive)
        {
            if (coroutineRunning != null)
            {
                StopCoroutine(coroutineRunning);
            }
            coroutineRunning = StartCoroutine(HideInteractButton());
        }

        //Show/Interact with the interact button.
        if (highlightedInteractable != null)
        {
            if (prevHighlightedInteractable != highlightedInteractable)
            {
                interactableItemBody = highlightedInteractable.parent.GetComponent<FakeHeightObject>().transBody;
                highlightedInteractableCutsceneTrigger = highlightedInteractable.parent.GetComponent<cutsceneTrigger>();
                targetReached = false;
            }
            
            Vector3 interactButtonOffset = highlightedInteractableCutsceneTrigger.interactButtonPosition;
            targetButtonScreenPoint = Camera.main.WorldToScreenPoint(interactableItemBody.position + interactButtonOffset);
            // targetButtonScreenPoint = Camera.main.WorldToScreenPoint(interactableItemBody.position + interactButtonOffset);

            if (!interactButtonActive)
            {
                if (coroutineRunning != null)
                {
                    StopCoroutine(coroutineRunning);
                }
                coroutineRunning = StartCoroutine(ShowInteractButton(highlightedInteractable));
            } else
            {
                if (interactButton.position != targetButtonScreenPoint)
                {
                    if (!targetReached)
                    {
                        interactButton.position = new Vector2(Mathf.Lerp(interactButton.position.x, targetButtonScreenPoint.x, interactButtonInterpolation), Mathf.Lerp(interactButton.position.y, targetButtonScreenPoint.y, interactButtonInterpolation));
                    } else
                    {
                        interactButton.position = targetButtonScreenPoint;
                    }
                }

                if (Vector2.Distance(interactButton.position, targetButtonScreenPoint) < 0.01 && !targetReached)
                {
                    targetReached = true;
                }
            }

            if (Input.GetKeyDown(KeyCode.A))
            {
                StopCoroutine(coroutineRunning);
                Interact(highlightedInteractable);
            }
        }

        prevHighlightedInteractable = highlightedInteractable;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("LandTarget") && collision.transform.parent.GetComponent<cutsceneTrigger>() != null && !possibleInteractables.Contains(collision.transform))
        {
            possibleInteractables.Add(collision.transform);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (possibleInteractables.Contains(collision.transform))
        {
            possibleInteractables.Remove(collision.transform);
            if (highlightedInteractable == collision.transform)
            {
                highlightedInteractable = null;
            }
        }
    }

    private void Interact(Transform item)
    {
        coroutineRunning = StartCoroutine(HideInteractButton());

        //CODE HERE for interaction.
    }

    private IEnumerator ShowInteractButton(Transform item)
    {
        interactButtonActive = true;

        RectTransform interactButtonRect = interactButton.GetComponent<RectTransform>();
        while (Mathf.Abs(interactButtonRect.localScale.x - 28) > 0.01)
        {
            interactButtonRect.localScale = new Vector3(Mathf.Lerp(interactButtonRect.localScale.x, 28, interactButtonInterpolation), Mathf.Lerp(interactButtonRect.localScale.y, 28, interactButtonInterpolation), 1);
            yield return new WaitForSeconds(0.01f);
        }

        interactButtonRect.localScale = new Vector3(28, 28, 1);
    }

    private IEnumerator HideInteractButton()
    {
        interactButtonActive = false;

        RectTransform interactButtonRect = interactButton.GetComponent<RectTransform>();
        while (interactButtonRect.localScale.x > 0.01)
        {
            interactButtonRect.localScale = new Vector3(Mathf.Lerp(interactButtonRect.localScale.x, 0, interactButtonInterpolation), Mathf.Lerp(interactButtonRect.localScale.y, 0, interactButtonInterpolation), 1);
            yield return new WaitForSeconds(0.01f);
        }

        interactButtonRect.localScale = new Vector3(0, 0, 1);
    }
}
