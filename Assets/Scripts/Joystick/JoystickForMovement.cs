using System;
using UnityEngine;

public class JoystickForMovement : JoystickHandler
{
    [SerializeField] private CharacterMovement characterMovement;

    /// <summary>
    /// Направление движения
    /// </summary>
    /// <returns></returns>
    private void Update()
    {
        if (characterMovement == null)
            throw new NullReferenceException(nameof(characterMovement));

        var moveDirection = InputVector.x != 0 || InputVector.y != 0 // if joystick
            ? new Vector3(InputVector.x, 0, InputVector.y) // joystick
            : new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")); // WASD
        characterMovement.MoveCharacter(moveDirection);
        characterMovement.RotateCharacter(moveDirection);
    }
}
