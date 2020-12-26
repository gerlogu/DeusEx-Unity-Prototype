using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InGameUIController : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject crosshair;

    public Menu menu = Menu.NONE;

    private GameManager gameManager;

    public enum Menu
    {
        NONE   = 0,
        START  = 1,
        SELECT = 2
    }

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    private void Update()
    {
        #region Start Menu
        if (Input.GetButtonDown("Start"))
        {
            if (menu.Equals(Menu.START))
            {
                FindObjectOfType<PlayerMovementOld>().fov = FindObjectOfType<CameraFOVController>().fieldOfView;
                menu = Menu.NONE;
                
            }
            else if(menu.Equals(Menu.NONE))
            {
                menu = Menu.START;
            }
        }
        #endregion

        switch (menu)
        {
            case Menu.START:
                if (!pauseMenu.activeSelf)
                {
                    pauseMenu.SetActive(true);
                }
                if (pauseMenu.activeSelf)
                {
                    if (crosshair)
                        crosshair.SetActive(false);
                }
                if(!gameManager.CheckPlayerState(1))
                {
                    gameManager.SetPlayerState(1);
                }
                if(Time.timeScale != 0)
                    Time.timeScale = 0;
                gameManager.ShowCursor();
                break;
            case Menu.SELECT:
                break;
            case Menu.NONE:
                if (pauseMenu.activeSelf)
                {
                    pauseMenu.SetActive(false);
                }
                if (!pauseMenu.activeSelf)
                {
                    if(crosshair)
                        crosshair.SetActive(true);
                }
                if (!gameManager.CheckPlayerState(0))
                {
                    gameManager.SetPlayerState(0);
                }
                if (Time.timeScale == 0)
                    Time.timeScale = 1;
                gameManager.HideCursor();
                break;
            default:
                break;
        }


        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            SceneManager.LoadScene(0);
        }
    }
}
