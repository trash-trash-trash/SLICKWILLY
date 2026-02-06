using UnityEngine;

public class TempCameraFollow : MonoBehaviour
{
    public Transform target;

    void LateUpdate()
    {
        if (!target) return;

        // Vector3 pos = transform.position;
        // pos.x = target.position.x;
        // pos.z = target.position.z;
        transform.position = target.position + new  Vector3(0, 30, -30);
        transform.LookAt(target);
    }
}