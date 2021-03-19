using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Tilemap))]
public class GoalPlatform : MonoBehaviour
{
    private Tilemap tilemap;

    private void Awake()
    {
        tilemap = GetComponent<Tilemap>();
    }
    public bool HasTile(Vector3 targetPos)
    {
        Vector3Int posToCheck = tilemap.WorldToCell(targetPos);
        return tilemap.HasTile(posToCheck);
    }

    public bool HasTile(Vector3Int targetPos)
    {
        return tilemap.HasTile(targetPos);
    }

    public void RemoveStart()
    {
        tilemap.SetTile(new Vector3Int(0, 0, 0), null);
    }
}
