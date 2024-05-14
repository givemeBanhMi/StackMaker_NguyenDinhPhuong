using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Vector3 _offset;
    private Transform target;
    [SerializeField] private float smoothTime;
    private Vector3 _currentVelocity = Vector3.zero;

    public Transform Target { get => target; set => target = value; }

    private void Awake()
{
    // if (target != null)
    // {
    //     _offset = transform.position - target.TransformPoint(Vector3.zero);
    //     Debug.Log("_offset: " + _offset);
    // }
}

private void LateUpdate()
{
    if (target != null)
    {
        //Vector3 targetPosition = target.TransformPoint(Vector3.zero) $+ _offset;
        //transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref _currentVelocity, smoothTime);
        transform.position = Vector3.Lerp(transform.position, target.position - _offset, smoothTime);
    }
}
}
