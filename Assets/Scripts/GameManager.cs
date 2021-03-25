using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    Player player;
    public Map map;

    public Movement movement;

    // Memorization phase total time
    public float MemorizationTime;

    // Time before the platform fails
    public int ActiveTime;
    
    public TMP_Text textTimer;
    private IEnumerator timer = null;
    private IEnumerator memorizationPhase;

    public Button skipButton;

    public LevelManager lm;
    public Level level;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    public void StartLevel()
    {
        memorizationPhase = StartMemorizationPhase();
        StartCoroutine(memorizationPhase);
    }

    private IEnumerator StartGamePhase()
    {
        memorizationPhase = null;
        skipButton.enabled = false;
        
        map.LoadFirstTilemap();
        while (!map.firstTilemapHasLoaded)
            yield return null;

        player.Jump(Vector3Int.up);
        movement.allowMovement = true;
        
        // Removes the starting tile after jumping -> comment this line to cheat through the levels
        level.GoalPlatform.RemoveStart();
        StartTimer();
    }

    // Checks if the player has a tile underneath or if he has won
    // Should get called after every movement
    public void CheckState()
    {
        if (level.GoalPlatform.HasTile(player.transform.position))
        {
            StopTimer();
            lm.OnLevelCompleted();
        }

        else if (!player.HasTileUnderneath(map.ActiveTilemap))
        {
            StopTimer();
            player.Fall();
            lm.OnLevelFailed();
        }
    }

    // Like CheckState but get called preemptively to disable movement commands before the player arrives at the tile
    public void CheckTile(Vector3Int coords)
    {
        if (level.GoalPlatform.HasTile(coords))
        {
            movement.allowMovement = false;
        }

        else if (!map.ActiveTilemap.HasTile(coords))
        {
            movement.allowMovement = false;
        }
    }

    private IEnumerator StartMemorizationPhase()
    {
        textTimer.text = "Skip";
        skipButton.enabled = true;
        map.LoadAllTilemaps();
        yield return new WaitForSeconds(MemorizationTime);
        StartCoroutine(StartGamePhase());
    }

    public void SkipMemorizationPhase()
    {
        if (memorizationPhase != null)
        {
            StopCoroutine(memorizationPhase);
            StartCoroutine(StartGamePhase());
        }
    }    


    /// <summary>
    /// Timer Logic
    /// </summary>

    private IEnumerator CountSeconds()
    {
        int timeTillFall = ActiveTime;
        while (timeTillFall >= 0)
        {
            textTimer.text = timeTillFall.ToString();
            timeTillFall--;
            yield return new WaitForSeconds(1f);
        }
        map.ActiveTilemap.gameObject.SetActive(false);
        lm.OnLevelFailed() ;
    }

    public void StartTimer()
    {
        if (timer != null)
        {
            Debug.LogWarning("Timer already running");
            return;
        }    

        timer = CountSeconds();
        StartCoroutine(timer);
    }

    public void StopTimer()
    {
        if (timer == null)
            return;

        StopCoroutine(timer);
        timer = null;
    }

    public void ResetTimer()
    {
        StopTimer();
        StartTimer();                 
    }
}
