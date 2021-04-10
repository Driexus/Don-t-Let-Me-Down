using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using DLMD.PlatformColors;

public class Platform : MonoBehaviour
{
    public PlatformColor color;
    public Tilemap tilemap;

    public List<Vector3> tileWorldLocations;

    private void Awake()
    {
        GameObject aplatform = Resources.Load("Platform/Ascending Platform") as GameObject;
        GameObject acube = Resources.Load("Platform/Pretty Cube V3") as GameObject;

        GameObject APlatform = Instantiate(aplatform, transform);

        foreach (var pos in tilemap.cellBounds.allPositionsWithin)
        {
            Vector3Int localPlace = new Vector3Int(pos.x, pos.y, pos.z);
            Vector3 place = tilemap.CellToWorld(localPlace);
            if (tilemap.HasTile(localPlace))
            {

                Instantiate(acube, place, acube.transform.rotation, APlatform.transform);
            }
        }

        APlatform.GetComponent<AscendingPlatform>().SetPlatformColor(color);
        APlatform.SetActive(true);
    }
}
