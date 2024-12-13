using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoseScreen : MonoBehaviour
{   

    public static string previousScene;
    
    // Start is called before the first frame update
    void Start()
    {   
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RestartLastLevel(){
        if(previousScene != null) SceneManager.LoadScene(previousScene);
    }

    public void GoToMainMenu(){
        SceneManager.LoadScene("Main Menu");
    }
}
