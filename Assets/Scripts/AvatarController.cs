using UnityEngine;
using System.Collections;
using Spets.Airflare.Shared;
using Google.Maps.Coord;
using System;

public class AvatarController : MonoBehaviour
{
    public static GameObject Selected;
    public static GameObject avatar;
    public GameObject nightLight;

    public Transform CameraRig;

    private Gyroscope Gyro;
    private bool GyroSupported;
    private Transform previousTransform;
    private LatLng previousLatLng;
    public static bool relocated = false;
    bool isMoving = false;

    private float turningSpeed = 1f;

    private void Start()
    {
        SelectAvatar("RHINOCEROS");
    }

    void SelectAvatar(string name)
    {
        avatar = transform.gameObject;
        GameObject[] animals = GameObject.FindGameObjectsWithTag("Animals");
        Debug.Log("animals:" + animals.Length);
        foreach (GameObject animal in animals)
        {
            animal.SetActive(false);
        }
        Selected = transform.Find("Animals/"+ name).gameObject;
        Selected.SetActive(true);
        SwitchAnimation("idleBreathe");
    }

    void SwitchAnimation(string moveName)
    {
        Animation avatarAnimation = Selected.GetComponent<Animation>();
        avatarAnimation.Play(moveName);
    }

    void Update()
    {
        //Selected.transform.position = CityView.DynamicMapsService.transform.position;
        //RotateHeading();
        //GyroModifyAvatar();

        if (Input.location.status == LocationServiceStatus.Running)
        {
            try
            {
                LatLng moveLatLng = new LatLng(Input.location.lastData.latitude, Input.location.lastData.longitude);
                Vector3 coordForNewPosition = CityView.DynamicMapsService.MapsService.Projection.FromLatLngToVector3(moveLatLng);
                var avatarPosition = transform.position;
                if (moveLatLng.Lat != previousLatLng.Lat || moveLatLng.Lng != previousLatLng.Lng)
                {
                    SpetsLogger.Show("moving");
                    isMoving = true;
                    SwitchAnimation("walk");

                   
                    if (previousTransform != null)
                    {
                        
                        float dist = Vector3.Distance(avatarPosition, previousTransform.position);
                        if(dist != 0)
                            SpetsLogger.Show("moving:"+ dist);

                        if (dist > 100)
                        {
                            CityView.DynamicMapsService.MapsService.MoveFloatingOrigin(moveLatLng);
                            previousTransform = transform;
                            relocated = true;
                        }
                    }
                    else
                    {
                        CityView.DynamicMapsService.MapsService.MoveFloatingOrigin(moveLatLng);
                        previousTransform = transform;
                    }
                    previousLatLng = moveLatLng;
                }

                if(isMoving)
                    MoveAvatar(coordForNewPosition);

                CameraFollowAvatar();
                RotateAvatarToTarget(coordForNewPosition);
            }
            catch (Exception err)
            {
                SpetsLogger.Show("error:" + err);
            }

        }
    }


    void RotateAvatarToTarget(Vector3 newPosition)
    {
        float singleStep = turningSpeed * Time.deltaTime;
        Vector3 newDirection = Vector3.RotateTowards(transform.forward, newPosition - transform.position, singleStep, 0.0f);
        Debug.DrawRay(transform.position, newDirection, Color.red);

        transform.rotation = Quaternion.LookRotation(newDirection);

        //transform.LookAt(newPosition);
    }

    void MoveAvatar(Vector3 newPosition)
    {
        
        float dist = Vector3.Distance(transform.position, newPosition);
        SpetsLogger.Show("MoveAvatar : "+ newPosition + "-" + dist );
        if(dist > 0.2f)
        {
            transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime);
        }
        else
        {
            SwitchAnimation("idleEat");
            isMoving = false;
        }
            
    }

    void CameraFollowAvatar()
    {
        //SpetsLogger.Show("CameraFollowAvatar");
        CameraRig.transform.position = transform.position;
    }

    void GyroModifyAvatar()
    {
        if (GyroSupported)
        {
            Selected.transform.rotation = GyroToUnity(Input.gyro.attitude);
        }
    }

    private static Quaternion GyroToUnity(Quaternion q)
    {
        return new Quaternion(0, q.y, 0, -q.w);
    }

    private Vector3 GetStampPosition(int index)
    {
        LatLng latLng = new LatLng(Input.location.lastData.latitude, Input.location.lastData.longitude);
        //LatLng latLng = LatLngs[index];

        return CityView.DynamicMapsService.MapsService.Projection.FromLatLngToVector3(latLng);
    }
}
