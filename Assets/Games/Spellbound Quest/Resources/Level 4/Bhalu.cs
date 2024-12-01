using UnityEngine;

public class Bhalu : MonoBehaviour
{
    [Header("Attack Settings")]
    [Tooltip("The interval between each attack in seconds.")]
    public float attackInterval = 5f;

    [Header("References")]
    [Tooltip("Animator component for playing attack animations.")]
    public Animator animator;

    [Tooltip("BoxCollider2D used for detecting attack collisions.")]
    public BoxCollider2D attackCollider;

    private void Start()
    {
        // Validate components
        if (animator == null)
        {
            Debug.LogError("Animator is not assigned.");
        }

        if (attackCollider == null)
        {
            Debug.LogError("BoxCollider2D is not assigned.");
            return;
        }

        // Disable the BoxCollider2D at the start
        attackCollider.enabled = false;

        // Start the attack loop
        InvokeRepeating(nameof(PerformAttack), attackInterval, attackInterval);
    }

    private void PerformAttack()
    {
        Debug.Log("Performing attack.");

        // Play attack animation
        animator.SetBool("isPunching", true);

        // Enable the attack collider for the duration of the attack
        attackCollider.enabled = true;

        // Schedule to end the attack after the animation duration
        float animationDuration = GetAnimationClipLength("Bhalu Punching"); // Replace with your animation clip name
        Invoke(nameof(EndAttack), animationDuration);
    }

    private void EndAttack()
    {
        Debug.Log("Ending attack.");

        // Stop the attack animation
        animator.SetBool("isPunching", false);

        // Disable the attack collider
        attackCollider.enabled = false;
    }

    private float GetAnimationClipLength(string clipName)
    {
        // Find and return the length of the specified animation clip
        if (animator.runtimeAnimatorController == null)
        {
            Debug.LogWarning("Animator Controller is not assigned.");
            return 0f;
        }

        foreach (AnimationClip clip in animator.runtimeAnimatorController.animationClips)
        {
            if (clip.name == clipName)
            {
                return clip.length;
            }
        }

        Debug.LogWarning($"Animation clip {clipName} not found.");
        return 0f;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the collider triggered with the player
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Attack hit the player!");
        }
    }
}
