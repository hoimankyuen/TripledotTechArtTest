using UnityEngine;

public class SimpleRotationAnimation : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private Vector3 axis;
    [SerializeField] private float angularVelocity;

    private void Update()
    {
        transform.Rotate(axis, angularVelocity * Time.deltaTime);
    }
}
