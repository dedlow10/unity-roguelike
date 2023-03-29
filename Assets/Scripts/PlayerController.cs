using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.Diagnostics;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    [SerializeField] GameObject dustTrail;
    //dash effect
    [SerializeField] float dashSpeed;
    private float dashCounter;
    private float dashCoolCounter;
    [SerializeField] private float dashLength = .5f;
    [SerializeField] private float dashCooldown = 1f;

    Rigidbody2D rb;
    Vector2 moveDirection;

    //push effect
    private float pushRecoverySpeed = 0.9f;
    private Vector2 pushDirection;
    private Animator animator;
    private float activeMoveSpeed;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = transform.Find("PlayerSprite").GetComponent<Animator>();
    }

    private void Start()
    {
        activeMoveSpeed = moveSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.IsGamePaused()) return;

        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        moveDirection = new Vector2(moveX, moveY).normalized;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (dashCoolCounter <=0 && dashCounter <=0) {
                AudioManager.instance.PlaySFX("PlayerDash", transform.position);
                activeMoveSpeed = dashSpeed;
                dashCounter = dashLength;
                dustTrail.SetActive(true);
            }
        }

        if (dashCounter > 0)
        {
            dashCounter -= Time.deltaTime;

            if (dashCounter <= 0)
            {
                activeMoveSpeed = moveSpeed;
                dashCoolCounter = dashCooldown;
                dustTrail.SetActive(false);
            }
        }

        if (dashCoolCounter > 0)
        {
            dashCoolCounter -= Time.deltaTime;
        }
    }

    private void FixedUpdate()
    {
        if (GameManager.instance.IsGamePaused())
        {
            moveDirection.x = 0;
            moveDirection.y = 0;
            rb.velocity = moveDirection;
            return;
        }
        pushDirection = Vector3.Lerp(pushDirection, Vector3.zero, pushRecoverySpeed);

        //if (CanMove())
        //{
            var moveDelta = new Vector2(moveDirection.x, moveDirection.y) * activeMoveSpeed;
            moveDelta += pushDirection;
            rb.velocity = moveDelta;
        //}
        //else
        //{
        //    rb.velocity = pushDirection;
        //}


        animator.SetBool("IsWalking", rb.velocity.magnitude > 0);
 
    }

    private bool CanMove()
    {
        float distanceFromObstaclesMultiple = 2f;
        Vector2 currentPosition = transform.Find("Center").transform.position;
        Vector2 newPosition = currentPosition + moveDirection * distanceFromObstaclesMultiple * moveSpeed * Time.deltaTime;
        var hit = Physics2D.Linecast(currentPosition, newPosition, LayerMask.GetMask("Obstacle", "Actor"));

        return hit.collider == null;
    }

    public void OnPush(Vector2 pushDirection)
    {
        this.pushDirection = pushDirection;
    }
}
