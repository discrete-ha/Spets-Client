using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicCameraRotation : MonoBehaviour
{
    public void RotateUpDown(Transform avatarTransform, float axis)
    {
        Camera.main.transform.RotateAround(avatarTransform.position, Camera.main.transform.right, -axis * Time.deltaTime);
    }

    public void RotateRightLeft(Transform avatarTransform, float axis)
    {
        Camera.main.transform.RotateAround(avatarTransform.position, Vector3.up, -axis * Time.deltaTime);
    }
}
