using UnityEngine;
using Unity.Cinemachine;

public class CameraFollowPlayer : MonoBehaviour
{
    [SerializeField] private CinemachineCamera _cinemachineCamera;

    public void SetFollow(Transform target)
    {
        _cinemachineCamera.Target.TrackingTarget = target;
    }
}