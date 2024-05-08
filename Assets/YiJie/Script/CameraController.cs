using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float moveSpeed = 40f; // 視角移動速度
    void Start()
    {

    }

    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 moveDirection = new Vector3(horizontalInput, verticalInput, 0f).normalized;

        Vector3 newPosition = transform.position + moveDirection * moveSpeed * Time.deltaTime;

        //newPosition.z = -10f;
        transform.position = newPosition;
    }
}
