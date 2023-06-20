using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    Player player;

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

    [HideInInspector]
    public Level level;

    [HideInInspector]
    public Map map;

    // Events
    public delegate void GameManagerEventHandler();
    public event GameManagerEventHandler OnGamePhaseStarted;
    public event GameManagerEventHandler OnMemorizationPhaseStarted;
    public event GameManagerEventHandler OnMemorizationPhaseEnded;


    bool canSkip;

    private void Awake()
    {
        // Reset the timescale
        Time.timeScale = 1f;

        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        OnMemorizationPhaseEnded += () => canSkip = false;
    }

    private void Start()
    {
        timer.SetTimer(ActiveTime, extraTime, MemorizationTime);

        lm.OnLevelFailed += () => StartCoroutine(ReloadMenu());
    }

    public void StartLevel()
    {
        player.OnStartedJumping += () => timer.NextTimer();
        player.OnStartedJumping += () => timer.PauseTimer();
        player.OnEndedJumping += () => timer.ResumeTimer();

        memorizationPhase = StartMemorizationPhase();
        StartCoroutine(memorizationPhase);
    }

    private IEnumerator StartGamePhase()
    {
        OnGamePhaseStarted?.Invoke();
        memorizationPhase = null;

        map.LoadFirstTilemap();

        yield return StartCoroutine(player.JumpAndWait(Vector3Int.up));
        movement.allowMovement = true;

        player.OnStartedAscending += () => map.NextTilemap();
        player.OnHasAscended += () => CheckState();

        // Removes the starting tile after jumping -> comment this line to cheat through the levels
        level.GoalPlatform.RemoveStart();
        timer.StartTimer();
    }

    // Checks if the player has a tile underneath or if he has won
    // Should get called after every movement
    // Returns true if player lost or false in any other case
    public void CheckState()
    {
        if (level.GoalPlatform.HasTile(player.transform.position))
        {
            lm.LevelCompleted();
            movement.allowMovement = false;
        }

        else if (!player.HasTileUnderneath(map.ActiveTilemap))
        {
            lm.LevelFailed();
            movement.allowMovement = false;
        }

        else
            movement.allowMovement = true;
    }

    // Like CheckState but get called preemptively to disable movement commands before the player arrives at the tile
    public bool CheckTile(Vector3Int coords)
    {
        if (level.GoalPlatform.HasTile(coords))
        {
            return false;
        }

        else if (!map.ActiveTilemap.HasTile(coords))
        {
            return false;
        }
        return true;
    }

    private IEnumerator StartMemorizationPhase()
    {
        OnMemorizationPhaseStarted?.Invoke();
        yield return StartCoroutine(map.LoadAllTilemaps());
        canSkip = true;
        yield return new WaitForSeconds(MemorizationTime);
        OnMemorizationPhaseEnded?.Invoke();
        StartCoroutine(StartGamePhase());
    }

    public void SkipMemorizationPhase()
    {
        if (canSkip)
        {
            StopCoroutine(memorizationPhase);
            OnMemorizationPhaseEnded?.Invoke();
            StartCoroutine(StartGamePhase());
        }
    }

    #region Pause Menu

    public GameObject pauseMenu;

    public void PauseUnPauseGame()
    {
        Canvas canvas = pauseMenu.transform.parent.parent.GetComponent<Canvas>();
        if (Time.timeScale == 1f)
        {
            // Make thruster particles invisible
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            pauseMenu.SetActive(true);
            Time.timeScale = 0f;
        }

        else if (Time.timeScale == 0f)
        {
            // Make robot faces visible
            canvas.renderMode = RenderMode.ScreenSpaceCamera;
            pauseMenu.SetActive(false);
            Time.timeScale = 1f;
        }
    }

    public IEnumerator ReloadMenu()
    {
        yield return new WaitForSeconds(4.5f);
        PauseUnPauseGame();
        pauseMenu.transform.Find("Resume").gameObject.SetActive(false);
        pauseMenu.transform.Find("Reload").gameObject.SetActive(true);
    }

    #endregion
}