using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonMovement : MonoBehaviour
{
    public CharacterController controller;
    public CharacterController groundController;
    public Transform cam;

    [SerializeField]
    public float speedsetting = 8f;
    public float speed;

    [SerializeField]
    public float sprintingMultiplier;
    public bool isSpringting = false;
    

    public float verticalVelocity;
    public float gravity = 200.0f;
    public float jumpForce = 10.0f;

    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;

    #region - Gravity -
    private void Gravity()
    {
        if (groundController.isGrounded || controller.isGrounded)
        {
            verticalVelocity = -gravity * Time.deltaTime;
            if (Input.GetKeyDown(KeyCode.Space))
            {
                verticalVelocity = jumpForce;
            }
            if (Input.GetKey(KeyCode.LeftShift))
            {
                isSpringting = true;
            }
            else
            {
                isSpringting = false;
            }
        }
        else
        {
            verticalVelocity -= gravity * Time.deltaTime;
        }
        Vector3 moveVector = new Vector3(0, verticalVelocity, 0);
        controller.Move(moveVector * Time.deltaTime);
        
    }
    #endregion

    #region - Movement - 
    private void Movement()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            speed = speedsetting;
            if (isSpringting == true)
            {
                speed += sprintingMultiplier;
            }
            controller.Move(moveDir.normalized * speed * Time.deltaTime);
        }
    }
    #endregion

    #region - Update -
    // Update is called once per frame
    private void Update()
    {
        Gravity(); 
        Movement();
    }
    #endregion
}
