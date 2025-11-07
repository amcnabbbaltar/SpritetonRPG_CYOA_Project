using UnityEngine; 
using System.Collections;
public class Effect : MonoBehaviour
{
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogWarning($"{name}: No Animator found, destroying after fallback time.");
            Destroy(gameObject, 1f); // fallback
            return;
        }

        // Start coroutine to wait until animation ends
        StartCoroutine(DestroyAfterAnimation());
    }

    private IEnumerator DestroyAfterAnimation()
    {
        // Get the current state info
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        float waitTime = stateInfo.length;

        // Wait for the animation duration
        yield return new WaitForSeconds(waitTime);

        Destroy(gameObject);
    }
}