using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordAttack : MonoBehaviour
{
    [SerializeField] private Vector2 rightAttackOffset;
    [SerializeField] private Collider2D swordCollider;
    [SerializeField] private float damage = 3f;
    [SerializeField] private AudioSource[] swings;

    private void Start()
    {
        rightAttackOffset = transform.localPosition;
    }

    public void AttackLeft()
    {
        print("Attack Left!");
        transform.localPosition = -rightAttackOffset;
        swordCollider.enabled = true;
        //swordCollider.enabled = false;   Try and flicker the collider enabled and disabled in the animation
        //swordCollider.enabled = true;
        transform.localPosition = rightAttackOffset; // Local position is used to make it relative to the Parent Object
        //transform.localPosition= new Vector3(rightAttackOffset.x * -1, rightAttackOffset.y); //Need to make a new Vector as if we use -rightAttackOffset it will flip the Y axis as well which isn't desired.
    }

    public void AttackRight()
    {
        print("Attack Right!");
        transform.localPosition = -rightAttackOffset;
        swordCollider.enabled = true;
        transform.localPosition = rightAttackOffset; // Local position is used to make it relative to the Parent Object
    }

    public void StopAttack()
    {
        swordCollider.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //After a successful hit I need to move the character slightly to detect subsequent hits.
        print(other);
        if (other.tag == "Enemy")
        {
            // Deal damage to enemy
            Enemy enemy = other.GetComponent<Enemy>();
            print(enemy);
            if (enemy != null)
            {
                enemy.Health -= damage;
               // enemy.DisplayHpBar();
            }
        }
    }

    public void PlaySwordSFX()
    {
        swings[Random.Range(0, swings.Length)].Play();
    }
}
