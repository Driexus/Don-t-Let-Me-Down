using UnityEngine;

public class Player : MonoBehaviour
{
    public Map map;
    public Vector3 Offset;

    public bool HasTileUnderneath
    {
        get
        {
            Vector3Int posToCheck = map.ActiveTilemap.WorldToCell(transform.position);
            return map.ActiveTilemap.HasTile(posToCheck);
        }
    }

    public Vector3Int GridPosition
    {
        get
        {
            return map.ActiveTilemap.WorldToCell(transform.position);
        }
    }

    public void MoveToTile(Vector3Int coords)
    {
        if (map.ActiveTilemap.HasTile(coords))
            transform.position = map.ActiveTilemap.CellToWorld(coords) + Offset;
    }
}
