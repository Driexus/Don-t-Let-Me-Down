using UnityEngine;

public class Jump_End : StateMachineBehaviour
{
    Player player;
    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player.EndedJumping();
    }
}