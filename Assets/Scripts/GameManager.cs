using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Player player;
    public Map map;

    public GameObject StartingTile;

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

    private void Start()
    {
        map.LoadFirstTilemap();
        memorizationPhase = AlternateTilemaps();
        StartCoroutine(memorizationPhase);
    }

    private IEnumerator StartGamePhase()
    {
        while (!map.ActiveTilemapHasLoaded)
            yield return null;
        
        skipButton.enabled = false;
        StartingTile.SetActive(false);
        moveButtons.interactable = true;
        StartTimer();
    }


    // Checks if the player has a tile underneath or if he has won
    // Should get called after every movement
    public void CheckState()
    {
        if (!player.HasTileUnderneath)
        {
            map.floor.SetActive(false);
            StartCoroutine(ReloadLevel());
        }

        else if (map.ActiveTilemap.GetTile(player.GridPosition) == map.EndTile)
        {
            StopTimer();
            WonScreen.SetActive(true);
            Debug.Log("Won");
            StartCoroutine(ReloadLevel());
        }
    }

    public IEnumerator ReloadLevel()
    {
        moveButtons.interactable = false;
        transition.speed = 0.4f;
        transition.SetTrigger("ChangeScene");
        yield return new WaitForSeconds(2.5f);
        SceneManager.LoadScene("MainMenu");
    }

    private IEnumerator AlternateTilemaps()
    {
        for (int i = 0; i < Repeats; i++)
        {
            for (int j = 0; j < map.tilemapCount; j++)
            {                
                yield return new WaitForSeconds(AlterTime);
                map.NextTilemap();
            }
        }
        StartCoroutine(StartGamePhase());
    }

    public void SkipMemorizationPhase()
    {
        if (memorizationPhase != null)
        {
            StopCoroutine(memorizationPhase);
            map.LoadFirstTilemap();
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
        map.floor.SetActive(false);
        StartCoroutine(ReloadLevel());
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
