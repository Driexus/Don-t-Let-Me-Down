using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    public StoryTeller storyTeller;

    public CanvasGroup RobotPanel;
    public CanvasGroup TargetPanel;

    bool robotMustFadeIn;
    bool targetMustFadeIn;

    public float fadeInTime = 1f;

    // Update is called once per frame
    void Update()
    {
        if (storyTeller.CurrentLine == 2)
            robotMustFadeIn = true;
        else
        {
            robotMustFadeIn = false;
            RobotPanel.alpha = 0;
        }    
        if (storyTeller.CurrentLine == 3)
            targetMustFadeIn = true;
        else
        {
            targetMustFadeIn = false;
            TargetPanel.alpha = 0;
        }

        if (storyTeller.CurrentLine == 2)
            storyTeller.useManualTransitions = true;

        if (robotMustFadeIn)
            RobotPanel.alpha += Time.unscaledDeltaTime / fadeInTime;
        if (targetMustFadeIn)
            TargetPanel.alpha += Time.unscaledDeltaTime / fadeInTime;
    }
}
