using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorLevelChange : MonoBehaviour{


        public string NextLevel = "MainMenu";
        public GameObject msgPressE;
        public bool canPressE =false;

       void Start(){
              msgPressE.SetActive(false);
        }

       void Update(){
              if ((canPressE == true) && (Input.GetKeyDown(KeyCode.E))){
                     EnterDoor();
              }
        }

        void OnTriggerEnter2D(Collider2D other){
              if ((other.gameObject.tag == "Plant") || (other.gameObject.tag == "Robot")){ ;
                     msgPressE.SetActive(true);
                     canPressE =true;
              }
        }

        void OnTriggerExit2D(Collider2D other){
              if ((other.gameObject.tag == "Plant") || (other.gameObject.tag == "Robot")){
                     msgPressE.SetActive(false);
                     canPressE = false;
              }
        }

      public void EnterDoor(){
            SceneManager.LoadScene (NextLevel);
      }

}