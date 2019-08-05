using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpactEffectBehaviour : StateMachineBehaviour {

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        if (animatorStateInfo.normalizedTime >= 1)
        {
            Destroy(animator.gameObject);
        }
    }
}
