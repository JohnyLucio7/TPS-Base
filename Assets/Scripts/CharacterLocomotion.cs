using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterLocomotion : MonoBehaviour
{
    #region Variables

    private CharacterController cc;
    private Animator animator;
    private bool isRunning;
    private bool isJumping;
    private Vector2 input;
    private Vector3 rootMotion; // precisamos pegar a posição do personagem atravez da animação para atualizar o character controller
    private Vector3 velocity;

    [Header("Locomotion Config")]
    [SerializeField] private float movementSpeed;
    [SerializeField] private float incrementSpeed = 0.7f;
    [SerializeField] private float jumpHeight;
    [SerializeField] private float gravity;
    [SerializeField] private float stepDown = 0.3f;
    [SerializeField] private float airControl = 2.5f;
    [SerializeField] private float groundSpeed = 1f;
    [SerializeField] private float jumpDamp = 0.5f;

    #endregion

    #region Unity Methods

    void Start()
    {
        animator = GetComponent<Animator>();
        cc = GetComponent<CharacterController>();
        isRunning = true;
    }

    void Update()
    {
        InputVectorUpdate();
        InputUpdate();
        MovementSpeedUpdate();
        AnimationValuesUpdate();
    }

    private void FixedUpdate()
    {
        if (isJumping)
        {
            UpdateOnAir();
        }
        else
        {
            UpdateOnGround();
        }
    }

    private void OnAnimatorMove()
    {
        rootMotion = animator.deltaPosition;
    }

    #endregion

    #region Custom Methods

    void InputVectorUpdate()
    {
        input.x = Input.GetAxis("Horizontal");
        input.y = Input.GetAxis("Vertical");
    }

    void InputUpdate()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            isRunning = false;
        }

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            isRunning = true;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
    }

    void MovementSpeedUpdate()
    {
        if (isRunning && movementSpeed < 1)
        {
            movementSpeed += incrementSpeed * Time.deltaTime;
        }

        if (!isRunning && movementSpeed > 0)
        {
            movementSpeed -= incrementSpeed * Time.deltaTime;
        }
    }

    void AnimationValuesUpdate()
    {
        animator.SetFloat("InputX", input.x);
        animator.SetFloat("InputY", input.y);
        animator.SetFloat("MovementSpeed", movementSpeed);
        animator.SetBool("isJumping", isJumping);
    }

    /** Realiza a movimentação do personagem no chão */
    void UpdateOnGround()
    {
        Vector3 stepForwardAmount = rootMotion * groundSpeed; 
        Vector3 stepDownAmount = Vector3.down * stepDown;

        cc.Move(stepForwardAmount + stepDownAmount);

        rootMotion = Vector3.zero;

        if (!cc.isGrounded)
        {
            SetInAir(0);
        }
    }

    /** Realiza a movimentaçãodo personagem no ar */

    void UpdateOnAir()
    {
        velocity.y -= gravity * Time.fixedDeltaTime; // Aplicando o peso da gravidade ao vetor velocidade que usamos para definir o deslocamento

        Vector3 displacemant = velocity * Time.fixedDeltaTime; // aplicando o vetor velocidade ao deslocamento

        displacemant += CalculateInAir(); // somando o deslocamento ao movimento realizando pelo player

        cc.Move(displacemant); // aplicando deslocamento ao character controller

        isJumping = !cc.isGrounded;

        rootMotion = Vector3.zero;
    }

    void SetInAir(float jumpVelocity)
    {
        isJumping = true;
        velocity = animator.velocity * jumpDamp * groundSpeed;
        velocity.y = jumpVelocity;
    }

    void Jump()
    {
        if (!isJumping)
        {
            float jumpVelocity = Mathf.Sqrt(2 * gravity * jumpHeight);
            SetInAir(jumpVelocity);
        }
    }

    Vector3 CalculateInAir()
    {
        return ((transform.forward * input.y) + (transform.right * input.x)) * (airControl / 100);
    }

    #endregion
}
