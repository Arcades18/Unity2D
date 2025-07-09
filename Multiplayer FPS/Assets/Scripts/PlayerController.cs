using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(ConfigurableJoint))]
[RequireComponent(typeof(PlayerMotor))]
public class PlayerController : MonoBehaviour
{
    [Header("Player Setting")]
    [SerializeField] private float speed = 5f;
    [SerializeField] private float sensitivity = 3f;
    [SerializeField] private float thrusterForce = 1000f;

    [Header("Spring Setting")]
    [SerializeField] private float jointSpring = 20f;
    [SerializeField] private float jointMaxForce = 40f;

    private Animator animator;
    private ConfigurableJoint joint;
    private PlayerMotor motor;

    private void Awake()
    {
        motor = GetComponent<PlayerMotor>();
        joint = GetComponent<ConfigurableJoint>();
        animator = GetComponent<Animator>();
        SetJointSetting(jointSpring);
    }
    private void Update()
    {
        float xMove = Input.GetAxis("Horizontal");
        float zMove = Input.GetAxis("Vertical");

        Vector3 movHorizontal = transform.right * xMove;
        Vector3 movVertical = transform.forward * zMove;

        Vector3 velocity = (movHorizontal + movVertical) * speed;

        animator.SetFloat("ForwardVelocity", zMove);

        motor.Move(velocity);

        float yRotation = Input.GetAxisRaw("Mouse X");
        Vector3 _rotation = new Vector3(0f, yRotation, 0f) * sensitivity;

        motor.Rotate(_rotation);

        float xRotation = Input.GetAxisRaw("Mouse Y");
        float _cameraRotationX = xRotation * sensitivity;

        motor.RotateCamera(_cameraRotationX);

        Vector3 _thruterForce = Vector3.zero;
        if (Input.GetButton("Jump"))
        {
            _thruterForce = Vector3.up * thrusterForce;
            SetJointSetting(0f);
        }
        else
        {
            SetJointSetting(jointSpring);
        }
            motor.ApplyThrusterForce(_thruterForce);
    }
    private void SetJointSetting(float _jointSpring)
    {
        joint.yDrive = new JointDrive
        {
            positionSpring = _jointSpring,
            maximumForce = jointMaxForce
        };
    }
}
