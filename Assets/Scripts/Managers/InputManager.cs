using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    PlayerInput playerInput;

    [SerializeField] Player player;

    private void Awake()
    {
        playerInput = new PlayerInput();

        playerInput.Gameplay.Movement.performed += ctx => player.inputDir = ctx.ReadValue<Vector2>();
        playerInput.Gameplay.Movement.canceled += ctx => player.inputDir = Vector2.zero;
    }

    private void OnEnable()
    {
        playerInput.Gameplay.Enable();
    }

    private void OnDisable()
    {
        playerInput.Gameplay.Disable();
    }
}
