using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BouncyWall))]
public class BonusWall : MonoBehaviour {
    [SerializeField] private int _bouncesToDestroy = 3;
    [SerializeField] private GameObject[] _destroyAnim;
    private BouncyWall _wall;

    private int _bounces = 0;

    private GameObject _currentAnim = null;


    private void Awake() {
        _wall = GetComponent<BouncyWall>();

        _wall.OnHit.AddListener(Bounce);
    }

    private void Bounce() {
        _bounces++;

        if (_bounces >= _bouncesToDestroy) {
            Destroy(gameObject);
        }
        else {
            if (_currentAnim != null) {
                Destroy(_currentAnim);
            }
            _currentAnim = Instantiate(_destroyAnim[_bounces - 1], transform);
        }

        if (Ball.Instance.Attraction != null) {
            Ball.Instance.Attraction = GameManager.Instance.Sides.Find(side => side != Ball.Instance.Attraction);
        }
    }
}
