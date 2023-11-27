using UnityEngine;
using UnityEngine.UI;

public class InputController : MonoBehaviour
{
    /// <summary>
    /// Персонаж
    /// </summary>
    [SerializeField] private GameObject _character;

    /// <summary>
    /// Crouch icon
    /// </summary>
    [SerializeField] private Image _crouchIcon;

    /// <summary>
    /// Stand icon
    /// </summary>
    [SerializeField] private Image _standIcon;

    /// <summary>
    /// Сменить положение
    /// </summary>
    [SerializeField] private Button _changeStateButton;

    private CharacterMovement _characterMovement;

    void Start()
    {
        _characterMovement = _character.GetComponent<CharacterMovement>();
        _changeStateButton.onClick.AddListener(ChangeState);
    }

    // Update is called once per frame
    void Update()
    {
        var isCrouching = _characterMovement.IsCrouching;

        _crouchIcon.enabled = !isCrouching;
        _standIcon.enabled = isCrouching;
    }

    /// <summary>
    /// Сменить положение
    /// </summary>
    void ChangeState()
    {
        _characterMovement.SetCrouching(!_characterMovement.IsCrouching);
    }
}
