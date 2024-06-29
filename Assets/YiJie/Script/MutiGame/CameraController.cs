using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CameraController : MonoBehaviour
{
    public float moveSpeed = 40f; // 視角移動速度
    private Camera camera;
    private PhotonView Photonview;
    void Start()
    {
        camera = GetComponent<Camera>();
        Photonview = this.GetComponentInParent<PhotonView>();
        if (Photonview.IsMine)
        {
            // camera.enabled = true;
            camera.gameObject.SetActive(true);
        }
        else
        {
            //camera.enabled = false;
            camera.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if (Photonview.IsMine)
        {
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");

            Vector3 moveDirection = new Vector3(horizontalInput, verticalInput, 0f).normalized;

            Vector3 newPosition = transform.position + moveDirection * moveSpeed * Time.deltaTime;

            //newPosition.z = -10f;
            transform.position = newPosition;
        }
    }
}
