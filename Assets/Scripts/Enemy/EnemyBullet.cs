using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] bool gravity;
    [SerializeField] float deSpawnTime;
    [SerializeField] ParticleSystem explosionEffect;
    [SerializeField] LayerMask ignoreLayer;
    [SerializeField] Rigidbody rb;
    [SerializeField] float damage;

    Coroutine deSpawnCR;

    private void OnEnable()
    {
        deSpawnCR = StartCoroutine(DeSpawnCR());
        rb.useGravity = gravity;
    }

    private IEnumerator DeSpawnCR()
    {
        yield return new WaitForSeconds(deSpawnTime);
        ObjectPoolManager.ReturnObjectToPool(gameObject);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == ignoreLayer)
            return;
        else
        {
            if (collision.gameObject.GetComponent<IDamagable>() != null)
            {
                var hitObj = collision.gameObject.GetComponent<IDamagable>();
                hitObj.TakeDamage(damage);
            }

            DestroyBullet();
        }
    }

    private void DestroyBullet()
    {
        ObjectPoolManager.SpawnObject(explosionEffect.gameObject, transform.position, Quaternion.identity, ObjectPoolManager.PoolType.Bullet_Explosion);
        ObjectPoolManager.ReturnObjectToPool(gameObject);
        StopCoroutine(deSpawnCR);
    }

    public void SetVelocity(Vector3 value)
    {
        rb.velocity = value * speed;
    }

    public void ResetBullet()
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }
}
