using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class PauseMenuManager : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject pauseButton;


    public static bool GameisPaused = false;

    //audio:
    public AudioMixer mixer;
    public static float volumeLevel = 1.0f;
    private Slider sliderVolumeCtrl;


    void Awake()
    {
        pauseMenu.SetActive(true); // so slider can be set
        SetLevel(volumeLevel);
        GameObject sliderTemp = GameObject.FindWithTag("PauseMenuSlider");
        if (sliderTemp != null)
        {
            sliderVolumeCtrl = sliderTemp.GetComponent<Slider>();
            sliderVolumeCtrl.value = volumeLevel;
        }
    }

    void Start(){
        pauseButton.SetActive(true);
        pauseMenu.SetActive(false);
        GameisPaused = false;

    }

    void Update(){
        if (Input.GetKeyDown(KeyCode.Escape)){
            if (GameisPaused){
                Resume();
            }
            else{
                Pause();
            }
        }
    }

    public void Pause()
    {
        pauseButton.SetActive(false);
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        GameisPaused = true;
    }

    public void RestartCurrentLevel(){
        if(LoseScreen.previousScene == SceneManager.GetActiveScene().name){
            Resume();
            SceneManager.LoadScene(LoseScreen.previousScene);
        }
    }

    public void Resume()
    {
        pauseButton.SetActive(true);
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        GameisPaused = false;
    }

    public void SetLevel(float sliderValue)
    {
        mixer.SetFloat("MusicVolume", Mathf.Log10(sliderValue) * 20);
        volumeLevel = sliderValue;
    }

    public void ReturnToMainMenu() {
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
