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

    public Button skipButton;
    public CanvasGroup moveButtons;

    public LevelManager lm;

    private void Start()
    {
        StartLevel();
    }

    public void StartLevel()
    {
        moveButtons.interactable = false;
        map.LoadFirstTilemap();
        memorizationPhase = StartMemorizationPhase();
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
        if (!player.HasTileUnderneath(map.ActiveTilemap))
        {
            floor.SetActive(false);
            lm.OnLevelFailed();
        }

        else if (map.ActiveTilemap.GetTile(player.GridPosition(map.ActiveTilemap)) == map.EndTile)
        {
            moveButtons.interactable = false;
            StopTimer();
            lm.OnLevelCompleted();
        }
    }

    private IEnumerator StartMemorizationPhase()
    {
        textTimer.text = "Skip";
        skipButton.enabled = true;
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
