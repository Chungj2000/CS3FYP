using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Script for handling logic of animations a Unit has.
 * Currently not used due to bugs it raises.
 */
public class UnitAnimator : MonoBehaviour {
    
    private Animator animator;

    private const string isWalking = "IsWalking";
    private const string isDead = "IsDead";
    private const string attack = "Attack";
    private const string takeDamage = "TakeDamage";

    private void Start() {
        animator = GetComponentInChildren<Animator>();
    }

    public void PlayWalking() {
        Debug.Log("Playing walking animation.");
        animator.SetBool(isWalking, true);
    }

    public void StopWalking() {
        Debug.Log("Stopping walking animation.");
        animator.SetBool(isWalking, false);
    }

    public void PlayDead() {
        Debug.Log("Playing dead animation.");
        animator.SetBool(isDead, true);
    }

    public void PlayAttack() {
        Debug.Log("Playing attack animation.");
        animator.SetTrigger(attack);
    }

    public void PlayTakeDamage() {
        Debug.Log("Playing take_damage animation.");
        animator.SetTrigger(takeDamage);
    }

}
