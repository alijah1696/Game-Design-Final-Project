using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenutest : MonoBehaviour
{
    public GameObject Pause_Menu;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    public void Pause()
    {
        Pause_Menu.SetActive(true);
        Time.timeScale = 0;
        Debug.Log(Time.timeScale);
    }
   
    public void Continue()
    {
        Debug.Log("RESUME!");
        Pause_Menu.SetActive(false);
        Time.timeScale = 1;
    }
    public void QuitGame()
    {
        Debug.Log("QUIT!");
        Application.Quit();
    }
}
