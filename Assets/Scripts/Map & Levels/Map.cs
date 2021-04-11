using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Map : MonoBehaviour
{
    // An index pointing to the current active tilemap
    private int tilemapIndex;
    private Tilemap[] tilemaps;

    public delegate void MapEventHandler();
    public event MapEventHandler OnFirstTilemapHasLoaded;

    public void FirstTilemapHasLoaded()
    {
        OnFirstTilemapHasLoaded?.Invoke();
    }
    public bool firstTilemapHasLoaded;

    // Returns the active tilemap
    public Tilemap ActiveTilemap
    {
        get
        {
            return tilemaps[tilemapIndex];
        }
    }

    // Returns the total count of tilemaps
    public int tilemapCount
    {
        get
        {
            return transform.childCount;
        }
    }

    private void Awake()
    {
        tilemaps = new Tilemap[tilemapCount];

        for (int i = 0; i < tilemapCount; i++)
            tilemaps[i] = transform.GetChild(i).GetComponent<Tilemap>();
    }

    public void NextTilemap()
    {
        tilemaps[tilemapIndex].GetComponent<Animator>().SetTrigger("FadeOut");
        tilemapIndex++;

        if (tilemapIndex >= tilemapCount)
            tilemapIndex = 0;

        tilemaps[tilemapIndex].gameObject.SetActive(true);
    }

    
    public void LoadAllTilemaps()
    {
        foreach (Tilemap tilemap in tilemaps)
        {
            tilemap.gameObject.SetActive(true);
        }
    }

    public void LoadFirstTilemap()
    {
        for (int i = 1; i < tilemapCount; i++)
            tilemaps[i].GetComponent<Animator>().SetTrigger("FadeOut");
    }
}
