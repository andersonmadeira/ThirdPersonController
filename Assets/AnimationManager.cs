using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    Animator animator;

    int horizontalParameter;
    int verticalParameter;

    void Awake()
    {
        animator = GetComponent<Animator>();
        horizontalParameter = Animator.StringToHash("Horizontal");
        verticalParameter = Animator.StringToHash("Vertical");
    }

    public void UpdateAnimationValues(float horizontalMovement, float verticalMovement)
    {
        animator.SetFloat(horizontalParameter, SnapMovement(horizontalMovement));
        animator.SetFloat(verticalParameter, SnapMovement(verticalMovement));
    }

    float SnapMovement(float movementDirection)
    {
        if (movementDirection > 0 && movementDirection < 0.55f)
        {
            return 0.5f;
        }

        if (movementDirection > 0.55f)
        {
            return 1;
        }

        if (movementDirection < 0 && movementDirection > -0.55f)
        {
            return -0.5f;
        }

        if (movementDirection < -0.55f)
        {
            return -1;
        }

        return 0;
    }


}
