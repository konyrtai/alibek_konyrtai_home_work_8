using UnityEngine;


[RequireComponent(typeof(CharacterController))]
public class CharacterMovement : MonoBehaviour
{
    [Header("Character controller")]
    private CharacterController _characterController;

    /// <summary>
    /// ���������� ���������
    /// </summary>
    [Header("Gravity")] private float _currentAttractionCharacter = 0;

    /// <summary>
    /// ���� ����������
    /// </summary>
    [SerializeField] private readonly float _gravityForce = 20;


    #region Movement

    /// <summary>
    /// �������� �������� ���������
    /// </summary>
    public float Velocity = 0.0f;

    /// <summary>
    /// ������������ �������� ��������
    /// </summary>
    [SerializeField] private readonly float _maxStandingVelocity = 4;

    /// <summary>
    /// ������������ �������� ��������
    /// </summary>
    [SerializeField] private readonly float _maxCrouchingVelocity = 1.3f;

    /// <summary>
    /// ���������
    /// </summary>
    public float Acceleration = 0.7f;

    /// <summary>
    /// ����������
    /// </summary>
    public float Deceleration = 1.5f;

    /// <summary>
    /// �������� �������� ���������
    /// </summary>
    public float RotateSpeed = 5f;


    /// <summary>
    /// � �������
    /// </summary>
    public bool IsCrouching = false;

    #endregion

    #region Animator

    /// <summary>
    /// Hash velocity � animator controller
    /// </summary>
    private int _velocityHash;

    /// <summary>
    /// Hash isCrouching � animator controller
    /// </summary>
    private int _isCrouchingHash;


    /// <summary>
    /// Animator controller
    /// </summary>
    private Animator _animator;

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        _characterController = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();
        _velocityHash = Animator.StringToHash("velocity");
        _isCrouchingHash = Animator.StringToHash("isCrouching");
    }

    // Update is called once per frame
    void Update()
    {
        GravityHandling();
    }

    /// <summary>
    /// ������������
    /// </summary>
    /// <param name="moveDirection"></param>
    public void MoveCharacter(Vector3 moveDirection)
    {
        UpdateVelocity(moveDirection, IsCrouching ? _maxCrouchingVelocity: _maxStandingVelocity);

        if(Velocity > 0 && isVectorEmpty(moveDirection))
            moveDirection = transform.forward;

        moveDirection *= Velocity;
        moveDirection.y = _currentAttractionCharacter;
        _characterController.Move(moveDirection * Time.deltaTime);
    }

    /// <summary>
    /// �������
    /// </summary>
    /// <param name="moveDirection"></param>
    public void RotateCharacter(Vector3 moveDirection)
    {
        if (!_characterController.isGrounded) return;
        if (Vector3.Angle(transform.forward, moveDirection) <= 0) return;

        var newDirection = Vector3.RotateTowards(transform.forward, moveDirection, RotateSpeed, 0);
        transform.rotation = Quaternion.LookRotation(newDirection);
    }

    /// <summary>
    /// ���������� ���������
    /// </summary>
    public void StopCharacter()
    {
        Velocity = 0;
    }

    /// <summary>
    /// ���������� ������
    /// </summary>
    public void SetCrouching(bool isCrouching)
    {
        StopCharacter();
        IsCrouching = isCrouching;
        _animator.SetBool(_isCrouchingHash, IsCrouching);
    }


    /// <summary>
    /// ���������� ���������
    /// </summary>
    /// <param name="moveDirection"></param>
    private void UpdateVelocity(Vector3 moveDirection, float maxVelocity)
    {       
        // �������� �� ��������
        var isMoving = !isVectorEmpty(moveDirection);
        if (isMoving && Velocity < maxVelocity)
        {
            if (Velocity == 0)
            {
                Velocity = Acceleration;
            }
            else
            {
                Velocity += Time.deltaTime * Acceleration;
            }
        }
        if (!isMoving && Velocity > 0)
        {
            if(Velocity is < 0.4f or < 0)
                Velocity = 0;
            else
                Velocity -= Time.deltaTime * Deceleration;
        }
        _animator.SetFloat(_velocityHash, Velocity);
    }

    /// <summary>
    /// ������ �� ������
    /// </summary>
    /// <param name="vector"></param>
    /// <returns></returns>
    private static bool isVectorEmpty(Vector3 vector) => vector.Equals(new Vector3(0, 0, 0));


    private void GravityHandling()
    {
        // ����� �� �����
        if (_characterController.isGrounded)
        {
            _currentAttractionCharacter = 0;
        }
        else
        {
            // ��������� ���� ����������
            _currentAttractionCharacter -= _gravityForce * Time.deltaTime;
        }

    }
}
