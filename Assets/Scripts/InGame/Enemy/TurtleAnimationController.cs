using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurtleAnimationController : AnimationController
{
    public enum AnimState {
        IdleNormal, //0
        IdleBattle, //1
        Attack01,   //2
        Attack02,   //3
        WalkFWD,    //4
        RunFWD,     //5
        SenseSomethingST,   //6
        SenseSomethingRPT,  //7
        Taunt,      //8
        Victory,    //9
        GetHit,     //10
        Dizzy,      //11
        Die         //12
    }
    
    public override void PlayDieAnimation() {
        ChangeAnimationState(AnimState.Die.ToString());
    }
    public override void PlayAttackAnimation(int attackType) {
        switch(attackType) {
            case 1:
                ChangeAnimationState_Once(AnimState.Attack01.ToString(), AnimState.IdleBattle.ToString());
                break;
            case 2:
                ChangeAnimationState_Once(AnimState.Attack02.ToString(), AnimState.IdleBattle.ToString());
                break;
        }
    }
    public override void PlayWalkFWDAnimation() {
        ChangeAnimationState(AnimState.WalkFWD.ToString());
    }
}
