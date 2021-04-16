using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Map : MonoBehaviour
{
    Player player;

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

    // Add RB when start falling
    Player.PlayerEventHandler fallHandler;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

        fallHandler = () =>
        {
            if (player.HasTileUnderneath(ActiveTilemap))
                gameObject.AddComponent<Rigidbody>();
        };

        player.OnStartedFalling += fallHandler;
    }

    // Unregister the fallHandler
    private void OnDestroy()
    {
        player.OnStartedFalling -= fallHandler;
    }

    public void NextTilemap()
    {
        Debug.Log("Next Tilemap");
        tilemaps[tilemapIndex].GetComponent<Animator>().SetTrigger("FadeOut");
        tilemapIndex++;

        if (tilemapIndex >= tilemapCount)
            tilemapIndex = 0;

        tilemaps[tilemapIndex].gameObject.SetActive(true);
    }

    // Loads all tilemaps and waits for them
    public IEnumerator LoadAllTilemaps()
    {
        foreach (Tilemap tilemap in tilemaps)
        {
            tilemap.gameObject.SetActive(true);
        }

        AnimatorStateInfo info = tilemaps[0].GetComponent<Animator>().GetCurrentAnimatorStateInfo(0);
        float seconds = info.length * info.speed;
        yield return new WaitForSeconds(seconds);
    }

    public void LoadFirstTilemap()
    {
        Debug.Log("First Tilemap");
        for (int i = 1; i < tilemapCount; i++)
            tilemaps[i].GetComponent<Animator>().SetTrigger("FadeOut");
    }
}
