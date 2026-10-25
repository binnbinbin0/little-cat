using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;


[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{

    [SerializeField] float moveSpeed = 3;
    [SerializeField] float attackMoveSpeed = 1;
    [SerializeField] float jumpPower = 3;
    [SerializeField] float distToGround;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] GameObject hitObject;

    private Collider2D hitBox;
    private Collider2D playerCollider;
    private float moveInput;
    private bool isAttacking = false;

    // this represents in-air state too
    private bool isJumping = false;
    private bool isDucking = false;
    private float applyMoveSpeed;
    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer sr;
    private GameObject currentOneWayPlatform;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        hitBox = hitObject.GetComponent<Collider2D>();
        playerCollider = GetComponent<Collider2D>();
        applyMoveSpeed = moveSpeed;
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.CompareTag("OneWayPlatform"))
        {
            currentOneWayPlatform = collision.gameObject;
        }

    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("OneWayPlatform"))
        {
            currentOneWayPlatform = null;
        }
    }

    private IEnumerator DisableCollision()
    {
        if (currentOneWayPlatform != null)
        {
            BoxCollider2D platformCollider = currentOneWayPlatform.GetComponent<BoxCollider2D>();
            
            Physics2D.IgnoreCollision(playerCollider, platformCollider);
            yield return new WaitForSeconds(1f);
            Physics2D.IgnoreCollision(playerCollider, platformCollider, false);
            
        }

    }

    public void OnMove(InputAction.CallbackContext ctx)
    {
        moveInput = ctx.ReadValue<float>();
        animator.SetBool("isWalking", moveInput != 0);
        sr.flipX = moveInput != 0 ? moveInput < 0 : sr.flipX;


        // hitObject location adjusting (it follows forward of sprite)
        float targetZ = sr.flipX ? 180f : 0f;
        hitObject.transform.rotation = Quaternion.Euler(0f, targetZ, 0f);
    }

    public void OnJump(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            Jump();
        }
    }

    public void OnAttack(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            Attack();
        }
    }

    public void OnDuck(InputAction.CallbackContext ctx)
    {
        // represents if pressing under arrow for lower jump

        if (ctx.performed) isDucking = true;
        else if (ctx.canceled) isDucking = false;

        Debug.Log(isDucking);
    }

    private bool IsGrounded => rb.linearVelocityY == 0;

    private void Jump()
    {
        if (!IsGrounded || isAttacking) return;
        if (isDucking)
        {
            StartCoroutine(DisableCollision());
            return;
        }

        rb.linearVelocityY = jumpPower;
        isJumping = true;
        animator.SetBool("isJumping", true);
    }

    private IEnumerator ProcessHitBox()
    {
        hitBox.enabled = true;
        
        yield return new WaitForSeconds(0.1f);

        hitBox.enabled = false;
    }


    private void Attack()
    {
        if (!IsGrounded) return;

        if (isAttacking)
        {
            animator.Play("cat_attack", 0, 0f);
        }

        isAttacking = true;
        applyMoveSpeed = attackMoveSpeed;
        animator.SetBool("isAttacking", true);
        StartCoroutine(ProcessHitBox());
    }

    public void OnAttackFinish()
    {
        animator.SetBool("isAttacking", false);
        isAttacking = false;
        applyMoveSpeed = moveSpeed;
    }

    private void FixedUpdate()
    {
        rb.linearVelocityX = moveInput * applyMoveSpeed;
        if (isJumping)
        {
            if (IsGrounded)
            {
                isJumping = false;
                animator.SetBool("isJumping", false);
            }
        }

        if (!IsGrounded)
        {
            isJumping = true;
            animator.SetBool("isJumping", true);
        }

    }

}


