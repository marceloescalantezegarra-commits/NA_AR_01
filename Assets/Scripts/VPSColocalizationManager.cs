using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Niantic.Lightship.AR.LocationAR;
using Niantic.Lightship.SharedAR.Colocalization;
using System.Linq;
using Niantic.Lightship.AR.VpsCoverage;
using Unity.Netcode;

[RequireComponent(typeof(SharedSpaceFactory))]
public class VPSColocalizationManager : MonoBehaviour, IColocalizationRoom
{
    [SerializeField] private ARLocationManager locationManager;
    [SerializeField] private GameObject connectionUI;

    private SharedSpaceFactory sharedSpaceFactory;
    private void OnEnable()
    {
        sharedSpaceFactory = GetComponent<SharedSpaceFactory>();
        sharedSpaceFactory.OnSharedSpaceTracking += SharedSpaceStartTracking;
    }
    private void OnDisable()
    {
        sharedSpaceFactory.OnSharedSpaceTracking -= SharedSpaceStartTracking;
    }
    // Start is called before the first frame update
    void Start()
    {
        CreateRoomOptions();
    }
    public void CreateRoomOptions()
    {
        string roomName = locationManager.ARLocations.First().Payload.ToBase64();
        var vpsTrackingOptions = ISharedSpaceTrackingOptions.CreateVpsTrackingOptions(locationManager.ARLocations.First());
        sharedSpaceFactory.CreateRoom(vpsTrackingOptions, roomName);
    }

    public void SharedSpaceStartTracking()
    {
        connectionUI.SetActive(true);
    }

    public void StartClient()
    {
        NetworkManager.Singleton.StartClient();    
        connectionUI.SetActive(false);
    }

    public void StartHost()
    {
        NetworkManager.Singleton.StartHost();
        connectionUI.SetActive(false);
    }
}
    

