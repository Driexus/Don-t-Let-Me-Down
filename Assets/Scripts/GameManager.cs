using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Player player;
    public Map map;

    public GameObject floor;

    public int Repeats;
    public float AlterTime;

    public int ActiveTime;
    public TMP_Text textTimer;
    private IEnumerator timer = null;
    private IEnumerator memorizationPhase;

    public GameObject WonScreen;
    public Button skipButton;
    public CanvasGroup moveButtons;

    public Animator transition;

    public LevelManager lm;

    private void Start()
    {
        StartLevel();
    }

    public void StartLevel()
    {
        map.LoadFirstTilemap();
        memorizationPhase = AlternateTilemaps();
        StartCoroutine(memorizationPhase);
    }

    private IEnumerator StartGamePhase()
    {
        skipButton.enabled = false;
        
        map.LoadFirstTilemap();
        while (!map.ActiveTilemapHasLoaded)
            yield return null;

        player.Jump(Vector3Int.up);
        map.StartingTile.SetActive(false);
        StartTimer();
    }


    // Checks if the player has a tile underneath or if he has won
    // Should get called after every movement
    public void CheckState()
    {
        if (!player.HasTileUnderneath)
        {
            floor.SetActive(false);
            StartCoroutine(LoadMainMenu());
        }

        else if (map.ActiveTilemap.GetTile(player.GridPosition) == map.EndTile)
        {
            moveButtons.interactable = false;
            StopTimer();
            lm.OnLevelCompleted();
        }
    }

    public IEnumerator LoadMainMenu()
    {
        moveButtons.interactable = false;
        transition.speed = 0.4f;
        transition.SetTrigger("ChangeScene");
        yield return new WaitForSeconds(2.5f);
        SceneManager.LoadScene("MainMenu");
    }

    private IEnumerator AlternateTilemaps()
    {
        for (int i = 1; i < Repeats * map.tilemapCount; i++)
        {               
                yield return new WaitForSeconds(AlterTime);
                map.NextTilemap();
        }
        yield return new WaitForSeconds(AlterTime);
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
        floor.SetActive(false);
        StartCoroutine(LoadMainMenu());
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
