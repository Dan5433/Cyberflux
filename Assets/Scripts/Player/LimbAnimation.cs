using UnityEngine;

[RequireComponent(typeof(Animator))]
public class LimbAnimation : MonoBehaviour
{
    static readonly string ANIMATION_PARAMETER_IS_WALKING = "walking";
    static readonly string ANIMATION_PARAMETER_IS_SPRINTING = "sprinting";
    static readonly string ANIMATION_PARAMETER_ON_GROUND = "onGround";

    Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void UpdateAnimations(Vector2 movementInput, bool isSprinting, bool onGround)
    {
        animator.SetBool(ANIMATION_PARAMETER_ON_GROUND, onGround);


        animator.SetBool(ANIMATION_PARAMETER_IS_WALKING, false);
        animator.SetBool(ANIMATION_PARAMETER_IS_SPRINTING, false);

        if (movementInput.magnitude <= 0)
            return;

        if (isSprinting)
            animator.SetBool(ANIMATION_PARAMETER_IS_SPRINTING, true);
        else
            animator.SetBool(ANIMATION_PARAMETER_IS_WALKING, true);
    }
}
