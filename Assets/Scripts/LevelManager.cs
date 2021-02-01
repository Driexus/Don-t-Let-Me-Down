using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public GameManager GM;
    public Player player;
    public GameObject WonScreen;
    public Animator SceneTransition;
    public Map nextMap;

    public void OnLevelCompleted()
    {
        if (nextMap == null)
        {
            WonScreen.SetActive(true);
            StartCoroutine(LoadMainMenu());
        }
        else
        {
            GM.map.gameObject.SetActive(false);
            nextMap.gameObject.SetActive(true);
            GM.map = nextMap;
            //ToDO: Fix Camera
            nextMap = null;
            GM.StartLevel();
        }
    }

    public IEnumerator LoadMainMenu()
    {
        SceneTransition.speed = 0.4f;
        SceneTransition.SetTrigger("ChangeScene");
        yield return new WaitForSeconds(2.5f);
        SceneManager.LoadScene("MainMenu");
    }

    public void OnLevelFailed()
    {
        Debug.Log("Level Failed");
    }
}
