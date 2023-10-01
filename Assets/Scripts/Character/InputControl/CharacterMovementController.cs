using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterMovementController"
    , menuName = "InputController/CharacterMovementController")]
public class CharacterMovementController : InputController
{
    public override bool RetrieveJumpInput()
    {
        return Input.GetButtonDown("Jump");
    }
    public override float RetrieveMoveInput()
    {
        return Input.GetAxisRaw("Horizontal");
    }
    public override bool RetrieveJumpHoldInput()
    {
        return Input.GetButton("Jump");
    }
}
