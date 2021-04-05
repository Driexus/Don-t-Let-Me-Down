using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    Player player;
    public Map map;

    public Movement movement;

    // Memorization phase total time
    public float MemorizationTime;

    // Time before the platform fails
    public float ActiveTime;

    // Time added in each jump
    public float extraTime;

    private IEnumerator memorizationPhase;

    // The robot faces which count time
    public RobotTimer timer;

    public LevelManager lm;
    public Level level;

    // Events
    public delegate void GameManagerEventHandler();
    public event GameManagerEventHandler OnGamePhaseStarted;
    public event GameManagerEventHandler OnMemorizationPhaseStarted;
    public event GameManagerEventHandler OnMemorizationPhaseEnded;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    private void Start()
    {
        timer.SetTimer(ActiveTime, extraTime, MemorizationTime);

        // Debug stuff
        OnMemorizationPhaseStarted += () => Debug.Log("Mem started");
    }

    public void StartLevel()
    {
        memorizationPhase = StartMemorizationPhase();
        StartCoroutine(memorizationPhase);
    }

    private IEnumerator StartGamePhase()
    {
        OnGamePhaseStarted?.Invoke();
        memorizationPhase = null;
        
        map.LoadFirstTilemap();
        while (!map.firstTilemapHasLoaded)
            yield return null;

        yield return StartCoroutine(player.JumpAndWait(Vector3Int.up));
        movement.allowMovement = true;
        
        // Removes the starting tile after jumping -> comment this line to cheat through the levels
        level.GoalPlatform.RemoveStart();
        timer.StartTimer();
    }

    // Checks if the player has a tile underneath or if he has won
    // Should get called after every movement
    // Returns true if player lost or false in any other case
    public bool CheckState()
    {
        if (level.GoalPlatform.HasTile(player.transform.position))
        {
            lm.LevelCompleted();    
        }

        else if (!player.HasTileUnderneath(map.ActiveTilemap))
        {
            return true;
        }
        return false;
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
            lm.LevelFailed();
        }
    }

    private IEnumerator StartMemorizationPhase()
    {
        OnMemorizationPhaseStarted?.Invoke();
        map.LoadAllTilemaps();
        yield return new WaitForSeconds(MemorizationTime);
        OnMemorizationPhaseEnded?.Invoke();
        StartCoroutine(StartGamePhase());
    }

    public void SkipMemorizationPhase()
    {
        if (memorizationPhase != null)
        {
            StopCoroutine(memorizationPhase);
            OnMemorizationPhaseEnded?.Invoke();
            StartCoroutine(StartGamePhase());
        }
    }
}
