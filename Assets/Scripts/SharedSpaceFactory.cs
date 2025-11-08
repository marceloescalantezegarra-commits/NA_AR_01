using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Niantic.Lightship.SharedAR.Colocalization;
using Unity.Netcode;
using System;


public class SharedSpaceFactory : MonoBehaviour
{
    [SerializeField] private SharedSpaceManager sharedSpaceManager;
    [SerializeField] private GameObject roomOriginPrefab;

    [Header("Room Options")]
    [SerializeField] private string roomName = "000";
    [SerializeField] private int capacity = 10;
    [SerializeField] private string description = "XR SENATI Room";


    public event Action OnSharedSpaceTracking;
    // Start is called before the first frame update
    void Start()
    {
        sharedSpaceManager.sharedSpaceManagerStateChanged += SharedSpaceManagerStateChanged;
    }

    private void SharedSpaceManagerStateChanged(SharedSpaceManager.SharedSpaceManagerStateChangeEventArgs args)
    {
        if (args.Tracking) {
            bool hasToStartNetwork = !NetworkManager.Singleton.IsConnectedClient && !NetworkManager.Singleton.IsHost;
            if (hasToStartNetwork) {
                OnSharedSpaceTracking?.Invoke();
                Instantiate(roomOriginPrefab, sharedSpaceManager.SharedArOriginObject.transform, false);
            }
        }
    }

    public void CreateRoom(ISharedSpaceTrackingOptions trackingOptions, string roomName) 
    {
        this.roomName = roomName;
        var roomOptions = ISharedSpaceRoomOptions.CreateLightshipRoomOptions(roomName,
                                                                             capacity,
                                                                             description);
        sharedSpaceManager.StartSharedSpace(trackingOptions, roomOptions);
    }
}
