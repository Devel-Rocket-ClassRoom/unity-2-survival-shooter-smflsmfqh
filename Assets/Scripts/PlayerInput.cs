using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public static readonly string HmoveAxisName = "Horizontal";   
    public static readonly string VmoveAxisName = "Vertical";   
    public static readonly string fireButtonName = "Fire1"; 

    public float HMove { get; private set; }
    public float VMove { get; private set; }
    public bool Fire { get; private set; } 

    void Awake()
    {
        HMove = 0f;
        VMove = 0f;
        Fire = false;
    }

    void Update()
    {
        HMove = Input.GetAxis(HmoveAxisName);
        VMove = Input.GetAxis(VmoveAxisName);
        Fire = Input.GetButton(fireButtonName);
    }

}
