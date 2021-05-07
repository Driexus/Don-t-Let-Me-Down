using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenuManager : MonoBehaviour
{
    public Animator transition;
    public TMP_Text continueButton;

    private void Awake()
    {
        // If first time opening start on level 1
        if (PlayerPrefs.GetInt("FIRSTTIMEOPENING", 1) == 1)
        {
            GameData data = new GameData(1, 1, 0, 0, -1);
            Saver.SaveData(data);
            PlayerPrefs.SetInt("FIRSTTIMEOPENING", 0);
        }
    }

    public void Start()
    {
        RefreshContinueButton();
    }

    // Changes the text of continue button
    void RefreshContinueButton()
    {
        GameData data = Saver.LoadData();
        if (data.highscoreLevel == 1 && data.letdowns == 0)
            continueButton.text = "New Run";
    }

    public void LoadGame()
    {
        StartCoroutine(loadGame());
    }

    private IEnumerator loadGame()
    {     
        // Trigger ChangeScene to fade out and then wait till we get to the SceneLoading animation to load the next scene
        transition.SetTrigger("ChangeScene");
        while (!transition.GetCurrentAnimatorStateInfo(0).IsName("SceneLoading"))
            yield return null;
        
        SceneManager.LoadScene("Game");
    }

    public void QuitApplication()
    {
        Application.Quit();
    }

    // Continue from last highscore level
    public void ContinueHighscoreLevel()
    {
        // Load the save file
        GameData data = Saver.LoadData();
        GameSceneData.HighscoreEnabled = true;
        GameSceneData.Level = data.highscoreLevel;
        LoadGame();
    }

    public void ResetCurrentRun()
    {
        GameData data = Saver.LoadData();
        data.highscoreLevel = 1;
        data.letdowns = 0;
        Saver.SaveData(data);
        RefreshContinueButton();
    }
}
