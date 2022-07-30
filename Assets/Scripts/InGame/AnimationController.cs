using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class AnimationController : MonoBehaviourPun
{
    Animator animator;
    private string currentState;

    private void Awake() {
        animator = GetComponent<Animator>();
    }

    [PunRPC]
    public void ChangeAnimationState(string newState) {
        if(currentState == newState) return;

        animator.Play(newState);

        currentState = newState;
    }
}
