using System;
using Player;
using UnityEngine;
using UnityEngine.InputSystem;

public class MobileInputController : MonoBehaviour
{
    private PlayerController _player;

    private void Awake()
    {
        _player = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<PlayerController>();
        
    }

    public void OnUpRegion(InputAction.CallbackContext context)
    {
        // Make the player go up
        _player.MovePlayer(context);
    }

    public void OnDownRegion(InputAction.CallbackContext context)
    {
        // Make the player go down
        _player.MovePlayer(context);
    }

    public void OnSwipe()
    {
        // Make the player throw the payload
        
    }
}
