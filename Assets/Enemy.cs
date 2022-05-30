using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float _maxHealth = 5f;
    [SerializeField] private float _damage = 1f;

    private float _health;
    private float _distance = 0.6f;
    private float _moveSpeed = 0.2f;
    private float _chaseTimer = 2f;
    private float _chaseRemain=0f;

    private Transform target; //player
    private SpriteRenderer spriteRenderer;
    private Animator animator;

    public Image hpImage;
    public Image hpEffectImage;
    private float _healthBarDecayTime = 0.015f;
    private bool isAlive = true;

    public delegate void EnemyKilled();
    public static event EnemyKilled OnEnemyKilled;

    private GameObject patrolManager;

    public Transform[] m_PatrolPoints;

    public float Health
    {
        get { return _health; }
        set
        {
            _health = value;
            if (_health <= 0)
            {
                Defeated();
                isAlive = false;
                if (OnEnemyKilled!=null)
                {
                    OnEnemyKilled();
                }
            }
        }
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        patrolManager = GameObject.FindGameObjectWithTag("Patrol");
        m_PatrolPoints = patrolManager.GetComponentsInChildren<Transform>();
        _health = _maxHealth;
    }

    private void FixedUpdate()
    {
        Move();
        TurnDirection();
        DisplayHpBar();
    }

    private void Move()
    {
        if (isAlive)
        {
            if (Vector2.Distance(transform.position, target.position) < _distance || _chaseRemain > 0f)
            {
                if (Vector2.Distance(transform.position, target.position) < _distance)
                {
                    _chaseRemain = _chaseTimer;
                }
                else
                {
                    _chaseRemain=_chaseRemain-Time.deltaTime;
                }
                transform.position = Vector2.MoveTowards(transform.position, target.position, _moveSpeed * Time.deltaTime);
                
            }
            else
            {
                //Find waypoint to move towards
                transform.position = Vector2.MoveTowards(transform.position, m_PatrolPoints[Random.Range(0, m_PatrolPoints.Length)].position, _moveSpeed * Time.deltaTime);
            }
        }
    }

    private void TurnDirection()
    {
        if (transform.position.x > target.position.x)
        {
            spriteRenderer.flipX = true;
        }
        else
        {
            spriteRenderer.flipX = false;
        }
    }

    public void Defeated()
    {
        animator.SetTrigger("Defeated");
    }

    private void RemoveEnemy()
    {
        Destroy(gameObject);
    }

    public void DisplayHpBar()
    {
        hpImage.fillAmount = _health / _maxHealth;

        if (hpEffectImage.fillAmount > hpImage.fillAmount)
        {
            hpEffectImage.fillAmount -= _healthBarDecayTime;
        }
        else
        {
            hpEffectImage.fillAmount = hpImage.fillAmount;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            // Deal damage to enemy
            PlayerController player = other.GetComponent<PlayerController>();
            print(player);
            if (player != null)
            {
                player.Health -= _damage;
                // enemy.DisplayHpBar();
            }
        }
    }
}
