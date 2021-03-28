using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(Button))]
public class LevelButton : MonoBehaviour
{
    public int level;
    public TMP_Text tmp_text;
    Button button;
    MainMenuManager MMM;

    private void Awake()
    {
        button = GetComponent<Button>();
        MMM = GameObject.FindGameObjectWithTag("MainMenuManager").GetComponent<MainMenuManager>();
    }

    private void Start()
    {
        button.onClick.AddListener(StartLevel);
        tmp_text.text = level.ToString();
        if (Saver.LoadData().level < level)
            button.interactable = false;
    }
    public void StartLevel()
    {
        GameSceneData.Level = level;
        MMM.LoadGame();
    }
}
