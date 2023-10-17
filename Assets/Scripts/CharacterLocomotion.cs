using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterLocomotion : MonoBehaviour
{
    #region Variables

    private Animator animator;
    private Vector2 input;
    private bool isRunning;

    [SerializeField] private float movementSpeed;
    [SerializeField] private float incrementSpeed = 0.7f;

    #endregion

    #region Unity Methods

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        InputVectorUpdate();
        InputUpdate();
        MovementSpeedUpdate();
        AnimationValuesUpdate();
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
            isRunning = true;
        }

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            isRunning = false;
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
    }

    #endregion
}
