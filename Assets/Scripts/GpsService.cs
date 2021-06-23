using UnityEngine;
using System.Collections;
using Google.Maps.Coord;
using UnityEngine.UI;

public class GpsService : MonoBehaviour
{
    public static GpsService Instance { set; get; }
    //public LatLng latLon;
    public static bool gpsStarted = false;

    private void Start()
    {

        StartCoroutine(GpsStart());
    }

    public IEnumerator GpsStart()
    {
        Debug.Log("LocationService start");
        // First, check if user has location service enabled
        //if (!Input.location.isEnabledByUser)
        //{
        //    Debug.Log("LocationService is Disabled");
        //    yield break;
        //}

        // Start service before querying location
        Input.location.Start();
       
        // Wait until service initializes
        int maxWait = 20;
        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            yield return new WaitForSeconds(1);
            maxWait--;
        }

        // Service didn't initialize in 20 seconds
        if (maxWait < 1)
        {
            print("Timed out");
            yield break;
        }

        // Connection has failed
        if (Input.location.status == LocationServiceStatus.Failed)
        {
            print("Unable to determine device location");
            yield break;
        }
        else
        {
            Debug.Log("location service started");
            // Access granted and location value could be retrieved
            //print("Location: " + Input.location.lastData.latitude + " " + Input.location.lastData.longitude + " " + Input.location.lastData.altitude + " " + Input.location.lastData.horizontalAccuracy + " " + Input.location.lastData.timestamp);
            //latLon = new LatLng(Input.location.lastData.latitude, Input.location.lastData.longitude);
            gpsStarted = true;
        }

        // Stop service if there is no need to query location updates continuously
        //Input.location.Stop();
    }

    public static void GpsStop()
    {
        if (Input.location.isEnabledByUser)
        {
            gpsStarted = false;
            Input.location.Stop();
        }
    }
}