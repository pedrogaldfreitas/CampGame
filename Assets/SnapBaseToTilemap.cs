using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[ExecuteInEditMode]
public class SnapBaseToTilemap : MonoBehaviour
{
    private Tilemap referenceTilemap;
    private Transform baseTransform;
    private Vector3 lastPosition;

    void Awake()
    {
        try
        {
            referenceTilemap = GameObject.Find("PlatformGrid").transform.Find("Tilemap").GetComponent<Tilemap>();
        } catch (System.Exception ex)
        {
            Debug.Log("ERROR: Platform couldn't find the tilemap to reference.");
        }
        baseTransform = transform.Find("base");
    }

    void Update()
    {
#if UNITY_EDITOR

        if (Application.isPlaying || referenceTilemap == null || baseTransform == null)
            return;

        if (!referenceTilemap.gameObject.activeInHierarchy || !referenceTilemap.enabled)
            return;

        if (transform.position != lastPosition) {
            Vector3 baseWorldPos = baseTransform.position;

            Vector3Int cellPos = referenceTilemap.WorldToCell(baseWorldPos);

            //Snap to center of the grid cell
            Vector3 snappedPosition = referenceTilemap.GetCellCenterWorld(cellPos);

            //Important: Calculate offset to move the whole parent so "Base" ends up at snappedPosition
            Vector3 offset = snappedPosition - baseTransform.position;
            transform.position += offset;

            lastPosition = transform.position;
        }
#endif
    }
}
