using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class InteractableObject : MonoBehaviourPun
{
    protected string interactString = "";

    public string GetInteractString() {
        return interactString;
    }

    public virtual void Interact() {
        
    }

    public void DestoryInNetwork() {
    // 모든 네트워크에서 이 오브젝트 제거
        photonView.RPC("RpcDestroy", RpcTarget.MasterClient);
    }

    [PunRPC]
    public void RpcDestroy() {
        PhotonNetwork.Destroy(this.gameObject);
    }
}
