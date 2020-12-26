using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState
{
    IN_GAME = 0,
    IN_MENU = 1
}

public class GameManager : MonoBehaviour
{

    public PlayerState playerState = PlayerState.IN_GAME;

    

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    /// <summary>
    /// Shows mouse and unlocks it.
    /// </summary>
    public void ShowCursor()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    /// <summary>
    /// Hides cursor and unlocks it.
    /// </summary>
    public void HideCursor()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public bool CheckPlayerState(int index)
    {
        PlayerState stateToCheck = (PlayerState)index;
        
        if(stateToCheck == playerState)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void SetPlayerState(int index)
    {
        PlayerState newState = (PlayerState)index;
        playerState = newState;
    }
}
