using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField] float explosionDamage = 100f;
    [SerializeField] Collider2D explosionCollider;

    WaitForSeconds waitExplosionTime = new WaitForSeconds(0.1f);

    private void OnEnable()
    {
        StartCoroutine(nameof(ExplosionCoroutine));
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<Enemy>(out Enemy enemy))
        {
            enemy.TakeDamage(explosionDamage);
        }
    }

    IEnumerator ExplosionCoroutine()
    {
        explosionCollider.enabled = true;
        yield return waitExplosionTime;
        explosionCollider.enabled = false;
    }
}
