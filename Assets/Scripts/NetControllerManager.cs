using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using Niantic.Lightship.SharedAR.Colocalization;
using TMPro;

public class NetControllerManager : NetworkBehaviour
{
    public static NetControllerManager Instance;

    [SerializeField] private GameObject characterPrefab;
    [SerializeField] private SharedSpaceManager sharedSpaceManager;

    [HideInInspector]
    public Color MyPlayerColor;
    [HideInInspector]
    public CharacterController CharacterController;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else {
            Instance = this;
        }
    }
    public void SpawnOrMoveCharacter(Vector3 targetPosition) {
        if (CharacterController)
        {
            CharacterController.SetDestination(targetPosition);
            return;
        }
        SpawnObject(targetPosition);
        
        
    }
    public void SpawnObject(Vector3 position) {
        Vector3 posRelativeToSharedOrigin =
            sharedSpaceManager.SharedArOriginObject.transform.InverseTransformPoint(position);
        SpawnObjectServerRpc(posRelativeToSharedOrigin);
    }

    [ServerRpc(RequireOwnership = false)]
    public void SpawnObjectServerRpc(Vector3 spawnPosition, ServerRpcParams serverRpcParams = default) { 
        var clientId = serverRpcParams.Receive.SenderClientId;
        
        NetworkObjectSpawner.SpawnNetworkObjectWithOwnershipToClient(
            characterPrefab,
            clientId,
            spawnPosition
            );
    }
}
