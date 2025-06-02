using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CircleCollider2D))]
public class PlayerBehavior : MonoBehaviour {
    
    // VARIAVEIS
    #region 🔧 COMPONENTES E GERENCIADORES
    private Rigidbody2D rb;
    private GameManager GameManager;
    #endregion

    #region 🕹️ CONTROLE DE MOVIMENTO
    [SerializeField] private float directionCooldown = 0.12f;
    [SerializeField] public float speed = 1.5f;
    [SerializeField] private float speedPerTime = 0.1f;
    [SerializeField] private float senserMove = 0.5f;
    private Vector2 snakeDirection = Vector2.up;
    private float lastDirectionTime;
    private bool snakeMovingVertically = true;
    private bool snakeMovingHorizontally = false;
    private int walls = 6;
    #endregion

    #region 🧩 SISTEMA DO CORPO DA COBRA
    private List<Transform> bodyParts = new();
    public int initialBodyParts = 8;
    [SerializeField] private int spaceBetweenBody = 22;
    private List<Vector3> snakePositionHistory = new();
    public int delayOffset = 24;
    #endregion

    #region 🎨 PREFABS DAS PARTES DO CORPO
    [SerializeField] private GameObject bodyPrefab;
    [SerializeField] private GameObject bodyPrefabNoCollision;
    [SerializeField] private GameObject tailPrefab;
    #endregion

    #region 🍏 ELEMENTOS DO JOGO
    //[SerializeField] private AppleSpawner spawner;
    #endregion

    #region 🛡️ ESTADOS TEMPORÁRIOS
    private bool isInvincible = false;
    #endregion


    public bool canPlay = true;


    // METODOS
    #region 🔽 CICLO DE VIDA UNITY
    private void Awake() { rb = GetComponent<Rigidbody2D>(); }

    private void Start() {
        snakePositionHistory.Add(transform.position);

        for (int i = 0; i < initialBodyParts; i++)
            growSnake();

        StartCoroutine(StartInvincibility(2.5f));
    }

    private void Update() { snake(); }
    #endregion

    #region 🔼 CONTROLE DE COBRA
    public void increaseSpeed(float incrSpeed) => speed += incrSpeed;
    public float getSpeedPerTime() => speedPerTime;

    private void snake() {
        moveSnake();
        headDirection();
        UpdateBodyPositions();
    }

    private void moveSnake() {
        if (Time.time - lastDirectionTime < directionCooldown) return;

        float vertical = GameManager.Instance.InputManager.Movement_W_S;
        float horizontal = GameManager.Instance.InputManager.Movement_A_D;
        if (!canPlay) return;

        if (vertical > senserMove && snakeMovingHorizontally) {
            SetDirection(Vector2.up, true, false);
        } else if (vertical < -senserMove && snakeMovingHorizontally) {
            SetDirection(Vector2.down, true, false);
        } else if (horizontal < -senserMove && snakeMovingVertically) {
            SetDirection(Vector2.left, false, true);
        } else if (horizontal > senserMove && snakeMovingVertically) {
            SetDirection(Vector2.right, false, true);
        }
    }

    private void SetDirection(Vector2 newDirection, bool vertical, bool horizontal) {
        snakeDirection = newDirection;
        snakeMovingVertically = vertical;
        snakeMovingHorizontally = horizontal;
        lastDirectionTime = Time.time;
    }

    public void headDirection() {
        transform.position += (Vector3)(snakeDirection.normalized * speed * Time.deltaTime);

        float angle = snakeDirection == Vector2.up ? 180f :
                      snakeDirection == Vector2.down ? 0f :
                      snakeDirection == Vector2.left ? -90f : 90f;

        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    private void UpdateBodyPositions() {
        if (Vector3.Distance(snakePositionHistory[0], transform.position) > 0.01f)
            snakePositionHistory.Insert(0, transform.position);

        for (int i = 0; i < bodyParts.Count; i++) {
            int index = Mathf.Min(i * spaceBetweenBody, snakePositionHistory.Count - 1);
            bodyParts[i].position = Vector3.Lerp(
                bodyParts[i].position,
                snakePositionHistory[index],
                speed * Time.fixedDeltaTime
            );
        }

        UpdateBodyRotation();
    }

    private void UpdateBodyRotation() {
        for (int i = 0; i < bodyParts.Count; i++) {
            int index = Mathf.Min(i * spaceBetweenBody + delayOffset, snakePositionHistory.Count - 1);
            Vector3 direction = snakePositionHistory[index] - bodyParts[i].position;

            if (direction.sqrMagnitude > 0.001f) {
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 90f;
                bodyParts[i].rotation = Quaternion.Euler(0f, 0f, angle);
            }
        }
    }

    #endregion

    #region ➕ CRESCIMENTO DO CORPO
    public void growSnake() {
        GameObject newSegment;

        if (bodyParts.Count == initialBodyParts - 1) {
            Transform last = bodyParts[^1];
            bodyParts.RemoveAt(bodyParts.Count - 1);
            Destroy(last.gameObject);

            newSegment = Instantiate(bodyPrefab, bodyParts[^1].position, Quaternion.identity);
            bodyParts.Add(newSegment.transform);

            newSegment = Instantiate(tailPrefab, transform.position - (Vector3)snakeDirection, Quaternion.identity);

        } else if (bodyParts.Count > 1) {
            newSegment = Instantiate(bodyPrefab, bodyParts[^1].position, Quaternion.identity);

        } else {
            newSegment = Instantiate(bodyPrefabNoCollision, transform.position - (Vector3)snakeDirection, Quaternion.identity);
        }

        bodyParts.Add(newSegment.transform);
    }
    #endregion

    #region 🛡️ INVENCIBILIDADE TEMPORÁRIA
    private IEnumerator StartInvincibility(float duration) {
        isInvincible = true;
        yield return new WaitForSeconds(duration);
        isInvincible = false;
    }
    #endregion

    #region 💥 DETECÇÃO DE COLISÕES
   private bool alreadyLoading = false;

    private void OnTriggerEnter2D(Collider2D other) {
        if (!isInvincible && other.CompareTag("BodySegment") && !alreadyLoading) {
            speed = 0f;
            alreadyLoading = true;
            GameManager.Instance.LoadLastScene("Game_Over");
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.layer == walls && !alreadyLoading) {
            speed = 0f;
            alreadyLoading = true;
            GameManager.Instance.LoadLastScene("Game_Over");
        }
    }

    #endregion
}
