using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Descending : StateMachineBehaviour
{
    Player player;
    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player.StartedDescending();
    }
}
