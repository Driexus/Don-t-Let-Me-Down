using UnityEngine;

/// <summary>
/// In order to Disable the movebuttons a canvas group in a gameobject tagged as MoveButtons must be included in the scene
/// </summary>

public class DisableMoveButtons : StateMachineBehaviour
{
    private CanvasGroup moveButtons;

    private void Awake()
    {
        moveButtons = GameObject.FindGameObjectWithTag("MoveButtons").GetComponent<CanvasGroup>();
    }

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        moveButtons.interactable = false;
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        moveButtons.interactable = true;
    }
}