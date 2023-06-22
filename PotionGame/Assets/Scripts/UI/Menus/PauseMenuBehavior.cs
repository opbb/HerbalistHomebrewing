using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuBehavior : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject controlsMenu;
    public bool isPaused = false;
    private UIController controller;
    private GameObject player; 

    private void Start()
    {
        MouseLook.isUIActive = false;
        player = GameObject.FindGameObjectWithTag("Player");
        controller = player.GetComponent<UIController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void PauseGame()
    {
        if (MouseLook.isUIActive == false && !controller.isUIActive())
        {
            isPaused = true;

            Time.timeScale = 0f;
            pauseMenu.SetActive(true);

            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }

    public void ResumeGame()
    {
        isPaused = false;
        MouseLook.isUIActive = false;

        Time.timeScale = 1f;
        pauseMenu.SetActive(false);

        controlsMenu.SetActive(false);

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
