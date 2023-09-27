using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonCamera : MonoBehaviour
{
    // Variables
    public Transform player;
    public float mouseSensitivity = 2f;
    public float cameraVerticalRotation = 0f;
    public float cameraHorizontalRotation = 0f;


    float cameraHorizontalRotationY;
    float cameraHorizontalRotationZ;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Locked;

        cameraHorizontalRotationY = cameraHorizontalRotation - 90f;
        cameraHorizontalRotationZ = cameraHorizontalRotation + 90f;
    }

    // Update is called once per frame
    void Update()
    {
        // Collect Mouse Input
        float inputX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float inputY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        // Rotate the Camera around its local X axis
        cameraVerticalRotation -= inputY;
        cameraVerticalRotation = Mathf.Clamp(cameraVerticalRotation, -90f, 90f);
        transform.localEulerAngles = Vector3.right * cameraVerticalRotation;

        // Rotate the player around its local y axis
        cameraHorizontalRotation += inputX;
        cameraHorizontalRotation = Mathf.Clamp(cameraHorizontalRotation, cameraHorizontalRotationY, cameraHorizontalRotationZ);
        player.transform.localEulerAngles = Vector3.up * cameraHorizontalRotation;
        
    }

    public void ChangePlayerTransform(Transform newPlayer)
    {
        player = newPlayer;
    }

    public void ReleaseCursor()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
}
