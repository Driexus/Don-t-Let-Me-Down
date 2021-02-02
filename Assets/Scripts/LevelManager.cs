using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public GameManager GM;
    public Player player;
    public GameObject WonScreen;
    public Animator SceneTransition;
    public GameObject currentLevel;
    public GameObject nextLevel;
    private Camera mainCam;
    private int levelIndex = 1;

    private void Awake()
    {
        mainCam = Camera.main;
        nextLevel = Instantiate(Resources.Load("Levels/Level2") as GameObject);
        nextLevel.SetActive(false);
    }

    public void OnLevelCompleted()
    {
        if (nextLevel == null)
        {
            WonScreen.SetActive(true);
            StartCoroutine(LoadMainMenu());
        }
        else
        {
            LoadNextLevel();
            GM.StartLevel();

           /* GM.map.gameObject.SetActive(false);
            nextMap.gameObject.SetActive(true);
            GM.map = nextMap;
            //ToDO: Fix Camera
            nextMap = null;
            GM.StartLevel();*/
        }
    }

    private void LoadNextLevel()
    {
        // TODO: fix camera

        player.grid = nextLevel.GetComponent<Grid>();

        nextLevel.transform.position = player.transform.position - player.playerWorldOffset;
        nextLevel.SetActive(true);
        mainCam.transform.position += nextLevel.transform.position;
        currentLevel.SetActive(false);
        GM.map = nextLevel.GetComponentInChildren<Map>();
        Destroy(currentLevel);
        currentLevel = nextLevel;
        levelIndex++;
        nextLevel = null;
        nextLevel = Resources.Load("Levels/Level" + (levelIndex + 1).ToString()) as GameObject;
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
