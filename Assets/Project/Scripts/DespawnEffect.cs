using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DespawnEffect : MonoBehaviour {
    [SerializeField] private float _time = 2;

    private IEnumerator Start() {
        yield return new WaitForSeconds(_time);

        Destroy(gameObject);
    }
}
