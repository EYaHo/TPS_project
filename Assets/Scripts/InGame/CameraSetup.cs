using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Photon.Pun;

public class CameraSetup : MonoBehaviourPun
{
    public CinemachineVirtualCamera followCam;
    private void Awake() {
        if(photonView.IsMine) {
            followCam = FindObjectOfType<CinemachineVirtualCamera>();
            Transform target = transform.GetComponent<PlayerMovement>().targetOfCam;
            followCam.Follow = target;
            followCam.LookAt = target;
        }
    }
    void Start()
    {
        
    }
}
