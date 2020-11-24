using UnityEngine;

using RPG.Resources;

public class Projectile : MonoBehaviour
{
    [SerializeField] float speed = 1f;
    [SerializeField] float damage = 8f;
    [SerializeField] bool isHoming = true;
    [SerializeField] GameObject impactEffect = null;
    [SerializeField] float lifeTime = 20.0f;

    private Health target = null;
    private GameObject instigator = null;

    private void Start() {
        if (!isHoming)
        {
            transform.LookAt(GetAimLocation());
        }
    }

    void Update()
    {
        if (target == null) return;
        if (isHoming && !target.IsDead())
        {   
            transform.LookAt(GetAimLocation());
        }
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    private Vector3 GetAimLocation()
    {
        CapsuleCollider targetCollider = target.GetComponent<CapsuleCollider>();
        if (targetCollider == null) return target.transform.position;
        return target.transform.position + Vector3.up * targetCollider.height / 3;
    }

    public void SetTarget(Health target, GameObject instigator, float damage)
    {
        this.target = target;
        this.damage = damage;
        this.instigator = instigator;

        Destroy(gameObject, lifeTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        Health reachedTarget = other.GetComponent<Health>();

        if (!reachedTarget) return;
        if (reachedTarget.IsDead()) return; 

        CapsuleCollider targetCollider = reachedTarget.GetComponent<CapsuleCollider>();
        if (impactEffect != null)
            Instantiate(impactEffect, reachedTarget.transform.position + Vector3.up * (targetCollider.height / 3), transform.rotation);

        reachedTarget.TakeDamage(instigator, damage);
        Destroy(gameObject);
    }
}