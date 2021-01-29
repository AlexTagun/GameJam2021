using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameramanInGameController : MonoBehaviour {
    [SerializeField] private CameramanMovement _cameramanMovement = null;
    [SerializeField] private CharacterController _characterController = null;

    public bool CanMove = false;
    public bool CanShoot = false;

    private void Update()
    {
        // движение камеры и поворот player
        _cameramanMovement.Rotate(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        
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
        _cameramanMovement.Move(velocity);
        
        }


  

    private IEnumerator Jump() {
        var k = 0.01f;
        for (int i = 0; i < _cameramanMovement.JumpFrameTime; i++) {
            Vector3 velocity = Vector3.zero;
            velocity.y += Mathf.Sqrt((_cameramanMovement.JumpSpeed - k) * -2f * -9.8f);
            k += k;
            _cameramanMovement.Move(velocity);
            yield return null;
        }
    }

}
