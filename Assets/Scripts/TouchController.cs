using System.Collections;
using System.Collections.Generic;
using Spets.Airflare.Shared;
using UnityEngine;

public class TouchController : BasicCameraRotation
{
    Vector3 firstPoint;
    float sensitivity = 5f;

    //public float movementTime = 1;
    //public float rotationSpeed = 0.1f;
    //public Transform avatarTransform;

    //bool isRotation = false;

    void Update()
    {
        TouchRotation();
    }

    void TouchRotation()
    {
        //isRotation = true;
        if (Input.touchCount > 0)
        {
            if (Input.GetTouch(0).phase == TouchPhase.Began)
            {
                firstPoint = Input.GetTouch(0).position;
            }
            if (Input.GetTouch(0).phase == TouchPhase.Moved)
            {
                Vector3 secondPoint = Input.GetTouch(0).position;

                float x = secondPoint.x - firstPoint.x;
                //RotateRightLeft(AvatarController.avatar.transform, x * sensitivity);
                //SpetsLogger.Show("AvatarController.avatar.transform.position :" + AvatarController.avatar.transform.position.x + "," + AvatarController.avatar.transform.position.y + ","+ AvatarController.avatar.transform.position.z);
                //SpetsLogger.Show("Camera.main.transform :" + Camera.main.transform.position.x + "," + Camera.main.transform.position.y + "," + Camera.main.transform.position.z);
                Camera.main.transform.RotateAround(AvatarController.avatar.transform.position, Vector3.up, -(x * Time.deltaTime * sensitivity));

                //var cameraRotationX = Camera.main.transform.eulerAngles.x;
                //SpetsLogger.Show("cameraRotationX :"+ cameraRotationX);
                //float y = secondPoint.y - firstPoint.y;
                //if ( ( cameraRotationX > 60 && cameraRotationX < 180 && y > 0) || ( cameraRotationX < 340 && cameraRotationX > 180 && y < 0))
                //{
                //    y = 0;
                //}

                //SpetsLogger.Show(" y :" + y);

                //if ( (cameraRotationX < 10 && y < 0) || (cameraRotationX > 60 && y > 0) )
                //{
                //    y = 0;
                //}

                ////InstructionsHandler.InstructionsText.text += "y * -sensitivit:" + (y * -sensitivity) + " \n";
                //RotateUpDown(avatarTransform, y * -sensitivity);
                firstPoint = secondPoint;
            }


        }
    }
}