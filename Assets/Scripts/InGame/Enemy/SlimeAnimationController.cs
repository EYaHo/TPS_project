using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeAnimationController : AnimationController
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
}
