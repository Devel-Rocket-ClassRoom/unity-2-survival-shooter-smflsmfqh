using UnityEngine;

public class PlayerAim : MonoBehaviour
{
    public LayerMask floorLayer;

    private Vector3 headAim;
    private Ray ray;
    private float rayDistance = 40f;
    public float rotateSpeed = 360f;

    private void Start()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);

    }

    private void Update()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        GetPos();
        //transform.LookAt(headAim);

        Vector3 dir = headAim - transform.position;
        dir.y = 0;

        if (dir != Vector3.zero)
        {
            Quaternion targetRot = Quaternion.LookRotation(dir);

            transform.rotation = Quaternion.RotateTowards(
                transform.rotation,
                targetRot,
                rotateSpeed * Time.deltaTime
            );
        }
    }

    private void GetPos()
    {
        if (Physics.Raycast(ray, out RaycastHit hit, rayDistance, floorLayer))
            if (hit.collider.CompareTag("Background"))
            {
                headAim = hit.point;
            }
    }

}
