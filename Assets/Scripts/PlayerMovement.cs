using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public static readonly int HashMove = Animator.StringToHash("Move");  

    public float moveSpeed = 10f;    

    Vector3 dir = Vector3.zero;
    private Animator playerAnimator;
    private PlayerInput playerInput;
    private Rigidbody rb;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        playerAnimator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
    }
    private void Update()
    {
        InputDir();
    }
    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + dir * moveSpeed * Time.deltaTime);
        playerAnimator.SetFloat(HashMove, playerInput.HMove);
    }

    private void InputDir()
    {
        dir.x = playerInput.HMove;
        dir.z = playerInput.VMove;
    }

}
