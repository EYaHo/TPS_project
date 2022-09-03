using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class AnimationController : MonoBehaviourPun
{
    public Animator animator { get; private set; }
    public List<AnimatorStateInfo> animationForPlayingOnce;
    protected string currentState;

    protected void Awake() {
        animator = GetComponent<Animator>();
    }

    [PunRPC]
    public void RpcChangeAnimationState(string newState) {
        //if(currentState == newState) return;

        animator.Play(newState);

        currentState = newState;
    }

    public void ChangeAnimationState(string newState) {
        if(currentState == newState) return;
        photonView.RPC("RpcChangeAnimationState", RpcTarget.All, newState);
    }

    IEnumerator WaitForAnimation(string newState, float ratio, bool play)
	{
		// if(play) {
        //     animator.Play(newState);
        //     Debug.Log("play " + newState);
        // }

        var anim = animator.GetCurrentAnimatorStateInfo(0);
        Debug.Log(anim.shortNameHash);
        var clip = animator.GetCurrentAnimatorClipInfo(0)[0].clip;
        Debug.Log(clip);
		
		while(anim.normalizedTime + float.Epsilon + Time.deltaTime < ratio)
			yield return new WaitForEndOfFrame();
    }
    IEnumerator Play_Once(string newState, string nextState)
	{
        animator.Play(newState);
        currentState = newState;

		yield return new WaitForSeconds(0.79f);

        // var clip = animator.GetCurrentAnimatorClipInfo(0)[0].clip;
        // Debug.Log(clip);

        // var anim = animator.GetCurrentAnimatorStateInfo(0);
        // Debug.Log(anim.shortNameHash);

        //yield return StartCoroutine(WaitForAnimation(newState, 1.0f, true));
        
        animator.Play(nextState);
        currentState = nextState;
	}

    public void ChangeAnimationState(string newState, string nextState) {
        if(currentState == newState) return;
        photonView.RPC("RpcChangeAnimationState", RpcTarget.All, newState, nextState);
    }
    [PunRPC]
    public void RpcChangeAnimationState(string newState, string nextState) {
        //if(currentState == newState) return;
        StartCoroutine(Play_Once(newState, nextState));
    }

    public virtual void PlayDieAnimation() {

    }
}
