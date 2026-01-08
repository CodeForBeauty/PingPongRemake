using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIControl : MonoBehaviour {
    [SerializeField] private ControlRacket _racket;

    [SerializeField] private float _reactDistance = 1;
    [SerializeField] private float _reactDistanceFast = 1.5f;

    [SerializeField] private LayerMask _wallsMask;

    [SerializeField] private float _unstickPos = 3;

    private float _random = 0;


    private float _direction = 0;
    ControlRacket _other;
    void Update() {
        if (_racket.IsBallStuck) {
            _direction = (_unstickPos * _random * 2) - _racket.transform.position.y;
            SetDirection();

            if (_direction < 0.3f && _direction > -0.3f) {
                _racket.UnstickBall();
            }

            return;
        }

        if (_racket.GunBullets > 0) {
            _other = GameManager.Instance.Sides[GameManager.Instance.Sides.IndexOf(_racket) == 0 ? 1 : 0];

            _direction = _other.transform.position.y - _racket.transform.position.y;
            SetDirection();

            if (_direction < 0.3f && _direction > -0.3f) {
                _racket.FireBullet();
            }

            return;
        }

        RaycastHit2D hit = Physics2D.Raycast(Ball.Instance.transform.position, Ball.Instance.Velocity, 500, _wallsMask);
        Vector3 hitPos = hit.point;
        float xDis = _racket.transform.position.x - hitPos.x;

        if ((Ball.Instance.SpeedMultiplier == 1.0f && xDis > _reactDistance) || (Ball.Instance.SpeedMultiplier > 1.0f && xDis > _reactDistanceFast)) {
            _random = Random.Range(-0.5f, 0.5f);
            _racket.Direction = 0;
            return;
        }

        if (Ball.Instance.IsHiden) {
            _random *= 2;
        }

        _direction = (hitPos.y + _random) - _racket.transform.position.y;
        if (Ball.Instance.IsDeath) {
            _direction = -_direction;
        }
        SetDirection();
    }

    private void SetDirection() {
        if (_direction < 0.3f && _direction > -0.3f) {
            return;
        }

        if (_direction < 0) {
            _racket.Direction = -1;
        }
        else if (_direction > 0) {
            _racket.Direction = 1;
        }
        else {
            _racket.Direction = 0;
        }
    }

    private void OnEnable() {
        _racket.isPlayerControl = false;
    }
}
