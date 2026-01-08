using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {
    [SerializeField] private float _speed = 10.0f;
    public Vector3 MoveDirection;

    void Update() {
        transform.position += _speed * Time.deltaTime * MoveDirection;
    }
}
