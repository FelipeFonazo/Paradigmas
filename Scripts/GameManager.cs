using UnityEngine;
using UnityEngine.UI;
using System.Collections;

using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour{
    public static GameManager Instance;

    public InputManager InputManager { get; private set; }

    public float[] coinsFases = new float[6]; // salva a moeda em um indice em cada fase.

    public Text[] scores = new Text[6];
    public Text scoreSingle;

    public int numberScene;
    public string lastScene = "";
    private string sceneEnd;

    // audios - fase beta

    private AudioSource audioSource;
    
    public AudioClip SongMenu;
    public AudioClip SongGame;
    public AudioClip SongTelaVitoria;
    public AudioClip SongTelaDerrota;
    public GameObject Deathmoment;
    public GameObject VictoryMoment;

    private int SongCount;
    // gameOver variaveis necessarias
    public string typeGame = "";
    public Image fadeImage;
    public int faseCount;
    public int countAnimation = 0;

    private void Awake(){
        this.audioSource = GetComponent<AudioSource>();

        this.SongCount = 0;
        this.audioSource.volume = 0.5f;
        this.audioSource.loop = true;

        this.faseCount = 0;

        if(Instance != null){
            Destroy(this.gameObject);
            return;
        }

        for(int i = 0; i < 6; i++){
            coinsFases[i] = 0f;
        }

        Instance = this;
        InputManager = new InputManager();
        DontDestroyOnLoad(this.gameObject); // impede a destruicao desse objeto ao trocar de cena.

    }

    public void LoadLastScene(string scene) {
        sceneEnd = scene;
        this.lastScene = "";
        audioSource.Stop();

        if (scene == "Game_Over" && countAnimation == 0) {
            GameObject death = Instantiate(Deathmoment, transform.position, Quaternion.identity);
            Destroy(death, 0.5f);
            countAnimation++;
            Invoke(nameof(LoadSceneGameOver), 0.7f);
        } else if(scene == "Vitoria" && countAnimation == 0){
            //GameObject victory = Instantiate(VictoryMoment, transform.position, Quaternion.identity);
            //Destroy(victory, 0.5f);

            // ATIVAR O CODIGO ACIMA QUANDO COLOCAR SOM PARA INDICAR VITORIA

            countAnimation++;
            Invoke(nameof(LoadSceneGameOver), 0.7f);

        }
    }

    private void LoadSceneGameOver() {
        SceneManager.LoadScene(sceneEnd);
    }

    IEnumerator FadeTo(float from, float to, float duration){
        Image fadeImage = GameObject.Find("FadeImage")?.GetComponent<Image>();
        
        if (fadeImage){
            Color color = fadeImage.color;
            float time = 0f;

            while (time < duration){

                if (fadeImage == null) yield break;

                float t = time / duration;
                color.a = Mathf.Lerp(from, to, t);
                fadeImage.color = color;
                time += Time.unscaledDeltaTime;
                yield return null;
            }

            color.a = to;
            fadeImage.color = color;
        }   
    }

    public void UpScore(float score, int indicador){
        coinsFases[indicador] += score; 

        if(lastScene == "Fases"){
            scoreSingle.text = "X " + coinsFases[indicador];

        }else{
            for(int i = 0; i <= indicador; i++){
                scores[indicador].text = "X " + coinsFases[indicador];
            }
        }

    }

    // FUNCOES QUE CAPTAM QUANDO A CENA É CARREGADA ( USADA PARA SABER QUANDO PODE PEGAR OS OBJETOS DA CENA )

    private void OnEnable(){
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable(){
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode){
        if(scene.name != "Menu" && scene.name != "Fases" && scene.name != "Game_Over") StartCoroutine(FadeTo(1f, 0f, 1.5f));

        if (lastScene == "Fases"){
            Text newScore = GameObject.Find("Score_" + numberScene )?.GetComponent<Text>();
            Timer timer = GameObject.Find("Timer")?.GetComponent<Timer>();
            GameObject moedaMap = GameObject.Find("Moeda_" + numberScene);


            Vector3 positionMoeda = new Vector3(-7.01f, 4, 0); 
            moedaMap.transform.position = positionMoeda; 

    
            Vector2 positionTextMoeda = new Vector2(-703, 430); 
            RectTransform scoreRect = newScore.GetComponent<RectTransform>();
            scoreRect.anchoredPosition = positionTextMoeda;


            timer.timer = 10f;
            timer.typeGame = "Fases";
            scoreSingle = newScore;
            
            for(int i = 0; i < 6; i++){
                GameObject texto = GameObject.Find("Score_" + i);
                GameObject moeda = GameObject.Find("Moeda_" + i);

                if(texto != null && i != numberScene){
                    Destroy(texto);
                    Destroy(moeda);
                }
            }

        }else if(lastScene != "Fases" && lastScene != "theEnd"){
            for (int i = 0; i < 6; i++) {
                Text newScore = GameObject.Find("Score_" + i)?.GetComponent<Text>();

                if(newScore != null) {
                    scores[i] = newScore;
                    scores[i].text = "X " + coinsFases[i];
                }
            }
        }



        if(scene.name == "Menu"){
            audioSource.clip = SongMenu;
            SongCount = 0;
            audioSource.Play();


        }else if(scene.name.Contains("Fase_")){
            faseCount++;

            if(SongCount == 0 ){
                audioSource.clip = SongGame;
                SongCount++;
                audioSource.Play();
            }
            
        }else if(scene.name == "Game_Over"){
            audioSource.clip = SongTelaDerrota;
            audioSource.Play();
        
        }else if(scene.name == "Vitoria"){
            audioSource.clip = SongTelaVitoria;
            audioSource.Play();
        }


    }   
    
}
