using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotFall : StateMachineBehaviour
{
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (animator.GetBool("Fall"))
            player.AddComponent(typeof(Rigidbody));
    }
}
