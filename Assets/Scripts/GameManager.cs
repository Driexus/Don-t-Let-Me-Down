using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public Player player;
    public Map map;
    public EventSystem eventSystem;

    public GameObject StartingTile;

    public int Repeats;
    public float AlterTime;

    public int ActiveTime;
    public TMP_Text textTimer;
    private IEnumerator timer = null;

    private void Start()
    {
        StartCoroutine(AlternateTilemaps());
    }

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
            Debug.Log("Won");
            StartCoroutine(ReloadLevel());
        }
    }

    public IEnumerator ReloadLevel()
    {
        eventSystem.enabled = false;
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public IEnumerator AlternateTilemaps()
    {
        for (int i = 0; i < Repeats; i++)
        {
            for (int j = 0; j < map.tilemapCount; j++)
            {
                yield return new WaitForSeconds(AlterTime);
                map.NextTilemap();
            }
        }
        StartingTile.SetActive(false);
        StartTimer();
    }

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
