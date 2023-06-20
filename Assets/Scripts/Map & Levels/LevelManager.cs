using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{   
    Player player;
    public GameManager GM;
    public Animator SceneTransition;
    private Vector3 camOffset;
   
    private Camera mainCam;
    
    private int levelIndex = 1;
    private GameObject currentLevel;
    private GameObject nextLevel;

    public Tutorial tutorial;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        mainCam = Camera.main;
        camOffset = mainCam.transform.position;

        levelIndex = GameSceneData.Level;
    }

    private void Start()
    {
        // Enable tutorial on first level
        GM.OnMemorizationPhaseStarted += () => {
            if (levelIndex == 1 && PlayerPrefs.GetInt("OpenTutorial", 1) == 1)
            {
                PlayerPrefs.SetInt("OpenTutorial", 0);
                tutorial.gameObject.SetActive(true);
            }    
        };
       
        FetchNextLevel();
        LoadNextLevel(false);
    }

    // Destroys the current level, loads the level saved to nextLevel and lastly fetches the next level
    // If nextLevel is null it exits
    private void LoadNextLevel(bool moveCamera = true)
    {
        if (nextLevel == null)
            return;
        
        if (currentLevel != null)
            Destroy(currentLevel);

        StartCoroutine(LoadLevel(nextLevel, moveCamera));

        levelIndex++;
        FetchNextLevel();
    }

    // Fetches the [levelIndex] level from Resources/Levels and saves it nextLevel
    private void FetchNextLevel()
    {
        nextLevel = Resources.Load("Levels/Level" + (levelIndex).ToString()) as GameObject;
    }

    // Instantiates level, fixes its transform, sets up GM, moves camera (if true) and lastly calls GM.StartLevel()
    private IEnumerator LoadLevel(GameObject level, bool moveCamera)
    {

        if (level == null)
            throw new UnityException("attempting to set current level to null");

        currentLevel = Instantiate(level);
        currentLevel.transform.position = player.transform.position - player.playerWorldOffset;
        GM.level = currentLevel.GetComponent<Level>();
        GM.map = currentLevel.GetComponentInChildren<Map>();
        player.grid = currentLevel.GetComponent<Grid>();

        if (moveCamera)
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

    /// <summary>
    /// Spaghetti different scenes and won game
    /// </summary>
    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
    IEnumerator FadeScene()
    {
        yield return new WaitForSeconds(2f);

        SceneTransition.speed = 0.4f;
        SceneTransition.SetTrigger("ChangeScene");
        if (!SceneTransition.GetCurrentAnimatorStateInfo(0).IsName("SceneLoading"))
            yield return null;
    }

    IEnumerator WonGame()
    {
        yield return new WaitForSeconds(4.5f);
        LoadMainMenu();
    }    

    // Events
    public delegate void levelEventHandler();
    public event levelEventHandler OnLevelFailed;

    public void LevelFailed()
    {
        // Add another letdown to saver
        GameData data = Saver.LoadData();
        data.totalLetdowns++;

        if (GameSceneData.HighscoreEnabled)
            data.letdowns++;

        Saver.SaveData(data);

        OnLevelFailed?.Invoke();
        StartCoroutine(FadeScene());
    }

    public event levelEventHandler OnLevelCompleted;
    public void LevelCompleted()
    {
        GameSceneData.Level = levelIndex;
        GameData data = Saver.LoadData();

        if (nextLevel == null)
        {
            if (GameSceneData.HighscoreEnabled)
            {
                if (data.letdowns < data.highscore || data.highscore < 0)
                    data.highscore = data.letdowns;

                data.highscoreLevel = 1;
                data.letdowns = 0;
            }

            StartCoroutine(FadeScene());
            StartCoroutine(WonGame());
        }
        else
        {
            player.Idle(Vector3Int.up);
            LoadNextLevel();

            if (data.level < levelIndex)
                data.level = levelIndex;

            if (GameSceneData.HighscoreEnabled && data.highscoreLevel < levelIndex)
                data.highscoreLevel = levelIndex;
        }

        Saver.SaveData(data);

        OnLevelCompleted?.Invoke();
    }

    // For UI button
    public void ReloadLevel()
    {       
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}