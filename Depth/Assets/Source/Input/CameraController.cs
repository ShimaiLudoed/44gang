using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
  public float sens = 2f;
  public float max = 90f;

  private float rotX = 0f;

  private void Update()
  {
    float mouseX = Input.GetAxis("Mouse X");
    float mouseY = Input.GetAxis("Mouse Y");

    transform.parent.Rotate(Vector3.up * mouseX * sens);

    rotX -= mouseY * sens;
    rotX = Mathf.Clamp(rotX, -max, max);
    transform.localRotation = Quaternion.Euler(rotX, 0, 0);
  }
}
