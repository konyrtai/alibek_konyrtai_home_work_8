using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
    [SerializeField] private CharacterMovement _characterMovement;

    /// <summary>
    /// Количество здоровья 
    /// </summary>
    public float Health { get; set; } = 100f;

    /// <summary>
    /// Значение урона
    /// </summary>
    [SerializeField] private float DamageValue { get; set; } = 10f;

    /// <summary>
    /// Значение лечения
    /// </summary>
    [SerializeField] private float HealValue { get; set; } = 10f;

    #region Animator

    /// <summary>
    /// Animator controller
    /// </summary>
    private Animator _animator;

    /// <summary>
    /// Hash dance animation в animator controller
    /// </summary>
    private int _danceHash;

    /// <summary>
    /// Hash hit animation в animator controller
    /// </summary>
    private int _hitHash;

    #endregion

    #region UI

    [SerializeField] private Image HealthBar;

    [SerializeField] private Button HitButton;

    [SerializeField] private Button HealButton;

    #endregion


    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();

        _characterMovement= GetComponent<CharacterMovement>();

        _hitHash = Animator.StringToHash("Hit");
        _danceHash = Animator.StringToHash("Dance");

        HitButton.onClick.AddListener(() => Damage(DamageValue));
        HealButton.onClick.AddListener(() => Heal(HealValue));
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void Heal(float value)
    {
        if(value <= 0)
            return;

        _characterMovement.StopCharacter();

        Health = 100;

        HealthBar.fillAmount = Mathf.Clamp(Health,0 , 100);

        _animator.Play(_danceHash);
    }

    public void Damage(float value)
    {
        if(value <= 0)
            return;

        _characterMovement.StopCharacter();
        
        if (Health - value <= 0)
        {
            Health = value;
        }
        else
        {
            Health -= value;
        }

        HealthBar.fillAmount = Health / 100f;

        _animator.Play(_hitHash);
    }
}
