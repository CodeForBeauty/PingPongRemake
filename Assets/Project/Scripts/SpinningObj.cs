using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinningObj : MonoBehaviour {
    [SerializeField] private float _speed = 5;

    private void Update() {
        transform.Rotate(0, 0, _speed * Time.deltaTime);
    }
}
