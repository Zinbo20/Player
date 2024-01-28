using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float moveSpeed;
    public Rigidbody2D rb;

    public Animator anim;

    private Vector2 moveDirection;
    private Vector2 lastMoveDirection;

    // Update is called once per frame
    void Update()
    {
        ProcessInputs();
    }

    void FixedUpdate()  //Physics Calculations
    {
        Move();
        Animate();
    }

    void ProcessInputs()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        if((moveX == 0 && moveY ==0)&& moveDirection.x !=0 || moveDirection.y != 0)
        {
            lastMoveDirection = moveDirection;
        }

        moveDirection = new Vector2(moveX, moveY); //jiojio
    }
    void Move()
    {
        rb.velocity = new Vector2(moveDirection.x * moveSpeed, moveDirection.y * moveSpeed);
    }

    void Animate()
    {
        anim.SetFloat("AnyMoveX", moveDirection.x);
        anim.SetFloat("AnyMoveY", moveDirection.y);
        anim.SetFloat("AnyMoveMagnitude",moveDirection.magnitude);
        anim.SetFloat("AnyLastMoveX", lastMoveDirection.x);
        anim.SetFloat("AnyLastMoveY", lastMoveDirection.y);
    }

}
