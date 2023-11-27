using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowingCamera : MonoBehaviour
{
    /// <summary>
    /// Персонаж
    /// </summary>
    [Header("Object to follow")]
    [SerializeField] private GameObject _mainCharacter;

    /// <summary>
    /// Скорость с которой камера возвращается к игроку, с эффектом отставания
    /// </summary>
    [Header("Camera settings")]
    [SerializeField] private float _returnSpeed;

    /// <summary>
    /// Высота расположения камеры 
    /// </summary>
    [SerializeField] private float _height;

    /// <summary>
    /// На каком расстоянии находится камера от игрока
    /// </summary>
    [SerializeField] private float _rearDistance;

    void Start()
    {
        transform.position = new Vector3(_mainCharacter.transform.position.x, _mainCharacter.transform.position.y + _height, _mainCharacter.transform.position.z - _rearDistance);
        transform.rotation = Quaternion.LookRotation(_mainCharacter.transform.position - transform.position);
    }

    void Update()
    {
        UpdateCameraPosition();
    }

    private void UpdateCameraPosition()
    {
        var currentVector = new Vector3(_mainCharacter.transform.position.x, _mainCharacter.transform.position.y + _height,
            _mainCharacter.transform.position.z - _rearDistance);
        transform.position = Vector3.Lerp(transform.position, currentVector, _returnSpeed * Time.deltaTime);
    }
}
