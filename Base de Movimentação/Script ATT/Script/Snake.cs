using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class Snake : MonoBehaviour {
    public float speed = 5f;
    public float multiSpeed = 1f;
    public float nowSpeed;
    private Vector3 atualDirecao = Vector3.up; // Renomeado de currentDirection

    void Awake() {
        nowSpeed = speed * multiSpeed;
    }

    void Update() {
        move();
        AumentarVelocidade();
    }

    void move() {
        Vector3 inputDirection = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0f);

        if ((inputDirection.y > 0 && atualDirecao.y < 0) || (inputDirection.y < 0 && atualDirecao.y > 0)) {
            inputDirection.y = 0;
        }

        if ((inputDirection.x > 0 && atualDirecao.x < 0) || (inputDirection.x < 0 && atualDirecao.x > 0)) {
            inputDirection.x = 0;
        }

        if (inputDirection != Vector3.zero) {
            atualDirecao = inputDirection;
        }

        transform.position += atualDirecao * nowSpeed * Time.deltaTime;
    }

    void AumentarVelocidade() {
        multiSpeed += 0.1f;
        if (multiSpeed > 10f) {
            multiSpeed = 10f;
        }
        nowSpeed = speed * multiSpeed;
    }
}