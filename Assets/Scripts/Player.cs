using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Player : MonoBehaviour
{
    public Map map;

    public bool HasFloor
    {
        get
        {
            Vector3Int posToCheck = map.ActiveTilemap.WorldToCell(transform.position);
            return map.ActiveTilemap.HasTile(posToCheck);
        }
    }
}
