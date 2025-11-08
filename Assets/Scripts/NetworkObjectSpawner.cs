using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
public class NetworkObjectSpawner
{
    public static GameObject SpawnNetworkObject(GameObject prefab,Vector3 position = default, Quaternion rotation = default) { 
        
        GameObject netGameObject = Object.Instantiate(prefab, position,rotation);
        NetworkObject netObject = netGameObject.GetComponent<NetworkObject>();
        netObject.Spawn();
        return netGameObject;
    }
    public static GameObject SpawnNetworkObjectWithOwnershipToClient(GameObject prefab,
        ulong clientId,
        Vector3 position = default,
        Quaternion rotation = default)
    {
        GameObject netGameObject = Object.Instantiate(prefab, position, rotation);
        NetworkObject netObject = netGameObject.GetComponent<NetworkObject>();
        netObject.SpawnWithOwnership(clientId);
        return netGameObject;
    }
}
