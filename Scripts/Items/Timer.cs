using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class Timer : MonoBehaviour{
    
    [SerializeField] Text timerText;
    [SerializeField] PlayerBehavior playerBehavior;
    [SerializeField] AppleSpawner appleSpawner;
    [SerializeField] private float nextPhase = 1;

    public string typeGame = "";

    private float timerAcumulado = 0;
    public float timer = 60;
    private int count;

    void Start(){ }

    void Update(){
        if(timer > 1){
            timer -= Time.deltaTime;
            int segundos = Mathf.FloorToInt(timer % 60);
            int minutos = Mathf.FloorToInt(timer / 60); 

            timerText.text = string.Format("{0:00}:{1:00}", minutos, segundos);
            timerAcumulado += Time.deltaTime;

            if(timerAcumulado >= 1f){
                if(playerBehavior.speed <= 8) playerBehavior.increaseSpeed(playerBehavior.getSpeedPerTime());

                timerAcumulado -=1f;
                count++;

                if( count == 4){
                    appleSpawner.SpawnApple();
                    count = 0;
                }
            }

        }else if(typeGame == "Fases"){
            GameManager gameManager = GameObject.Find("GameManager")?.GetComponent<GameManager>();
            gameManager.LoadLastScene("Vitoria");

        }else{
            if(nextPhase == 10){
                GameManager gameManager = GameObject.Find("GameManager")?.GetComponent<GameManager>();
                gameManager.LoadLastScene("Vitoria");

            }else{
                timerText.text = string.Format("{0:00}:{1:00}", 0, 0);
                NextPhaseFunction();
            }
           
        }
    }


    public void NextPhaseFunction(){
        StartCoroutine(Transition());
    }

    IEnumerator Transition(){
        Time.timeScale = 0.05f;
        yield return StartCoroutine(FadeToBlack(0.5f));
        yield return new WaitForSecondsRealtime(0.1f);
        Time.timeScale = 1f;
        SceneManager.LoadScene("Fase_" + nextPhase); 

    }

    IEnumerator FadeToBlack(float duration){
        Image fadeImage = GameObject.Find("FadeImage")?.GetComponent<Image>();
        
        if(fadeImage){
            Color color = fadeImage.color;
            float startAlpha = color.a;
            float time = 0f;

            while (time < duration){

                if (fadeImage == null) yield break;

                float t = time / duration;
                color.a = Mathf.Lerp(startAlpha, 1f, t);
                fadeImage.color = color;
                time += Time.unscaledDeltaTime;
                yield return null;
            }

            // Garante alpha 1 no final
            color.a = 1f;
            fadeImage.color = color;
        }
    }
}
