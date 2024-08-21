using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    private CharacterController _characterController;
    private PlayerInputScript _playerInputScript;
    private PlayerInput _playerInput;
    private Animator _animator;
    
    // cinemachine
    public GameObject CinemachineCameraTarget;
    public GameObject _mainCamera;
    
    private float _cinemachineTargetYaw;
    private float _cinemachineTargetPitch;
    
    public float TopClamp = 70.0f;
    public float BottomClamp = -30.0f;

    // player
    public float MoveSpeed = 2.0f;
    public float SprintSpeed = 5f;
    public bool isSit;

    
    private float _speed;
    private float _animationBlend;
    private float _targetRotation = 0.0f;
    private float _rotationVelocity;
    private float _verticalVelocity;
    private float _terminalVelocity = 53.0f;

    // Gravity
    public bool isGrounded;
    public LayerMask GroundLayers;
    public float gravityValue = -9.81f;
    public float groundedOffset;
    public float groundedRadius;
    
    void Start()
    {
        _cinemachineTargetYaw = CinemachineCameraTarget.transform.rotation.eulerAngles.y;
        _animator = GetComponent<Animator>();
        _characterController = GetComponent<CharacterController>();
        _playerInputScript = GetComponent<PlayerInputScript>();
        
    }
    
    private void Awake()
    {
        // get a reference to our main camera
        if (_mainCamera == null)
        {
            _mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
            isSit = true;
        
        if (isSit)
            sit();
        else
            Move();
        
        Gravity();
        GroundCheck();
    }
    private void LateUpdate()
    {
        CameraRotation();
    }

    void sit()
    {
        Debug.Log("눌렀다.");
        transform.position = new Vector3(0.7f, -0.527f, 3.75f);
        _animator.SetBool("isSit", true);
    }
    
    private void CameraRotation()
    {
        if (_playerInputScript.look.sqrMagnitude >= 0.01f)
        {
            _cinemachineTargetYaw += _playerInputScript.look.x * 2f;
            _cinemachineTargetPitch += _playerInputScript.look.y * 2f;
        }
        _cinemachineTargetYaw = ClampAngle(_cinemachineTargetYaw, float.MinValue, float.MaxValue);
        _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);

        CinemachineCameraTarget.transform.rotation = Quaternion.Euler(_cinemachineTargetPitch, _cinemachineTargetYaw, 0.0f);
    }
    
    private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
    {
        if (lfAngle < -360f) lfAngle += 360f;
        if (lfAngle > 360f) lfAngle -= 360f;
        return Mathf.Clamp(lfAngle, lfMin, lfMax);
    }
    
    private void Move()
        {
            float targetSpeed = _playerInputScript.sprint ? SprintSpeed : MoveSpeed;

            if (_playerInputScript.move == Vector2.zero)
            {
                targetSpeed = 0.0f;
            }

            float currentHorizontalSpeed = new Vector3(_characterController.velocity.x, 0.0f, _characterController.velocity.z).magnitude;

            float speedOffset = 0.1f;
            
            if (currentHorizontalSpeed < targetSpeed - speedOffset ||
                currentHorizontalSpeed > targetSpeed + speedOffset)
            {
                _speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed , Time.deltaTime * 10f);

                _speed = Mathf.Round(_speed * 1000f) / 1000f;
            }
            else
            {
                _speed = targetSpeed;
            }
            

            _animationBlend = Mathf.Lerp(_animationBlend, targetSpeed, Time.deltaTime * 10f);
            if (_animationBlend < 0.01f) _animationBlend = 0f;

            Vector3 inputDirection = new Vector3(_playerInputScript.move.x, 0.0f, _playerInputScript.move.y).normalized;

            if (_playerInputScript.move != Vector2.zero)
            {
                _targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg +
                                  _mainCamera.transform.eulerAngles.y;
                float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation, ref _rotationVelocity,
                    0.12f);

                transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
            }


            Vector3 targetDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;

            _characterController.Move(targetDirection.normalized * (_speed * Time.deltaTime) +
                             new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);
            
            _animator.SetFloat("Speed", _animationBlend);
        }
    
    void Gravity()
    {
        _verticalVelocity += gravityValue * Time.deltaTime;

        if (isGrounded)
        {
            if (_verticalVelocity < 0.0f)
                _verticalVelocity = -2f;
        }

        //_playerInputScript.jump = false;
    }

    void GroundCheck()
    {
        Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - groundedOffset, transform.position.z);
        isGrounded = Physics.CheckSphere(spherePosition, groundedRadius, GroundLayers, QueryTriggerInteraction.Ignore);
    }
}
