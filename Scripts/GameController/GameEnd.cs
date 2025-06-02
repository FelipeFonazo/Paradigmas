using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameEnd : MonoBehaviour{

    [SerializeField] private Text scoreFinal;
    GameManager gameManager;

    public void Awake(){ }

    void Update(){ }

    public void LoadScenes(){

        if(gameManager != null){
            for(int i = 0; i <  6; i++){
                gameManager.coinsFases[i] = 0f;
                gameManager.scores[i] = null;
            }

            gameManager.lastScene = "";
            gameManager.typeGame = "";
            gameManager.numberScene = 0;
            gameManager.faseCount = 0;
            gameManager.countAnimation = 0;

            SceneManager.LoadScene("Menu"); 
        }
    }



    private void OnEnable(){
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable(){
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode){
        gameManager = GameObject.Find("GameManager")?.GetComponent<GameManager>();
        float conversaoMoedas = 0f;

        for(int i = 0; i < 6; i++){
            switch (i){
                case 0:
                    conversaoMoedas += gameManager.coinsFases[i] * 0.039f;
                    break;
                
                case 1:
                    conversaoMoedas += gameManager.coinsFases[i] * 0.79f;
                    break;

                case 2:
                    conversaoMoedas += gameManager.coinsFases[i] * 1;
                    break;

                case 3:
                    conversaoMoedas += gameManager.coinsFases[i] * 5.67f;
                    break;

                case 4:
                    conversaoMoedas += gameManager.coinsFases[i] * 6.44f;
                    break;

                case 5:
                    conversaoMoedas += gameManager.coinsFases[i] * 621561;
                    break;
            }
        }

        scoreFinal.text = conversaoMoedas.ToString();

        if (gameManager.typeGame == "Fases"){
            Text newScore = GameObject.Find("Score_" + gameManager.numberScene )?.GetComponent<Text>();
            GameObject moedaMap = GameObject.Find("Moeda_" + gameManager.numberScene);

            newScore.text = "x " + gameManager.coinsFases[gameManager.numberScene].ToString();       

            Vector3 positionMoeda; 
            positionMoeda = (scene.name == "Game_Over") ? new Vector3(-0.666f, -3.354f, 0) : new Vector3(-1.03f, -3.72f, 0);

            if (moedaMap != null) moedaMap.transform.position = positionMoeda;
    
            Vector2 positionTextMoeda;
            positionTextMoeda = (scene.name == "Game_Over") ? new Vector2(60,-381) : new Vector2(0, -422f);

            RectTransform scoreRect = newScore.GetComponent<RectTransform>();
            if (positionTextMoeda != null) scoreRect.anchoredPosition = positionTextMoeda;
            
            
            for(int i = 0; i < 6; i++){
                GameObject texto = GameObject.Find("Score_" + i);
                GameObject moeda = GameObject.Find("Moeda_" + i);

                if(i != gameManager.numberScene){
                    Destroy(texto);
                    Destroy(moeda);
                
                }
            } // 90 por 120

        }else{
            for(int i = 0; i < 6; i++){
                GameObject texto = GameObject.Find("Score_" + i);
                GameObject moeda = GameObject.Find("Moeda_" + i);

                if(i >= gameManager.faseCount){
                    Destroy(texto);
                    Destroy(moeda);
                
                }else{
                    texto.GetComponent<Text>().text = "x " + gameManager.coinsFases[i].ToString();
                }
            }
        }

    }
}
