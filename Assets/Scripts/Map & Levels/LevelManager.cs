using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{   
    Player player;
    public GameManager GM;
    public GameObject WonScreen;
    public GameObject LoseScreen;
    public Animator SceneTransition;
    private Vector3 camOffset;
   
    private Camera mainCam;
    
    private int levelIndex = 0;
    private GameObject currentLevel;
    private GameObject nextLevel;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        mainCam = Camera.main;
        camOffset = mainCam.transform.position;
    }

    private void Start()
    {
        FetchNextLevel();
        LoadNextLevel();
    }

    // Destroys the current level, loads the level saved to nextLevel and lastly fetches the next level
    // If nextLevel is null it exits
    private void LoadNextLevel()
    {
        if (nextLevel == null)
            return;
        
        if (currentLevel != null)
            Destroy(currentLevel);

        StartCoroutine(LoadLevel(nextLevel));

        levelIndex++;
        FetchNextLevel();
    }

    // Fetches the [levelIndex + 1] level from Resources/Levels and saves it nextLevel
    private void FetchNextLevel()
    {
        nextLevel = Resources.Load("Levels/Level" + (levelIndex + 1).ToString()) as GameObject;
    }

    // Instantiates level, fixes its transform, sets up GM, moves camera and lastly calls GM.StartLevel()
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

    // Moves the camera to target position using LerpMove
    private IEnumerator MoveCamera()
    {
        LerpMove lerpMove = mainCam.GetComponent<LerpMove>();
        lerpMove.MoveTo(currentLevel.transform.position + camOffset);

        yield return null;
        while (lerpMove.IsMoving)
            yield return null;
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
        LoseScreen.SetActive(true);
        StartCoroutine(LoadMainMenu());
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
