using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSlowTime : MonoBehaviour {
    [SerializeField] private float _time = 0.5f;

    void Start() {
        Time.timeScale = _time;
    }
}
