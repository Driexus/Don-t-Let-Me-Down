using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(Button))]
[RequireComponent(typeof(Image))]
public class LevelButton : MonoBehaviour
{
    public int level;
    public TMP_Text tmp_text;
    public Sprite LockedLevel;
    Button button;
    Image image;
    MainMenuManager MMM;

    private void Awake()
    {
        button = GetComponent<Button>();
        image = GetComponent<Image>();
        MMM = GameObject.FindGameObjectWithTag("MainMenuManager").GetComponent<MainMenuManager>();
    }

    private void Start()
    {
        button.onClick.AddListener(StartLevel);
        tmp_text.text = level.ToString();
        if (Saver.LoadData().level < level)
        {
            button.interactable = false;
            Destroy(tmp_text.gameObject);
            image.sprite = LockedLevel;
        }

    }
    public void StartLevel()
    {
        GameSceneData.Level = level;
        MMM.LoadGame();
    }
}
