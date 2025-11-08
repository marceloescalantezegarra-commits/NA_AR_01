using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class NetControllerInitializer : NetworkBehaviour
{
    [SerializeField] private CharacterController characterController;
    [SerializeField] private NetObjectColor netObjectColor;

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        if (IsOwner)
        {
            NetControllerManager.Instance.CharacterController = characterController;
            Vector3 colorRGB = new Vector3(NetControllerManager.Instance.MyPlayerColor.r,
                                           NetControllerManager.Instance.MyPlayerColor.g,
                                           NetControllerManager.Instance.MyPlayerColor.b);
            ChangeColorServerRpc(colorRGB);
        }
        else { 
            characterController.enabled = false;
        }
    }

    [ServerRpc]
    public void ChangeColorServerRpc(Vector3 colorRGB) {
        netObjectColor.PlayerColorRGB.Value = colorRGB;
    }
}   
