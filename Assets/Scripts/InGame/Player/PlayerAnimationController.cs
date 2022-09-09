using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerAnimationController : AnimationController
{
    public enum AnimState {
        Idle_Guard_AR,
        Run_guard_AR,
        WalkFront_Shoot_AR,
        WalkBack_Shoot_AR,
        WalkLeft_Shoot_AR,
        WalkRight_Shoot_AR,
        Jump,
        Die,
        Idle_gunMiddle_AR,
        Shoot_SingleShot_AR,
        Shoot_BurstShot_AR,
        Shoot_Autoshot_AR,
        Reload
    }

    private string currentState_second;
/*
    [PunRPC]
    public void ChangeAnimationState(string newState, int layer, float normalizedTime) { // layer = -1: base layer, layer = 1: upper layer
        if(layer == -1) {
            if(currentState == newState) return;
            else currentState = newState;
        } else { // layer == 1
            if(currentState_second == newState) return;
            else currentState_second = newState;
        }

        animator.Play(newState, layer, normalizedTime);
    }
*/
    [PunRPC]
    public void RpcChangeAnimationState(string newState, int layer, float normalizedTime) { // layer = -1: base layer, layer = 1: upper layer
        if(layer == -1) {
            currentState = newState;
        } else { // layer == 1
            currentState_second = newState;
        }

        animator.Play(newState, layer, normalizedTime);
    }
    public void ChangeAnimationState(string newState, int layer, float normalizedTime) {
        if(layer == -1) {
            if(currentState == newState) return;
        } else { // layer == 1
            if(currentState_second == newState) return;
        }
        photonView.RPC("RpcChangeAnimationState", RpcTarget.AllBuffered, newState, layer, normalizedTime);
        
    }

    [PunRPC]
    public void ChangeAnimationState(string newState, int layer, float normalizedTime, bool init) { // init = true : 같은 애니메이션이 들어올 때도 애니메이션을 처음부터 다시 재생.
        if(layer == -1) {
            if(!init) {
                if(currentState == newState) return;
                else currentState = newState;
            }
        } else { // layer == 1
            if(!init) {
                if(currentState_second == newState) return;
                else currentState_second = newState;
            }
        }

        animator.Play(newState, layer, normalizedTime);
    }

    public override void PlayDieAnimation() {
        ChangeAnimationState(AnimState.Die.ToString(), -1, 0f);
        ChangeAnimationState(AnimState.Die.ToString(), 1, 0f);
    }
    public override void PlayAttackAnimation(int attackType) {
        switch(attackType) {
            case 1:
                ChangeAnimationState(AnimState.Shoot_SingleShot_AR.ToString(), 1, 0f);
                break;
            case 2:
                ChangeAnimationState(AnimState.Shoot_BurstShot_AR.ToString(), 1, 0f);
                break;
            case 3:
                ChangeAnimationState(AnimState.Shoot_Autoshot_AR.ToString(), 1, 0f);
                break;
        }
    }

    public void PlayIdleAnimation(int layer) {
        ChangeAnimationState(AnimState.Idle_gunMiddle_AR.ToString(), layer, 0f);
    }
    public void PlayWalkAnimation(string walkDirection) {
        switch(walkDirection) {
            case "front":
                ChangeAnimationState(AnimState.WalkFront_Shoot_AR.ToString(), -1, 0f);
                break;
            case "back":
                ChangeAnimationState(AnimState.WalkBack_Shoot_AR.ToString(), -1, 0f);
                break;
            case "left":
                ChangeAnimationState(AnimState.WalkLeft_Shoot_AR.ToString(), -1, 0f);
                break;
            case "right":
                ChangeAnimationState(AnimState.WalkRight_Shoot_AR.ToString(), -1, 0f);
                break;
        }
    }
}
