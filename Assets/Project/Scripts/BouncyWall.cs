using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody2D))]
public class BouncyWall : MonoBehaviour {
    [SerializeField] private string _ballTag = "Ball";
    [SerializeField] private string _phantomTag = "Phantom";

    [SerializeField] private AudioClip _bounceClip;

    public Vector3 BounceDirection = Vector3.up;

    public UnityEvent OnHit = new();

    public RectTransform rectTransform;

    private void OnCollisionEnter2D(Collision2D collision) {
        if (!collision.gameObject.CompareTag(_ballTag) && !collision.gameObject.CompareTag(_phantomTag)) {
            return;
        }

        Ball ball = collision.gameObject.GetComponent<Ball>();
        ball.AddVelocity(-collision.GetContact(0).normal);
        OnHit.Invoke();

        AudioSource.PlayClipAtPoint(_bounceClip, Camera.main.transform.position);
    }
}
