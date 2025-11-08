using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
public class NetObjectColor : NetworkBehaviour
{
    [HideInInspector]
    public NetworkVariable<Vector3> PlayerColorRGB = new NetworkVariable<Vector3>(Vector3.zero);
    [SerializeField] private Renderer objRenderer;

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        SetColor(Vector3.zero, PlayerColorRGB.Value);
    }
    private void Awake()
    {
        PlayerColorRGB.OnValueChanged += SetColor;

    }
    private void SetColor(Vector3 previous, Vector3 current) { 
        Color color = new Color(current.x, current.y, current.z);
        objRenderer.material.color = color;
    }
}
