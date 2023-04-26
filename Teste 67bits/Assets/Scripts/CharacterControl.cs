using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;
[RequireComponent(typeof(CharacterController))]
public class CharacterControl : MonoBehaviour
{
    [SerializeField]
    private PlayerInput inputs;
    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    [SerializeField]
    private float playerSpeed = 2.0f;
    private float gravityValue = -9.81f;
    [SerializeField]
    Animator anim;
    [SerializeField]
    private Collider rightHand;

    private void Awake()
    {
        controller = gameObject.GetComponent<CharacterController>();
        inputs = gameObject.GetComponent<PlayerInput>();
        anim = gameObject.GetComponent<Animator>();
    }

    void Update()
    {
        SetAnim();
        PlayerMove();
    }

    void SetAnim(){
        anim.SetBool("isMoving", inputs.actions["Move"].ReadValue<Vector2>().x + inputs.actions["Move"].ReadValue<Vector2>().y != 0 );
    }

    void PlayerMove(){
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        Vector3 move = new Vector3(inputs.actions["Move"].ReadValue<Vector2>().x, 0, inputs.actions["Move"].ReadValue<Vector2>().y);
        controller.Move(move * Time.deltaTime * playerSpeed);

        if (move != Vector3.zero)
        {
            gameObject.transform.forward = move;
        }

        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }

    public void Punch(){
        rightHand.isTrigger = false;
        anim.SetTrigger("Punch");
    }

    public void SetHandCollider(){
        rightHand.isTrigger = true;
    }
}
