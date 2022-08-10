using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class AnimationController : MonoBehaviourPun
{
    public Animator animator { get; private set; }
    protected string currentState;

    protected void Awake() {
        animator = GetComponent<Animator>();
    }

    [PunRPC]
    public void ChangeAnimationState(string newState) {
        if(currentState == newState) return;

        animator.Play(newState);

        currentState = newState;
    }
}
