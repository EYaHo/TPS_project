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
}
