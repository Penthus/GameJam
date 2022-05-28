using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float _health = 1f;
    private float _distance = 0.6f;
    private float _moveSpeed = 0.2f;
    private float _chaseTimer = 2f;
    private float _chaseRemain=0f;
    private Transform target; //player
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    

    public float Health
    {
        get { return _health; }
        set
        {
            _health = value;
            if (_health <= 0)
            {
                Defeated();
            }
        }
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    private void FixedUpdate()
    {
        Move();
        TurnDirection();
    }

    private void Move()
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
}
