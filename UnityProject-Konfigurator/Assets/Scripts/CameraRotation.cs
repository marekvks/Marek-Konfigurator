using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraRotation : MonoBehaviour
{
    public Transform carCenter;
    public Transform target;
    public float sensitivity = 1f;

    float smoothTime = 1f;
    float velocity;

    private Vector3 lastFreeLookPointPos;
    private Vector3 lastFreeLookPointRot;
    public Transform[] positions = new Transform[1];

    float xRot = 0f;

    public bool _canMove = true;

    bool _isViewingWheel;
    bool _isViewingSpoiler;

    float horizontal = 0f, vertical = 0f;

    void Update()
    {
        if (Input.GetKey(KeyCode.Mouse0) && _canMove)
        {
            horizontal += Input.GetAxis("Mouse X") * sensitivity;
            vertical -= Input.GetAxis("Mouse Y") * sensitivity;     // mínus, protože to chci obráceně :P
            vertical = Mathf.Clamp(vertical, -60, 25);      // clamp - neklesne pod "-60" a nestoupne nad "25", nemám tam 0, 90, protože to nefunguje ze super důvodu, tak jsem našel jiný souřadnice

            transform.LookAt(target);
            target.rotation = Quaternion.Euler(vertical, horizontal, 0f);       //  jsou otočený axis, vertical je x axis a horizontal y, to je, protože x se otáčí nahoru a dolu a y doprava a doleva

            /*Vector3 dir = new Vector3(vertical, horizontal, 0f);      !!! Tenhle kód sice funguje (teda z části), ale je zbytečně těžký a není clampnutý !!!

            dir.z = 0f;

            if (canMove) { transform.RotateAround(new Vector3(0, 0, 0), Vector3.right, vertical * Time.deltaTime); }
            transform.RotateAround(new Vector3(0, 0, 0), Vector3.up, horizontal * Time.deltaTime);

            Vector3 eulerRotation = transform.rotation.eulerAngles;
            transform.rotation = Quaternion.Euler(eulerRotation.x, eulerRotation.y, 0f);*/

        }
        Animations();
    }

    private void Animations()
    {
        if (!_isViewingWheel && !_isViewingSpoiler)
        {
            lastFreeLookPointPos = transform.position;
            lastFreeLookPointRot = transform.eulerAngles;
        }

        if (Input.GetKeyDown(KeyCode.F) && !_isViewingWheel)
        {
            _canMove = false;
            transform.DOMove(positions[0].position, smoothTime);
            transform.DORotate(positions[0].eulerAngles, smoothTime);
            _isViewingWheel = true;
            _isViewingSpoiler = false;
        } else if (Input.GetKeyDown(KeyCode.G) && !_isViewingSpoiler)
        {
            _canMove = false;
            transform.DOMove(positions[1].position, smoothTime);
            transform.DORotate(positions[1].eulerAngles, smoothTime);
            _isViewingWheel = false;
            _isViewingSpoiler = true;
        } else if ((Input.GetKeyDown(KeyCode.F) && _isViewingWheel) || (Input.GetKeyDown(KeyCode.G) && _isViewingSpoiler))
        {
            transform.DOMove(lastFreeLookPointPos, smoothTime).OnComplete(() => _canMove = true);
            transform.DORotate(lastFreeLookPointRot, smoothTime);
            _isViewingWheel = false;
            _isViewingSpoiler = false;
        }
    }
}