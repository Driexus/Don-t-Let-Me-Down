using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public Animator transition;

    private void Awake()
    {
        // If first time opening start on level 1
        if (PlayerPrefs.GetInt("FIRSTTIMEOPENING", 1) == 1)
        {
            Saver.SaveData(1);
            PlayerPrefs.SetInt("FIRSTTIMEOPENING", 0);
        }
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

    // Continue from last unlocked level
    public void ContinueLastLevel()
    {
        // Load the save file
        GameData data = Saver.LoadData();
        GameSceneData.Level = data.level;
        LoadGame();
    }
}
