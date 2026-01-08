using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicObj : MonoBehaviour {
    [SerializeField] private float _scaleMult = 1.2f;
    [SerializeField] private float _speed = 0.5f;

    private float _time = 0;

    private Vector3 _initScale;

    private void Awake() {
        _initScale = transform.localScale;
    }

    private void Update() {
        _time += Time.deltaTime;

        transform.localScale = Vector3.Lerp(_initScale, _initScale * _scaleMult, Mathf.Abs(Mathf.Sin(_time * _speed)));
    }
}
