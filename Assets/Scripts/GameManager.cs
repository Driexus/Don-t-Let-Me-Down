using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public Player player;
    public Map map;
    public EventSystem eventSystem;

    public void CheckState()
    {
        if (!player.HasTileUnderneath)
        {
            map.floor.SetActive(false);
            StartCoroutine(ReloadLevel());
        }

        else if (map.ActiveTilemap.GetTile(player.GridPosition) == map.EndTile)
        {
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
}
