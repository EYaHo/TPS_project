using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerInput : MonoBehaviourPun
{
    public string verticalMoveAxisName = "Vertical";
    public string horizontalMoveAxisName = "Horizontal";
    public string rotateAxisName = "Horizontal";
    public string jumpButtonName = "Jump";
    public string fireButtonName = "Fire1";

    public float verticalMove { get; private set; }
    public float horizontalMove { get; private set; }
    public float rotate { get; private set; }
    public bool jump { get; private set; }
    public bool fire { get; private set; }

    // Update is called once per frame
    void Update()
    {
        if(!photonView.IsMine) {
            return;
        }

        verticalMove = Input.GetAxis(verticalMoveAxisName);
        horizontalMove = Input.GetAxis(horizontalMoveAxisName);
        rotate = Input.GetAxis(rotateAxisName);
        jump = Input.GetButton(jumpButtonName);
        fire = Input.GetButton(fireButtonName);
    }
}
