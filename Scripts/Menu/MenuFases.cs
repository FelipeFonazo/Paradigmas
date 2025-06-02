using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuFases : MonoBehaviour
{
    public void LoadScenes(int number){
        string cena = "Fase_" + (number + 1);

        GameManager gameManager = GameObject.Find("GameManager")?.GetComponent<GameManager>();

        if(gameManager != null){
            gameManager.lastScene = SceneManager.GetActiveScene().name;
            gameManager.numberScene = number;
            gameManager.typeGame = "Fases";
        }else{
            Debug.Log("NAO FOI");
        }
        
        SceneManager.LoadScene(cena);   
    }

    public void Back(){
        SceneManager.LoadScene("Menu");   

    }

}
