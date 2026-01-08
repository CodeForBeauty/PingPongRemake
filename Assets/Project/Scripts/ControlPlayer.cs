using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlPlayer : MonoBehaviour {
    [SerializeField] private ControlRacket _wall;

    [SerializeField] private KeyCode _up;
    [SerializeField] private KeyCode _down;
    [SerializeField] private KeyCode _shoot;
    [SerializeField] private KeyCode _unstick;


    void Update() {
        if (Input.GetKey(_up)) {
            _wall.Direction = 1;
        }
        else if (Input.GetKey(_down)) {
            _wall.Direction = -1;
        }

        if (Input.GetKeyDown(_shoot)) {
            _wall.FireBullet();
        }

        if (Input.GetKeyDown(_unstick)) {
            _wall.UnstickBall();
        }
    }

    private void OnEnable() {
        _wall.isPlayerControl = true;
    }
}
