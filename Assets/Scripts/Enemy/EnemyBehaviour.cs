using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour, IDamagable
{
    [SerializeField] private float _health = 100f;
    [SerializeField] private GameObject _splinters;
    [SerializeField] private Material _damagedMat;
    [SerializeField] private float playerScanRadius;
    [SerializeField] private Transform bulletSpawn;
    [SerializeField] private EnemyBullet bulletPrefab;
    public float moveSpeed;


    #region Health Property
    public float Health
    {
        get => _health;
        set
        {
            if (_health != value)
                _health = value;
        }
    }
    #endregion

    #region State Machine Moment
    public EnemyStateMachine StateMachine { get; set; }
    public EnemyIdleState IdleState { get; set; }
    public EnemyChaseState ChaseState { get; set; }
    public EnemyAttackState AttackState { get; set; }

    public float EnemyChaseRadius;
    #endregion

    private bool dead;
    private MeshRenderer enemyRenderer;
    private Material enemyMat;
    private float initialY;

    private bool inPlayerRange;
    private bool preInPlayerRange;
    private PlayerMovement player;

    private void Awake()
    {
        StateMachine = new();
        IdleState = new(this, StateMachine);
        ChaseState = new(this, StateMachine);
        AttackState = new(this, StateMachine);

        StateMachine.Initialize(ChaseState);
    }

    private void Start()
    {
        enemyRenderer = GetComponent<MeshRenderer>();
        enemyMat = enemyRenderer.material;
        initialY = transform.position.y;

        player = FindObjectOfType<PlayerMovement>();
    }

    private void FixedUpdate()
    {
        StateMachine.CurrentEnemyState.PhysicsUpdate();
    }
    private void Update()
    {
        if(player != null && Vector3.Distance(transform.position, player.transform.position) <= playerScanRadius)
            inPlayerRange = true;
        else
            inPlayerRange = false;

        #region Attack and Chase State Changing
        if (inPlayerRange && !preInPlayerRange)
        {
            Debug.Log($"Changed to Attack state for {gameObject.name}");
            StateMachine.ChangeState(AttackState);
        }
        else if (!inPlayerRange && preInPlayerRange)
        {
            StateMachine.ChangeState(ChaseState);
        }

        preInPlayerRange = inPlayerRange; 
        #endregion

        if(transform.position.y != initialY)
        {
            transform.position = new(transform.position.x,
                                     initialY,
                                     transform.position.z);
        }
        StateMachine.CurrentEnemyState.FrameUpdate();
    }

    public void TakeDamage(float damage)
    {
        Health -= damage;
        if(Health <= 0 && !dead)
        {
            OnKilled();
            dead = true;
        }
        else
        {
            StartCoroutine(ChangeColor());
        }
    }

    private IEnumerator ChangeColor()
    {
        enemyRenderer.material = _damagedMat;
        yield return new WaitForSeconds(0.01f);
        enemyRenderer.material = enemyMat;
    }

    public void ShootBullet()
    {
        var spawnedBullet = ObjectPoolManager.SpawnObject(bulletPrefab.gameObject, bulletSpawn.position, bulletSpawn.rotation, ObjectPoolManager.PoolType.EnemyBullet);
        spawnedBullet.GetComponent<EnemyBullet>().SetVelocity(spawnedBullet.transform.forward);
    }

    public void OnKilled()
    {
        Instantiate(_splinters, new Vector3(transform.position.x, transform.position.y + 0.2f, transform.position.z), Quaternion.identity);

        var powerUp = GetComponent<PowerUpDropHandler>();

        if(powerUp != null)
        {
            powerUp.GeneratePowerUp();
        }

        Destroy(gameObject);
    }
}
