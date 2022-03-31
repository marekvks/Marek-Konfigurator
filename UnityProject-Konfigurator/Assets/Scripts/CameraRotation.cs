using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraRotation : MonoBehaviour
{
    public Transform carCenter;
    public Transform target;

    public float speed = 0.9f;
    float smoothTime = 1f;

    public float sensitivity = 1f;
    float horizontal = 0f, vertical = 0f;
    float saveHorizontalSpeed, saveVerticalSpeed;

    private Vector3 lastFreeLookPointPos, lastFreeLookPointRot;
    public Transform[] positions = new Transform[1];

    public bool _canMove = true;
    bool _isViewingWheel = false, _isViewingSpoiler = false;

    bool _isDragging = false;

    void Update()
    {
        vertical = Mathf.Clamp(vertical, -30, 25);
        target.rotation = Quaternion.Euler(vertical, horizontal, 0f);       //  jsou otočený axis, vertical je x axis a horizontal y, to je, protože x se otáčí nahoru a dolu a y doprava a doleva
        
        if (Input.GetKey(KeyCode.Mouse0) && _canMove)
        {
            horizontal += Input.GetAxis("Mouse X") * sensitivity;
            vertical -= Input.GetAxis("Mouse Y") * sensitivity;     // mínus, protože to chci obráceně :P

            transform.LookAt(target);   // Kamera se rotuje na základě středu auta
            _isDragging = true;

            saveHorizontalSpeed = Input.GetAxis("Mouse X"); // Uložení hor. rychlosti
            saveVerticalSpeed = -Input.GetAxis("Mouse Y");  // Uložení vert. rychlosti

        } else if (Input.GetKeyUp(KeyCode.Mouse0) && _isDragging)
        {
            _isDragging = false;
        } else if (!_isDragging)  // Když nepoužívám myš pro otáčení, tak se stane blok kódu pod
        {
            // Some spooky math
            saveHorizontalSpeed = Mathf.Lerp(saveHorizontalSpeed, 0f, speed); // Prostě získám hodnotu mezi saveHorizontalSpeed a 0 dle rychlosti se to snižuje (intertia)
            saveVerticalSpeed = Mathf.Lerp(saveVerticalSpeed, 0f, speed);     // To samý zde :)

            horizontal += saveHorizontalSpeed;  // Přidávám hodnoty k horizontal a vertical, protože přes tyto dvě proměnné rotuji kameru
            vertical += saveVerticalSpeed;
        }
    }

    public void Animations(int animationIndex) // 0 - Free Look, 1 - Wheel, 2 - Spoiler
    {
        if (!_isViewingWheel && !_isViewingSpoiler)
        {
            lastFreeLookPointPos = transform.position;
            lastFreeLookPointRot = transform.eulerAngles;
        }

        if (animationIndex == 1 && !_isViewingWheel)
        {
            _canMove = false;
            transform.DOMove(positions[0].position, smoothTime);
            transform.DORotate(positions[0].eulerAngles, smoothTime);
            _isViewingWheel = true;
            _isViewingSpoiler = false;
            _canMove = false;
        } else if (animationIndex == 2 && !_isViewingSpoiler)
        {
            _canMove = false;
            transform.DOMove(positions[1].position, smoothTime);
            transform.DORotate(positions[1].eulerAngles, smoothTime);
            _isViewingWheel = false;
            _isViewingSpoiler = true;
        } else if (animationIndex == 0 && (_isViewingWheel || _isViewingSpoiler))
        {
            transform.DOMove(lastFreeLookPointPos, smoothTime).OnComplete(() => _canMove = true);
            transform.DORotate(lastFreeLookPointRot, smoothTime);
            _isViewingWheel = false;
            _isViewingSpoiler = false;
        }
    }
}