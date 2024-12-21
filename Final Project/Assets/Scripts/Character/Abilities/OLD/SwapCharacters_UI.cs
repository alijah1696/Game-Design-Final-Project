using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwapCharacters_UI : MonoBehaviour
{
    public GameObject player;

    public GameObject plantChar_grayIcon;
    public GameObject robotChar_grayIcon;

    public GameObject plantChar_state1;
    public GameObject plantChar_state2;
    public GameObject plantChar_state3;

    public GameObject robotChar_state1;
    public GameObject robotChar_state2;
    public GameObject robotChar_state3;

    public bool isRobotFirst = true;


    // Start is called before the first frame update
    void Start(){
        player = GameObject.FindWithTag("Player");
        if (isRobotFirst == true) {
            //set plant icons inactive:
            plantChar_grayIcon.SetActive(true);
            plantChar_state1.SetActive(false);
            plantChar_state2.SetActive(false);
            plantChar_state3.SetActive(false);

            //set robot icons active:
            robotChar_grayIcon.SetActive(false);
            robotChar_state1.SetActive(true);
            robotChar_state2.SetActive(true);
            robotChar_state3.SetActive(true);
        } else {
            //set plant icons inactive:
            plantChar_grayIcon.SetActive(false);
            plantChar_state1.SetActive(true);
            plantChar_state2.SetActive(true);
            plantChar_state3.SetActive(true);

            //set robot icons active:
            robotChar_grayIcon.SetActive(true);
            robotChar_state1.SetActive(false);
            robotChar_state2.SetActive(false);
            robotChar_state3.SetActive(false);
        }
        

    }   


    public void ActivateRobot() {
        //set plant icons inactive:
        plantChar_grayIcon.SetActive(true);
        plantChar_state1.SetActive(false);
        plantChar_state2.SetActive(false);
        plantChar_state3.SetActive(false);

        //set robot icons active:
        robotChar_grayIcon.SetActive(false);
        robotChar_state1.SetActive(true);
        robotChar_state2.SetActive(true);
        robotChar_state3.SetActive(true);

        player.GetComponent<SwapCharacters>().SwapCharacter();
    }

    public void ActivatePlant(){
        //set plant icons inactive:
        plantChar_grayIcon.SetActive(false);
        plantChar_state1.SetActive(true);
        plantChar_state2.SetActive(true);
        plantChar_state3.SetActive(true);

        //set robot icons active:
        robotChar_grayIcon.SetActive(true);
        robotChar_state1.SetActive(false);
        robotChar_state2.SetActive(false);
        robotChar_state3.SetActive(false);

        player.GetComponent<SwapCharacters>().SwapCharacter();
    }

    public void plant_state1() {
        player.GetComponent<SwapCharacters>().SetPlantForm1();
    }
    public void plant_state2(){
        player.GetComponent<SwapCharacters>().SetPlantForm2();
    }
    public void plant_state3(){
        player.GetComponent<SwapCharacters>().SetPlantForm3();
    }

    public void robot_state1(){
        player.GetComponent<SwapCharacters>().SetRobotForm2();
    }
    public void robot_state2(){
        player.GetComponent<SwapCharacters>().SetRobotForm1();
    }
    public void robot_state3(){
        player.GetComponent<SwapCharacters>().SetRobotForm3();
    }

}
