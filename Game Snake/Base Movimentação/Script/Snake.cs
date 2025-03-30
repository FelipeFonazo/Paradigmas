using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class Snake : MonoBehaviour {
    public float speed;

    void Update() {
        move();
    }

    void move() {
        Vector3 direction = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0f);
        transform.position += direction * speed * Time.deltaTime;
    }
}