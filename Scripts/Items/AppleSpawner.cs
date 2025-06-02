using UnityEngine;

public class AppleSpawner : MonoBehaviour{
    public GameObject ApplePrefab;
    public Vector2 spawnAreaMin;
    public Vector2 spawnAreaMax;

    public void SpawnApple(){
        
    Vector2 spawnPos = new Vector2(
        Random.Range(spawnAreaMin.x, spawnAreaMax.x),
        Random.Range(spawnAreaMin.y, spawnAreaMax.y)
    );

    // Verifique se a posição de spawn está dentro dos limites desejados
    Instantiate(ApplePrefab, spawnPos, Quaternion.identity);
}

    private void Start(){
        SpawnApple();
    }
}
