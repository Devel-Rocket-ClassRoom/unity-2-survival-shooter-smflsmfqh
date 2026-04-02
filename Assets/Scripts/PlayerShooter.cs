using UnityEngine;

public class PlayerShooter : MonoBehaviour
{
    public Gun gun;
    public Transform gunPivot;
    
    private PlayerInput playerInput;
    private Animator playerAnimator;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        playerAnimator = GetComponent<Animator>();
    }

    private  void Update()
    {
        if (playerInput.Fire)
        {
            gun.Fire();
        }
    }

    private void OnAnimatorIK(int layerIndex)
    {

        gunPivot.position = playerAnimator.GetIKHintPosition(AvatarIKHint.RightElbow);

        playerAnimator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1f); // 왼손의 IK 위치 가중치를 1로 설정하여 왼손이 총을 잡도록 함
        playerAnimator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1f); // 왼손의 IK 회전 가중치를 1로 설정하여 왼손이 총을 잡도록 함


    }
}
