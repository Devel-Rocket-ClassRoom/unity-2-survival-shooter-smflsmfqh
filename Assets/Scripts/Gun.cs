using System.Collections;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public enum Status
    {
        Ready,
        Shooting,
    }

    public Status State { get; private set; }
    public Transform fireTransform;
    public LayerMask targetLayer;
    public ParticleSystem effect;
    private LineRenderer lineRenderer;

    private AudioSource gunAudioPlayer;
    public AudioClip fireClip;

    private float fireDistance = 40f;   
    private float lastFireTime;
    private float fireRate = 0.12f;
    private float damage = 20f;

    private Coroutine coShot;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        gunAudioPlayer = GetComponent<AudioSource>();

        lineRenderer.positionCount = 2;
        lineRenderer.enabled = false;
    }
    
    private void OnEnable()
    {
        State = Status.Ready;
        lastFireTime = 0f;
    }

    public void Fire()
    {
        if (State == Status.Ready && Time.time >= lastFireTime + fireRate)
        {
            lastFireTime = Time.time;
            Shot();
        }
    }

    private void Shot()
    {
        Ray ray = new Ray(fireTransform.position, fireTransform.forward);
        Vector3 hitPosition = Vector3.zero;

        if (Physics.Raycast(ray, out RaycastHit hit, fireDistance, targetLayer)) 
        {
            var target = hit.collider.GetComponent<LivingEntity>();
            if (target != null) 
            {
                if (!target.IsDead)
                {
                    target.OnDamage(damage, hit.point, hit.normal);
                }
            }
            hitPosition = hit.point; 
        }
        else
        {
            hitPosition = fireTransform.position + fireTransform.forward * fireDistance;
        }

        coShot = StartCoroutine(CoShotEffect(hitPosition));
    }

    private IEnumerator CoShotEffect(Vector3 hitPosition)
    {
        gunAudioPlayer.PlayOneShot(fireClip);
        effect.transform.position = fireTransform.position;
        effect.transform.rotation = fireTransform.rotation;
        effect.Play();

        lineRenderer.SetPosition(0, fireTransform.position);
        lineRenderer.SetPosition(1, hitPosition);
        lineRenderer.enabled = true;

        yield return new WaitForSeconds(0.03f);

        lineRenderer.enabled = false;

        coShot = null;
    }
}
