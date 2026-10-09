using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] float bulletSpeed = 20f;

    Rigidbody2D myRb;
    // tìm file script
    PlayerController player;
    float xSpeed;
    // Start is called before the first frame update
    void Start()
    {
        myRb = GetComponent<Rigidbody2D>();
        // tìm scriptt
        player = FindFirstObjectByType<PlayerController>();
        xSpeed = player.transform.localScale.x * bulletSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        myRb.velocity = new Vector2(xSpeed,0f);
    }

    void OnTriggerEnter2D(Collider2D other) // trigger này nằm ở collision chỗ kiểm tra enemy
    {
        if(other.CompareTag("Enemy")) // Khi Bullet đi vào Trigger của một vật thể, kiểm tra vật thể đó có Tag Enemy không. 
        // Nếu đúng thì xóa vật thể đó
        {
            Destroy(other.gameObject); 
        }
        Destroy(gameObject); // hủy viên đạn, obj chỗ này được hiểu là cái obj dc gắn script bullet
    
    }

    void OnCollisionEnter2D(Collision2D collision) 
    {
        Destroy(gameObject,1f);
    }


}
