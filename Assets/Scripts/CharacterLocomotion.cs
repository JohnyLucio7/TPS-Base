using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterLocomotion : MonoBehaviour
{
    private Animator animator;
    private Vector2 input;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        InputVectorUpdate();
        AnimationValuesUpdate();
    }

    void InputVectorUpdate()
    {
        input.x = Input.GetAxis("Horizontal");
        input.y = Input.GetAxis("Vertical");
    }

    void AnimationValuesUpdate()
    {
        animator.SetFloat("InputX", input.x);
        animator.SetFloat("InputY", input.y);
    }
}
