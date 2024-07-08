using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]

public class PlayerController : MonoBehaviour
{
    Rigidbody2D rb;
    Animator animator;
    Vector2 moveInput;

    private bool _isMoving = false;
    public float speed = 5f;

    public bool IsMoving {
        get
        {
            return _isMoving;
        }
        private set
        {
            _isMoving = value;
            animator.SetBool(AnimationStrings.isMoving, value);
        }
    }

    public bool _isFacingRight = true;

    public bool IsFacingRight { 
        get{ return _isFacingRight;} 
        private set{
            if(_isFacingRight != value){
                //flips local scale to make the player and all child componenets face the oppisote direction
                //this is better because when we make a hit box it will flip the child collider with the sprite
                transform.localScale *= new Vector2(-1,1);
            }

            _isFacingRight = value;
        } 
    }

    void Awake(){
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }
    void Start()
    {
        
    }

    private void FixedUpdate() {
        rb.velocity = new Vector2(moveInput.x * speed, rb.velocity.y);
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnMove(InputAction.CallbackContext context) {
        moveInput = context.ReadValue<Vector2>();

        IsMoving = moveInput != Vector2.zero;
        
        SetFacingDirection(moveInput);
    }

    private void SetFacingDirection(Vector2 moveInput)
    {
        if(moveInput.x > 0 && !IsFacingRight){
            //right
            IsFacingRight = true;
        }
        else if (moveInput.x < 0 && IsFacingRight){
            IsFacingRight = false;
        }
    }
}
