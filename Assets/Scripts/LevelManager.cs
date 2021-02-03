using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public GameManager GM;
    public Player player;
    public GameObject WonScreen;
    public Animator SceneTransition;
   
    private Camera mainCam;
    
    private int levelIndex = 0;
    private GameObject currentLevel;
    private GameObject nextLevel;

    private void Awake()
    {
        mainCam = Camera.main;
        
        FetchNextLevel(levelIndex + 1);
        LoadNextLevel();
    }

    private void LoadNextLevel()
    {
        if (nextLevel == null)
            return;

        if (currentLevel != null)
            Destroy(currentLevel);

        StartCoroutine(LoadLevel(nextLevel));

        levelIndex++;
        FetchNextLevel(levelIndex + 1);
    }

    private void FetchNextLevel(int l)
    {
        nextLevel = Resources.Load("Levels/Level" + l.ToString()) as GameObject;
    }

    private IEnumerator LoadLevel(GameObject level)
    {

        if (level == null)
            throw new UnityException("attempting to set current level to null");

        currentLevel = Instantiate(level);
        currentLevel.transform.position = player.transform.position - player.playerWorldOffset;
        GM.level = currentLevel.GetComponent<Level>();
        GM.map = currentLevel.GetComponentInChildren<Map>();
        player.grid = currentLevel.GetComponent<Grid>();

        yield return StartCoroutine(MoveCamera());
        GM.StartLevel();
    }
    private IEnumerator MoveCamera()
    {
        mainCam.transform.position += currentLevel.transform.position;
        yield return new WaitForSeconds(1f);
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
        }
    }
}
