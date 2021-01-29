using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections;

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
        tilemaps[tilemapIndex].GetComponent<Animator>().SetTrigger("FadeOut");
        tilemapIndex++;

        if (tilemapIndex >= tilemapCount)
            tilemapIndex = 0;

        tilemaps[tilemapIndex].GetComponent<Animator>().SetTrigger("FadeIn");
    }

    public void LoadFirstTilemap()
    {
        if (tilemapIndex > 0)
            tilemaps[tilemapIndex].GetComponent<Animator>().SetTrigger("FadeOut");

        tilemapIndex = 0;
        Animator tilemapAnim = tilemaps[tilemapIndex].GetComponent<Animator>();
        AnimatorStateInfo animInfo = tilemapAnim.GetCurrentAnimatorStateInfo(0);

        if (animInfo.IsName("AlphaZero") ||  animInfo.IsName("FadeOut"))
            tilemapAnim.SetTrigger("FadeIn");
    }

    public bool ActiveTilemapHasLoaded
    {
        get
        {
            return tilemaps[tilemapIndex].GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("AlphaOne");
        }
    }   
}
