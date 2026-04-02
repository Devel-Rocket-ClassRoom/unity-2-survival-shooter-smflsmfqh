using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public static readonly int HashMove = Animator.StringToHash("Move");  

    public float moveSpeed = 5f;    

    private Animator playerAnimator;
    private PlayerInput playerInput;
    private Rigidbody playerRigidbody;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        playerAnimator = GetComponent<Animator>();
        playerRigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        Move();
        
        playerAnimator.SetFloat(HashMove, playerInput.HMove);
    }

    private void Move()
    {
        Vector3 moveDir = (Vector3.forward * playerInput.VMove) + (Vector3.right * playerInput.HMove);

        playerRigidbody.transform.Translate(moveDir * Time.deltaTime * moveSpeed, Space.Self);
    }

   


}
