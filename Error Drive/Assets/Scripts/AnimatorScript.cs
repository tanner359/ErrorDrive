using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorScript : MonoBehaviour
{
    private Animator animator;
    private string idleBool = "idle";
    private string punchBool = "punch";
    private string moveBool = "move";

    public bool yes;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (yes == true)
        {
            AnimateIdle();
        }
    }

    public void AnimateIdle()
    {
        Debug.Log("Idleactivated");
        Animate(idleBool);
    }
    public void AnimatePunch()
    {
        Debug.Log("punchactivated");
        Animate(punchBool);
    }
    public void AnimateMove()
    {
        Animate(moveBool);
    }

    private void Animate(string boolName)
    {
        DisableNonCodeAnimations(boolName);

        animator.SetBool(boolName, true);
    }

    private void DisableNonCodeAnimations(string animation)
    {
        foreach(AnimatorControllerParameter parameter in animator.parameters)
        {
            if(parameter.name != animation)
            {
                animator.SetBool(parameter.name, false);
            }
        }
    }
}
