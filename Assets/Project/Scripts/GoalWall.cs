using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class GoalWall : MonoBehaviour {
    [SerializeField] private int _index;
    [SerializeField] private string _ballTag = "Ball";
    [SerializeField] private string _bulletTag = "Bullet";
    [SerializeField] private string _phantomTag = "Phantom";

    [SerializeField] private AudioClip _goalClip;


    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag(_bulletTag)) {
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.CompareTag(_phantomTag)) {
            Teleport(collision.gameObject);
            return;
        }
        if (!collision.gameObject.CompareTag(_ballTag)) {
            return;
        }

        //Teleport(collision.gameObject);
        if (!Ball.Instance.IsDeath) {
            GameManager.Instance.AddScore(_index == 0 ? 1 : 0);
        }
        else {
            GameManager.Instance.ResetLevel();
        }
        Ball.Instance.IsDeath = false;

        AudioSource.PlayClipAtPoint(_goalClip, Camera.main.transform.position);
    }

    private void Teleport(GameObject ball) {
        Vector3 newPos = ball.transform.position;
        newPos.x = -newPos.x;
        ball.transform.position = newPos;
    }
}
