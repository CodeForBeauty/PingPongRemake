using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class GameBonus : MonoBehaviour {
    [SerializeField] private Bonus _bonus;

    [SerializeField] private string _ballTag = "Ball";

    public void Init(Bonus bonus, float time) {
        _bonus = bonus;

        Instantiate(bonus.SkinPrefab, transform);

        Invoke(nameof(RemoveFromGame), time);
    }

    private void RemoveFromGame() {
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (!collision.gameObject.CompareTag(_ballTag)) {
            return;
        }

        _bonus.HitBall = collision.gameObject.GetComponent<Ball>();
        if (_bonus.HitBall == null) {
            return;
        }
        _bonus.OnHit.Invoke();
        Destroy(gameObject);

        AudioSource.PlayClipAtPoint(_bonus.ActivateSound, Camera.main.transform.position);
    }
}
