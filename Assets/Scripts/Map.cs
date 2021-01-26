﻿using UnityEngine;
using UnityEngine.Tilemaps;

public class Map : MonoBehaviour
{
    private int tilemapIndex;
    private Tilemap[] tilemaps;
    public GameObject floor;
    public TileBase EndTile;

    public Tilemap ActiveTilemap
    {
        get
        {
            return tilemaps[tilemapIndex];
        }
    }

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
        tilemaps[tilemapIndex].gameObject.SetActive(false);
        tilemapIndex++;
        
        if (tilemapIndex >= tilemapCount)
            tilemapIndex = 0;

        tilemaps[tilemapIndex].gameObject.SetActive(true);
    }

    public void PreviousTilemap()
    {
        tilemaps[tilemapIndex].gameObject.SetActive(false);
        tilemapIndex--;

        if (tilemapIndex < 0)
            tilemapIndex = tilemapCount - 1;

        tilemaps[tilemapIndex].gameObject.SetActive(true);
    }
}
