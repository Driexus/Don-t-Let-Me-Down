using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    public StoryTeller storyTeller;

    public CanvasGroup RobotPanel;
    public CanvasGroup TargetPanel;
    public CanvasGroup GotItButton;

    bool robotMustFadeIn;
    bool targetMustFadeIn;
    bool gotItMustFadeIn;

    public float fadeInTime = 1f;

    private void Start()
    {
        GotItButton.alpha = 0f;
        GotItButton.gameObject.SetActive(false);
        storyTeller.OnStoryEnded += () =>  gameObject.SetActive(false);
        storyTeller.OnStoryEnded += () => Time.timeScale = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        if (storyTeller.CurrentLine == 1)
            Time.timeScale = 0f;

        if (storyTeller.CurrentLine == 2)
        {
            robotMustFadeIn = true;
            GotItButton.gameObject.SetActive(true);
            gotItMustFadeIn = true;
            storyTeller.useManualTransitions = true;
        }
        else
        {
            robotMustFadeIn = false;
            gotItMustFadeIn = false;
            RobotPanel.alpha = 0;
        }    
        if (storyTeller.CurrentLine == 3)
            targetMustFadeIn = true;
        else
        {
            targetMustFadeIn = false;
            TargetPanel.alpha = 0;
        }

        if (storyTeller.CurrentLine == 6)
        {
            GotItButton.gameObject.SetActive(false);
            storyTeller.useManualTransitions = false;
        }

        if (robotMustFadeIn)
            RobotPanel.alpha += Time.unscaledDeltaTime / fadeInTime;
        if (targetMustFadeIn)
            TargetPanel.alpha += Time.unscaledDeltaTime / fadeInTime;
        if (gotItMustFadeIn)
            GotItButton.alpha += Time.unscaledDeltaTime / fadeInTime;
    }
}
