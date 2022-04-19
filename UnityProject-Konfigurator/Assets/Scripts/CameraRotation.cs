using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraRotation : MonoBehaviour
{
    [Header("Camera Rotation Target")]
    public Transform Target;

    [Header("Speed Floats")]
    [SerializeField] private float speed = 0.9f;
    [SerializeField] private float _smoothAnimationTime = 1f;


    [Tooltip("Mouse sensitivity - affects inertia")]
    [SerializeField] private float _sensitivity = 1f;
    private float _horizontal = 0f, _vertical = 0f;
    private float _saveHorizontalSpeed, _saveVerticalSpeed;
    
    // Poslední pozice, kdy byl nastaven Free Look, kvůli vracení kamery do původního stavu předtím, než se spustila animace
    private Vector3 _lastFreeLookPointPos, _lastFreeLookPointRot;

    [Header("View Target Transforms")]
    public Transform WheelTarget, SpoilerTarget;

    // booleans - _canMove použit u animací, abych se nemohl volně pohybovat - _isViewingWheel & Spoiler jako check abych věděl, co sleduju - _isDragging pro setrvačnost
    private bool _canMove = true;
    private bool _isViewingWheel = false, _isViewingSpoiler = false;
    private bool _isDragging = false;

    [Header("Clamp Values")]
    [SerializeField] private float minClamp = -35;
    [SerializeField] private float maxClamp = 25;

    void Update()
    {
        if (!_canMove) return;

        _vertical = Mathf.Clamp(_vertical, minClamp, maxClamp);
        Target.rotation = Quaternion.Euler(_vertical, _horizontal, 0f); //  jsou otočený axis, vertical je x axis a horizontal y, to je, protože x se otáčí nahoru a dolu a y doprava a doleva
        
        if (Input.GetKey(KeyCode.Mouse0))
        {
            _horizontal += Input.GetAxis("Mouse X") * _sensitivity;
            _vertical -= Input.GetAxis("Mouse Y") * _sensitivity; // mínus, protože to chci obráceně :P

            transform.LookAt(Target); // Kamera se rotuje na základě středu auta
            _isDragging = true;

            _saveHorizontalSpeed = Input.GetAxis("Mouse X"); // Uložení hor. rychlosti
            _saveVerticalSpeed = -Input.GetAxis("Mouse Y"); // Uložení vert. rychlosti

        } else if (Input.GetKeyUp(KeyCode.Mouse0) && _isDragging)
        {
            _isDragging = false;
        } else if (!_isDragging) // Když nepoužívám myš pro otáčení, tak se stane blok kódu pod
        {
            // Some spooky math
            _saveHorizontalSpeed = Mathf.Lerp(_saveHorizontalSpeed, 0f, speed); // Prostě získám hodnotu mezi saveHorizontalSpeed a 0 dle rychlosti se to snižuje (intertia)
            _saveVerticalSpeed = Mathf.Lerp(_saveVerticalSpeed, 0f, speed); // To samý zde :)

            _horizontal += _saveHorizontalSpeed; // Přidávám hodnoty k horizontal a vertical, protože přes tyto dvě proměnné rotuji kameru
            _vertical += _saveVerticalSpeed;
        }
    }

    public enum AnimationType // Enum typu animací - lepší než index (přehlednější)
    {
        FreeLook,
        Wheel,
        Spoiler
    };

    public void Animation(int animationTypeIndex) // Braní si indexu, jelikož si nemůžu v onClick funkci vybrat funkci s Enumem, takže musím brát index
    {
        AnimationType type = (AnimationType) animationTypeIndex; // převod indexu na enum
        
        if (!_isViewingWheel && !_isViewingSpoiler)
        {
            _lastFreeLookPointPos = transform.position; // Ukládání si pozice a rotace (řádek pod), aby potom bylo možné vrátit se do původní polohy, kde byla kamera, když byl ještě uživatel ve Free Looku
            _lastFreeLookPointRot = transform.eulerAngles;
        }

        if (type == AnimationType.Wheel && !_isViewingWheel)
        {
            transform.DOMove(WheelTarget.position, _smoothAnimationTime); // Animace přes Dotween
            transform.DORotate(WheelTarget.eulerAngles, _smoothAnimationTime);
            _isViewingWheel = true;
            _isViewingSpoiler = false;
            _canMove = false;
        } else if (type == AnimationType.Spoiler && !_isViewingSpoiler)
        {
            transform.DOMove(SpoilerTarget.position, _smoothAnimationTime);
            transform.DORotate(SpoilerTarget.eulerAngles, _smoothAnimationTime);
            _isViewingWheel = false;
            _isViewingSpoiler = true;
            _canMove = false;
        } else if (type == AnimationType.FreeLook && (_isViewingWheel || _isViewingSpoiler))
        {
            _canMove = true;
            transform.DOMove(_lastFreeLookPointPos, _smoothAnimationTime).OnComplete(() => _canMove = true);
            transform.DORotate(_lastFreeLookPointRot, _smoothAnimationTime);
            _isViewingWheel = false;
            _isViewingSpoiler = false;
        }
    }
}