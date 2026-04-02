using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public Transform player;

    public Vector3 offset;
    public float smoothSpeed = 0.125f;  

    private void LateUpdate()
    {
        transform.position = player.position + offset;
    }

}
