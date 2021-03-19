using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public Animator transition;
    public void LoadSampleScene()
    {
        StartCoroutine(loadSampleScene());
    }

    private IEnumerator loadSampleScene()
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
}
