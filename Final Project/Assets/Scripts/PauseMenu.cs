using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject pauseButton;
    bool isPaused = false;

    private void Start(){
        pauseMenu.SetActive(false);
        pauseButton.SetActive(true);
    }

    void Update(){
        if (Input.GetKeyDown(KeyCode.Escape)){
            //if (pauseMenu.activeInHierarchy){
            if (isPaused) { 
                ResumeGame();
            }
            else{
                PauseGame();
            }
        }
    }

    public void PauseGame(){
        pauseMenu.SetActive(true);
        pauseButton.SetActive(false);
        Time.timeScale = 0.0f;
        isPaused = true;
    }

    public void ResumeGame(){
        pauseMenu.SetActive(false);
        pauseButton.SetActive(true);
        Time.timeScale = 1.0f;
        isPaused = false;
    }
    public void BacktoMenu(){
        SceneManager.LoadScene("Main Menu");
    }

    public void QuitGame(){
        Debug.Log("QUIT!");
        #if UNITY_STANDALONE
        Application.Quit();
        #elif UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }


}