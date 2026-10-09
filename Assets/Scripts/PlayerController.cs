using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float speedMove = 15f;
    [SerializeField] float jumpSpeed = 5f;
    [SerializeField] float climbSpeed = 5f;
    [SerializeField] Vector2 dead = new Vector2 (10f, 20f);
    [SerializeField] GameObject bullet;
    [SerializeField] Transform gun ;// biến đổi viên đạn


    Vector2 moveInput;
    Rigidbody2D rb;
    Animator myAnimator; // 
    CapsuleCollider2D myBodyCollider;
    
    BoxCollider2D myFeetClollider;
    float gravityScaleAtClimb;

    bool isAlive = true;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        myBodyCollider = GetComponent<CapsuleCollider2D>();
        gravityScaleAtClimb = rb.gravityScale; // gọi rigidbody để lấy gravity trong rigidbody
        myFeetClollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!isAlive) {return;}
        Run(); // chạy
        FlipSprites(); // lật mặt player
        ClimbLadder(); // leo thang
        Die();
    }

    void OnMove(InputValue value)
    {
        if(!isAlive) {return;}
        moveInput = value.Get<Vector2>();
        Debug.Log(moveInput);
        
        
    }

    void OnJump(InputValue value)
    {
        if(!myFeetClollider.IsTouchingLayers(LayerMask.GetMask("Ground"))) { return;}
        if(value.isPressed) // kiểm tra có đang nhấn nút space không
        {
            rb.velocity += new Vector2 (0f, jumpSpeed); // += Giữ vận tốc hiện tại + cộng thêm vận tốc nhảy
        } 
    }

    

    void Run()
    {
        Vector2 playerVelocity = new Vector2(moveInput.x * speedMove, rb.velocity.y); 
        rb.velocity = playerVelocity; // gán rb.verlocity bằng playerVelocity
        bool hasHorizontalSpeed = Mathf.Abs(rb.velocity.x)> Mathf.Epsilon; // kiểm tra xem có đang di chuyển hay không
        myAnimator.SetBool("isRunning",hasHorizontalSpeed);
    }

    

    void FlipSprites() 
    {
        bool hasHorizontalSpeed = Mathf.Abs(rb.velocity.x)> Mathf.Epsilon;
        if(hasHorizontalSpeed)
        { 
            transform.localScale = new Vector2(Mathf.Sign(rb.velocity.x), 1f); //Mathf.Sign dùng để kiểm tra số âm ha số dương
            // âm là bên trái, dương bên phải
        }
        
    }

    

    void ClimbLadder() // thay đổi y , x giữ nguyên => kiểm tra xem nhân vật có đang chạm vào tag player là Climbing ko 
    // nếu không chạm vào thì leo không trả về
    {
        if(!myFeetClollider.IsTouchingLayers(LayerMask.GetMask("Climbing"))) 
        {
            rb.gravityScale = gravityScaleAtClimb; // nếu player không chạm vào thang thì trọng lực vẫn không thay đổi
            myAnimator.SetBool("isClimbing", false);
            return;
        } //capsulecollider là của player
        rb.gravityScale = 0f; // trọng lực bằng 0 để không rơi xuống
        Vector2 ClimbVelocity = new Vector2 (rb.velocity.x, moveInput.y * climbSpeed);
        rb.velocity = ClimbVelocity;
        bool hasVerticalSpeed = Mathf.Abs(rb.velocity.y)> Mathf.Epsilon;
        myAnimator.SetBool("isClimbing", hasVerticalSpeed);// nếu đúng thì bật hành động leo
        
    }

    void OnAttack(InputValue value)
    {
        if(!isAlive) {return;}
        Instantiate(bullet,gun.position, transform.rotation);
    }

    void Die()
    {
        if(myBodyCollider.IsTouchingLayers(LayerMask.GetMask("Enemies", "Hazad")))
        {
            isAlive = false;
            myAnimator.SetTrigger("Dying");
            rb.velocity = dead;
        }
    }
}
