using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class JoystickHandler : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    #region Joystick images

    /// <summary>
    /// ���� ������� ���������
    /// </summary>
    [Header("Joystick binding")]
    [SerializeField] private Image _joystickBackground;

    /// <summary>
    /// ������ ���������
    /// </summary>
    [SerializeField] private Image _joystick;

    /// <summary>
    /// ���� ������ ��������� (����� ������� ������ ����)
    /// </summary>
    [SerializeField] private Image _joystickArea;

    /// <summary>
    /// ���� ���� �������� �������
    /// </summary>
    [SerializeField] private Color _inactiveJoystickColor;

    /// <summary>
    /// ���� ���� �������� �� �������
    /// </summary>
    [SerializeField] private Color _activeJoystickColor;

    #endregion

    /// <summary>
    /// ��������� ��������� ���� ������� ���������
    /// </summary>
    private Vector2 _joystickBackgroundStartPosition;

    /// <summary>
    /// ����������� ��������
    /// </summary>
    protected Vector2 InputVector;

    /// <summary>
    /// ���� "�������� ������������"
    /// </summary>
    private bool _joystickIsActive = false;

    private void Start()
    {
        // ������������� ��������� ������� ���� ������� ���������
        _joystickBackgroundStartPosition = _joystickBackground.rectTransform.anchoredPosition;
    }

    /// <summary>
    /// ������������� ���������� ���������
    /// </summary>
    private void ClickEffect()
    {
        _joystickIsActive = !_joystickIsActive;
        _joystick.color = _joystickIsActive? _activeJoystickColor : _inactiveJoystickColor;
    }

    public void OnDrag(PointerEventData eventData)
    {
        // �������� �� ��������� ������� � ���� ������� ���������
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(_joystickBackground.rectTransform, eventData.position, null, out var joystickPosition) == false)
            return;

        joystickPosition.x = joystickPosition.x * 2 / _joystickBackground.rectTransform.sizeDelta.x;
        joystickPosition.y = joystickPosition.y * 2 / _joystickBackground.rectTransform.sizeDelta.y;

        InputVector = new Vector2(joystickPosition.x, joystickPosition.y);

        // ���� �������� ������ �������� �� ������, �� �����������, ���� ���, �� ��� ����� ����� ��� ������� �� �������� ������������
        if (InputVector.magnitude > 1f)
        {
            InputVector = InputVector.normalized;
        }

        // ������ ������� ������� ��������� 
        _joystick.rectTransform.anchoredPosition = new Vector2(
            InputVector.x * (_joystickBackground.rectTransform.sizeDelta.x / 2),
            InputVector.y * (_joystickBackground.rectTransform.sizeDelta.y / 2));
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        // ������ ���� ��������� �� ��������
        ClickEffect();

        // �������� �� ��������� ������� � ���� ������ ��������� (����� ���� ��� ����� ������ ���� ������) 
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(_joystickArea.rectTransform, eventData.position, null, out var _joystickBackgroundPosition) == false)
            return;

        // ����������� ���� ������� ��������� � ����� �������
        _joystickBackground.rectTransform.anchoredPosition = new Vector2(_joystickBackgroundPosition.x, _joystickBackgroundPosition.y);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        // ��������� �������� �� ������� �������
        _joystickBackground.rectTransform.anchoredPosition = _joystickBackgroundStartPosition;

        // ������ ���� ��������� �� �� ��������
        ClickEffect();
        
        // ���������� �����������
        InputVector = Vector2.zero;

        // ���������� ������
        _joystick.rectTransform.anchoredPosition = Vector2.zero;
    }
}
