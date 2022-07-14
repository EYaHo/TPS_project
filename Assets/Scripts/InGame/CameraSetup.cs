using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Photon.Pun;

public class CameraSetup : MonoBehaviourPun
{
    void Start()
    {
        if(photonView.IsMine) {
            CinemachineVirtualCamera followCam = FindObjectOfType<CinemachineVirtualCamera>();
            Transform target = transform.GetComponent<PlayerMovement>().targetOfCam;
            followCam.Follow = target;
            followCam.LookAt = target;
        }
    }
}
