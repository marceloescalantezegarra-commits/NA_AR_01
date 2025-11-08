using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class NetPlayerColor : NetworkBehaviour
{
    [HideInInspector]
    public NetworkVariable<Color> MyPlayerColor = new NetworkVariable<Color>(Color.white, 
        NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    [SerializeField] private List<Color> playerColors = new List<Color>();

    [SerializeField] private SpriteRenderer targetSprite;
    [SerializeField] private SpriteRenderer circleSprite;
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private TextMesh textMesh;

    private void Awake()
    {
        MyPlayerColor.OnValueChanged += PlayerColorChanged;
    }
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        if (IsOwner)
        {
            int indexColor = (int)OwnerClientId % playerColors.Count;
            MyPlayerColor.Value = playerColors[indexColor];

            NetControllerManager.Instance.MyPlayerColor = MyPlayerColor.Value;
        }
        else { 
            //targetSprite.color = MyPlayerColor.Value;
            PlayerColorChanged(Color.white,MyPlayerColor.Value);
        }
    
    }
    private void PlayerColorChanged(Color previous, Color current) { 
        targetSprite.color = current;
        circleSprite.color = current;
        lineRenderer.startColor = current;
        lineRenderer.endColor = current;
        textMesh.color = current;

    }
}
