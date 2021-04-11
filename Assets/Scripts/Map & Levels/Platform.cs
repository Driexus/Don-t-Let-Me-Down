using UnityEngine;
using UnityEngine.Tilemaps;
using DLMD.PlatformColors;

public class Platform : MonoBehaviour
{
    public PlatformColor color;
    public Tilemap tilemap;

    GameObject APlatform;

    private void Awake()
    {
        gameObject.GetComponent<TilemapRenderer>().enabled = false;
        GameObject aplatform = Resources.Load("Platform/Ascending Platform") as GameObject;
        GameObject acube = Resources.Load("Platform/Pretty Cube V3") as GameObject;

        APlatform = Instantiate(aplatform, transform);

        // Spaghetti logic
        bool isGoal = TryGetComponent(out GoalPlatform g);
        if (isGoal)
        {
            APlatform.GetComponent<AscendingPlatform>().skipAnimation = true;
        }

        foreach (var pos in tilemap.cellBounds.allPositionsWithin)
        {
            Vector3Int localPlace = new Vector3Int(pos.x, pos.y, pos.z);
            Vector3 place = tilemap.CellToWorld(localPlace) + (Vector3.right + Vector3.forward - Vector3.up) * 0.5f;
            if (tilemap.HasTile(localPlace))
            {

                Instantiate(acube, place, acube.transform.rotation, APlatform.transform);
            }
        }

        APlatform.GetComponent<AscendingPlatform>().SetPlatformColor(color);
        APlatform.SetActive(true);
        
        if (isGoal)
        {
            APlatform.GetComponent<AscendingPlatform>().SkipAnimation();
        }
    }

    public void OnPlatformHasDescended()
    {
        gameObject.SetActive(false);
    }

    public void OnPlatformBegunDescending()
    {
        APlatform.GetComponent<Animator>().SetTrigger("PlatformDown");
    }
}
