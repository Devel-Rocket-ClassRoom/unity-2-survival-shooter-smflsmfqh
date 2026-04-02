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
        transform.LookAt(headAim);

       
    }

    private void GetPos()
    {
        if (Physics.Raycast(ray, out RaycastHit hit, rayDistance, floorLayer))
            if (hit.collider.CompareTag("Background"))
            {
                headAim = new Vector3(hit.point.x, transform.position.y, hit.point.z);
                
            }
    }

}
