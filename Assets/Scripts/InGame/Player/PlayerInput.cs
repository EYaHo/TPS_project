using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerInput : MonoBehaviourPun
{
    public string verticalRotateAxisName = "Mouse X";
    public string horizontalRotateAxisName = "Mouse Y";
    public string verticalMoveAxisName = "Vertical";
    public string horizontalMoveAxisName = "Horizontal";
    public string rotateAxisName = "Horizontal";
    public string jumpButtonName = "Jump";
    public string fireButtonName = "Fire1";
    public string interactButtonName = "Interact";

    public float verticalRotate { get; private set; }
    public float horizontalRotate { get; private set; }
    public float verticalMove { get; private set; }
    public float horizontalMove { get; private set; }
    public float rotate { get; private set; }
    public bool jump { get; private set; }
    public bool fire { get; private set; }
    public bool interact { get; private set; }

    // Update is called once per frame
    void Update()
    {
        if(!photonView.IsMine) {
            return;
        }

        if(GameManager.Instance != null && GameManager.Instance.isGameover) {
            verticalMove = 0;
            horizontalMove = 0;
            rotate = 0;
            jump = false;
            fire = false;
            return;
        }

        verticalRotate = Input.GetAxis(verticalRotateAxisName);
        horizontalRotate = Input.GetAxis(horizontalRotateAxisName);
        verticalMove = Input.GetAxis(verticalMoveAxisName);
        horizontalMove = Input.GetAxis(horizontalMoveAxisName);
        rotate = Input.GetAxis(rotateAxisName);
        jump = Input.GetButtonDown(jumpButtonName);
        fire = Input.GetButton(fireButtonName);
        interact = Input.GetButtonDown(interactButtonName);
    }
}
