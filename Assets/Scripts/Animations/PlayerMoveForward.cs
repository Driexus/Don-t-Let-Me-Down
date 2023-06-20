using UnityEngine;

/// <summary>
/// The player moves one tile forward respective to his rotation. The movement lasts the same duration as animation.
/// A GameObject tagged as Player with a Player script must be included in the scene
/// </summary>

public class PlayerMoveForward : StateMachineBehaviour
{
    private Player player;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player.MoveForSeconds(stateInfo.length);
    }
}
