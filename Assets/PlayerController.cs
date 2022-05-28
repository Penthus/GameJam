using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public SwordAttack swordAttack;

    private Vector2 movementInput;
    private bool canMove = true;

    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private SpriteRenderer swordSpriteRenderer;

    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private float collisionOffset = 0.05f; // extra distance to check for objects to ensure you don't get stuck in a wall etc.
    [SerializeField] private ContactFilter2D movementFilter;
    [SerializeField] private List<RaycastHit2D> castCollisions = new List<RaycastHit2D>();

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (canMove)
        {
            // If movement input is not 0, try to move
            if (movementInput != Vector2.zero)
            {
                bool sucess = TryMove(movementInput);

                //If there is a collision then try to move via 1 axis to prevent player getting stuck - instead the player will slide off if possible
                if (!sucess && movementInput.x != 0)
                {
                    sucess = TryMove(new Vector2(movementInput.x, 0));
                }
                if (!sucess && movementInput.y != 0)
                {
                    sucess = TryMove(new Vector2(0, movementInput.y));
                }
                animator.SetBool("isMoving", sucess);
            }
            else
            {
                animator.SetBool("isMoving", false);
            }
            // set direction of sprite to movement direction
            if (movementInput.x < 0)
            {
                transform.localScale = new Vector3(-1, 1, 1);
                //spriteRenderer.flipX = true;
            }
            else if (movementInput.x > 0)
            {
                transform.localScale = new Vector3(1, 1, 1);
                //spriteRenderer.flipX = false;
            }
        }
    }

    private bool TryMove(Vector2 direction)
    {
        if (direction != Vector2.zero)
        {
            // Check for Collision
            int count = rb.Cast(
                direction,
                movementFilter, //which layer to collide with
                castCollisions, //List of collisions after cast
                moveSpeed * Time.captureFramerate + collisionOffset); //cast distance with an offset to prevent the player not getting stuck on walls etc
            if (count == 0)
            {
                rb.MovePosition(rb.position + direction * moveSpeed * Time.fixedDeltaTime);
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            //Can't move if there's no direction to move in
            return false;
        }
    }

    void OnMove(InputValue movementValue)
    {
        movementInput = movementValue.Get<Vector2>();
    }

    void OnFire()
    {
        print("Fire Pressed!");
        animator.SetTrigger("swordAttack");
    }

    public void SwordAttack()
    {
        LockMovement();

        if (transform.localScale == new Vector3(-1, 1, 1))
        {
            swordAttack.AttackLeft();
        }
        else
        {
            swordAttack.AttackRight();
        }
    }

    public void EndSwordAttack()
    {
        UnlockMovement();
        swordAttack.StopAttack();
    }

    public void LockMovement()
    {
        canMove = false;
    }

    public void UnlockMovement()
    {
        canMove = true;
    }
}
