using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class JoystickHandler : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    #region Joystick images

    /// <summary>
    /// Зона области джойстика
    /// </summary>
    [Header("Joystick binding")]
    [SerializeField] private Image _joystickBackground;

    /// <summary>
    /// Курсор джойстика
    /// </summary>
    [SerializeField] private Image _joystick;

    /// <summary>
    /// Зона работы джойстика (левая сторона экрана типа)
    /// </summary>
    [SerializeField] private Image _joystickArea;

    /// <summary>
    /// Цвет если джойстик активен
    /// </summary>
    [SerializeField] private Color _inactiveJoystickColor;

    /// <summary>
    /// Цвет если джойстик не активен
    /// </summary>
    [SerializeField] private Color _activeJoystickColor;

    #endregion

    /// <summary>
    /// Начальное положение зоны области джойстика
    /// </summary>
    private Vector2 _joystickBackgroundStartPosition;

    /// <summary>
    /// Направление движения
    /// </summary>
    protected Vector2 InputVector;

    /// <summary>
    /// Флаг "джойстик используется"
    /// </summary>
    private bool _joystickIsActive = false;

    private void Start()
    {
        // устанавливаем начальную позицию зоны области джойстика
        _joystickBackgroundStartPosition = _joystickBackground.rectTransform.anchoredPosition;
    }

    /// <summary>
    /// Устанавливаем активность джойстика
    /// </summary>
    private void ClickEffect()
    {
        _joystickIsActive = !_joystickIsActive;
        _joystick.color = _joystickIsActive? _activeJoystickColor : _inactiveJoystickColor;
    }

    public void OnDrag(PointerEventData eventData)
    {
        // проверка на попадание нажатия в зону области джойстика
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(_joystickBackground.rectTransform, eventData.position, null, out var joystickPosition) == false)
            return;

        joystickPosition.x = joystickPosition.x * 2 / _joystickBackground.rectTransform.sizeDelta.x;
        joystickPosition.y = joystickPosition.y * 2 / _joystickBackground.rectTransform.sizeDelta.y;

        InputVector = new Vector2(joystickPosition.x, joystickPosition.y);

        // если вытянули курсор джойстик на полную, то нормализуем, если нет, то это нужно будет для влияния на скорость передвижения
        if (InputVector.magnitude > 1f)
        {
            InputVector = InputVector.normalized;
        }

        // задаем позицию курсора джойстика 
        _joystick.rectTransform.anchoredPosition = new Vector2(
            InputVector.x * (_joystickBackground.rectTransform.sizeDelta.x / 2),
            InputVector.y * (_joystickBackground.rectTransform.sizeDelta.y / 2));
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        // меняем цвет джойстика на активный
        ClickEffect();

        // проверка на попадание нажатия в зону работы джойстика (левая зона где можно нажать куда угодно) 
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(_joystickArea.rectTransform, eventData.position, null, out var _joystickBackgroundPosition) == false)
            return;

        // перемещение зоны области джойстика в точку нажатия
        _joystickBackground.rectTransform.anchoredPosition = new Vector2(_joystickBackgroundPosition.x, _joystickBackgroundPosition.y);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        // возращаем джойстик на базовую позицию
        _joystickBackground.rectTransform.anchoredPosition = _joystickBackgroundStartPosition;

        // меняем цвет джойстика на не активный
        ClickEffect();
        
        // сбрасываем направление
        InputVector = Vector2.zero;

        // сбрасываем курсор
        _joystick.rectTransform.anchoredPosition = Vector2.zero;
    }
}
