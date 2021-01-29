using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameramanMovement : MonoBehaviour {
    [SerializeField] private Camera _camera = null;
    [SerializeField] private float _speed = 0f;
    [SerializeField] private float _speedRotation = 0f;
    [SerializeField] private float _jumpSpeed = 0f;
    [SerializeField] private int _jumpFrameTime = 0;
    [SerializeField] private CharacterController _characterController = null;



    public float JumpSpeed => _jumpSpeed;
    public int JumpFrameTime => _jumpFrameTime;
    // вращение камеры
    private float mouseDeltaX = 0f;
    private float mouseDeltaY = 0f;
    private Quaternion startRotation = Quaternion.identity;
    private Quaternion verticalRotation = Quaternion.identity;
    private Quaternion horizontalRotarion = Quaternion.identity;



    private void Awake() {
        startRotation = transform.rotation;
    }

    public void Rotate(float mouseDeltaX, float mouseDeltaY) {
        this.mouseDeltaX += mouseDeltaX * _speedRotation;
        this.mouseDeltaY += mouseDeltaY * _speedRotation;
        this.mouseDeltaY = Mathf.Clamp(this.mouseDeltaY, -60, 60);
        this.gameObject.transform.rotation = Quaternion.AngleAxis(this.mouseDeltaX, Vector3.up);
        verticalRotation = Quaternion.AngleAxis(this.mouseDeltaX, Vector3.up);
        horizontalRotarion = Quaternion.AngleAxis(-this.mouseDeltaY, Vector3.right);
        _camera.transform.rotation = startRotation * verticalRotation * horizontalRotarion;
    }

    public void Move(Vector3 vel) {
        vel.x *= _speed * Time.deltaTime;
        vel.z *= _speed * Time.deltaTime;
        _characterController.Move(vel);
    }
}
