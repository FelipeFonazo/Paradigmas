using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class appleBehavior : MonoBehaviour {

    #region 🍏 CONFIGURAÇÃO DA MAÇA
    [SerializeField] private float maxBodySnake = 100;
    [SerializeField] private float growCoin = 4;
    [SerializeField] private int indicadorMoeda = 0;
    [SerializeField] private float valorMoeda = 0f;
    #endregion

    #region Referências
    private SpriteRenderer spriteRenderer;
    private CircleCollider2D circleCollider;
    private AppleSpawner appleSpawner;
    private PlayerBehavior playerBehavior;
    private GameManager GameManager;
    public GameObject Collected;
    #endregion


    private void Awake(){
        spriteRenderer = GetComponent<SpriteRenderer>();
        circleCollider = GetComponent<CircleCollider2D>();
        appleSpawner = FindObjectOfType<AppleSpawner>();
        playerBehavior = FindObjectOfType<PlayerBehavior>();
    }

    private void OnTriggerEnter2D(Collider2D collider) {
        if (!collider.CompareTag("Player")) return;
        spriteRenderer.enabled = false;
        circleCollider.enabled = false;

        GameObject collectedEffect = Instantiate(Collected, transform.position, Quaternion.identity);
        Destroy(collectedEffect, 0.4f);

        GameManager = GameManager.Instance;
        GameManager.UpScore(valorMoeda, indicadorMoeda);

        appleSpawner.SpawnApple();
        Destroy(gameObject, 0.4f);

        int moeda = (int)GameManager.coinsFases[indicadorMoeda];
        if (moeda % growCoin == 0 && moeda < maxBodySnake * growCoin) {
            playerBehavior.initialBodyParts++;
            playerBehavior.growSnake();
        }
    }
}
