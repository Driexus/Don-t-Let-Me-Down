using UnityEngine;

public class Fall : StateMachineBehaviour
{
    Player player;
    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player.StartedFalling();
    }
}
