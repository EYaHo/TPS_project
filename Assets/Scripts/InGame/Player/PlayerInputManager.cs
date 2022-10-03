using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputManager : MonoBehaviour
{
    public Vector2 move { get; private set; }
    public Vector2 look { get; private set; }
    public bool jump { get; private set; }
    public bool fire { get; private set; }
    public bool interact { get; private set; }

    public void OnMove(InputValue value) {
        move = value.Get<Vector2>().normalized;
    }

    public void OnLook(InputValue value) {
        look = value.Get<Vector2>();
    }

    public void OnJump(InputValue value) {
        jump = value.isPressed;
    }

    public void OnFire(InputValue value) {
        fire = value.isPressed;
    }

    public void OnInteract(InputValue value) {
        interact = value.isPressed;
    }
}
