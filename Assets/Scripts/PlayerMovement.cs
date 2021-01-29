using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour {
    [SerializeField] private Camera _camera = null;
    [SerializeField] private Transform _target = null;
    [SerializeField] private float _speed = 0f;
    [SerializeField] private float _speedRotation = 0f;
    [SerializeField] private float _jumpSpeed = 0f;
    [SerializeField] private int _jumpFrameTime = 0;
    [SerializeField] private CharacterController _characterController = null;

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
        //_camera.transform.rotation = startRotation * verticalRotation * horizontalRotarion;
        _target.rotation = startRotation * verticalRotation * horizontalRotarion;
    }

    private void Move(Vector3 vel) {
        vel.x *= _speed * Time.deltaTime;
        vel.z *= _speed * Time.deltaTime;
        _characterController.Move(vel);
    }
    
    public bool CanMove = false;
    public bool CanShoot = false;

    private void Update()
    {
        // движение камеры и поворот player
        Rotate(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 velocity = transform.right * x + transform.forward * z;
        velocity.y -= 9.8f * Time.deltaTime;

        if (_characterController.isGrounded)
        {
            // velocity.y = 0;
            if (Input.GetKeyDown(KeyCode.Space))
            {
                // velocity.y = _cameramanMovement.JumpSpeed * Time.deltaTime;
                // velocity.y += Mathf.Sqrt(_cameramanMovement.JumpSpeed * -2f * -9.8f);
                StartCoroutine(Jump());
            };
        }
        //velocity.y -= 9.8f * Time.deltaTime;
        Move(velocity);
        
    }


  

    private IEnumerator Jump() {
        var k = 0.01f;
        for (int i = 0; i < _jumpFrameTime; i++) {
            Vector3 velocity = Vector3.zero;
            velocity.y += Mathf.Sqrt((_jumpSpeed - k) * -2f * -9.8f);
            k += k;
            Move(velocity);
            yield return null;
        }
    }
}
