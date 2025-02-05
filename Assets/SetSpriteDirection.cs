using UnityEditor;
using UnityEngine;

public class SetSpriteDirection : StateMachineBehaviour
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    //override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    AnimationClip animationClip;
    //    int bindingLenght;
    //    EditorCurveBinding binding;
    //    // animator.gameObject.GetComponent<SpriteRenderer>().sprite = animator.
    //    animationClip = animator.GetCurrentAnimatorClipInfo(layerIndex)[0].clip;
    //    bindingLenght = AnimationUtility.GetObjectReferenceCurveBindings(animationClip).Length;
    //    binding = AnimationUtility.GetObjectReferenceCurveBindings(animationClip)[0];
    //    AnimationUtility.GetObjectReferenceCurve(animationClip, binding);
    //}

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
