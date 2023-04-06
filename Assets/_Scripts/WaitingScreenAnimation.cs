using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitingScreenAnimation : MonoBehaviour {
    
    List<Animator> animationCharacters;

    //Start off initially animating.
    private bool isAnimating = true;

    [Tooltip("Animation delay between characters.")]
    [SerializeField] float characterAnimateWaitTime = .125f;
    [Tooltip("Animation delay between loops.")]
    [SerializeField] float characterAnimateLoopWaitTime = 1f;

    private const string animate = "Animate";

    private void Start() {
        //Get all animated characters in the object.
        animationCharacters = new List<Animator>(GetComponentsInChildren<Animator>());

        StartCoroutine(Animate());
    }

    IEnumerator Animate() {
        //Continue to animate until toggled off.
        while(isAnimating) {
            foreach(var animator in animationCharacters) {
                //Animate each character once until an entire cycle has been performed, then loop.
                animator.SetTrigger("Animate");
                yield return new WaitForSeconds(characterAnimateWaitTime);
            }
            yield return new WaitForSeconds(characterAnimateLoopWaitTime);
        }
    }

    //Turn off animation.
    public void ToggleIsAnimating() {
        isAnimating = !isAnimating;
    }

}
