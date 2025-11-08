using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.Netcode;
using Niantic.Lightship.SharedAR.Colocalization;
using Niantic.Protobuf.WellKnownTypes;
public class ARRayInteractor : NetworkBehaviour
{
    [SerializeField] private GameObject hitPointPrefab;
    [SerializeField] private NetPlayerColor netPLayerColor;
    [SerializeField] private bool spawnObjectAtHitPoint;
    private Camera aRCamera;
    private SharedSpaceManager sharedSpaceManager;

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        if (!IsOwner) { 
            enabled = false;
        }
    }
    void Start()
    {
        aRCamera = Camera.main;
        sharedSpaceManager = aRCamera.transform.root.GetComponent<SharedSpaceManager>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began))
        {
            Vector3 inputPosition = Input.GetMouseButtonDown(0) ? Input.mousePosition : Input.GetTouch(0).position;

            if (InputExtentions.IsTouchOverUI(inputPosition))
            {
                return;
            }

            Ray ray = aRCamera.ScreenPointToRay(inputPosition);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                //Do something when Ray hits something
                if (spawnObjectAtHitPoint) {
                    SpawnObjectAtHitPoint(hit.point);
                }

                NetControllerManager.Instance.SpawnOrMoveCharacter(hit.point);
            }
        }
    }
    private void SpawnObjectAtHitPoint(Vector3 hitPoint) { 
        Vector3 posRelativeToSharedOrigin = 
            sharedSpaceManager.SharedArOriginObject.transform.InverseTransformPoint(hitPoint);
        SpawnObjectAtHitPointServerRpc(posRelativeToSharedOrigin);
    }

    [ServerRpc]
    private void SpawnObjectAtHitPointServerRpc(Vector3 spawnPosition) { 
        GameObject netGameObject = NetworkObjectSpawner.SpawnNetworkObject(hitPointPrefab, spawnPosition);
        // Set color acording to player
        Vector3 colorRGB = new Vector3(netPLayerColor.MyPlayerColor.Value.r, netPLayerColor.MyPlayerColor.Value.g, netPLayerColor.MyPlayerColor.Value.b);
        netGameObject.GetComponent<NetObjectColor>().PlayerColorRGB.Value = colorRGB;
    }
}
