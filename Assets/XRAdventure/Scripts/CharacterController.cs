using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{

    [SerializeField] private float speed = 0.75f;
    [SerializeField] private Animator objectAnimator;
    private Vector3 targetPosition;
    private bool isMoving;

    void Update()
    {
        if (!isMoving)
        {
            return;
        }
        MoveARObject();
    }

    public void SetDestination(Vector3 position)
    {
        targetPosition = position;
        isMoving = true;
        PlayAnimation(1);
    }

    private void MoveARObject()
    {
        float step = speed * Time.deltaTime;
        Vector3 direction = (targetPosition - transform.position).normalized;
        transform.Translate(Vector3.forward * step, Space.Self);

        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(direction, Vector3.up), 8.0f);

        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            isMoving = false;
            PlayAnimation(0);
        }
    }

    private void PlayAnimation(float animParam)
    {
        objectAnimator.SetFloat("Walk", animParam);
    }
}
