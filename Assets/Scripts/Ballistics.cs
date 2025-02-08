using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class Ballistics : MonoBehaviour, IFraction 
{

    [Header("Основные")]
    public int health = 5;
    public int damage = 3;
    public Transform SpawnTransform;
    public Transform target;
    public Fraction.Team team = Fraction.Team.Ally;

    [Header("Настройки стрельбы")]
    public float AngleInDegrees;
    float g = Physics.gravity.y;
    public GameObject Bullet;
    public float attackCooldown = 2f;
    private bool isCooldown = false;


    [Header("Настройки AI")]
    public float detectionRadius = 25f;
    public float stopChaseRadius = 50f;
    public float attackRange = 5f;

    [Header("Ссылки")]
    public NavMeshAgent agent;

    [Header("Звуки")]
    public AudioSource audioSource;
    public AudioClip audioClip;

    [Header("Здоровье визуал")]
    public TextMeshProUGUI HP;

    public Fraction.Team GetTeam()
    {
        return team;
    }

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        HP.text = health.ToString();
    }

    void Update () 
    {
        HP.text = health.ToString();
        SpawnTransform.localEulerAngles = new Vector3(-AngleInDegrees, 0f, 0f);
        if (target == null)
        {
            FindTarget();
        }
        else if (agent != null)
        {
            PursueTarget();
        }
    }

    void FindTarget()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRadius);
        Transform closestTarget = null;
        float closestDistance = Mathf.Infinity;

        foreach (var collider in colliders)
        {
            IFraction fraction = collider.GetComponent<IFraction>();

            if (fraction != null && fraction.GetTeam() == Fraction.Team.Enemy)
            {
                float distanceToCollider = Vector3.Distance(transform.position, collider.transform.position);
                if (distanceToCollider < closestDistance)
                {
                    closestDistance = distanceToCollider;
                    closestTarget = collider.transform;
                }
            }
        }

        if (closestTarget != null)
        {
            target = closestTarget;
        }
    }

    void PursueTarget()
    {
        float distanceToTarget = Vector3.Distance(transform.position, target.position);

        if (distanceToTarget > stopChaseRadius)
        {
            Debug.Log("Цель вышла за радиус преследования");
            target = null;
            agent.ResetPath();
            return;
        }

        if (distanceToTarget <= attackRange)
        {
            StartCoroutine(Shot());
            agent.ResetPath();
        }
        else
        {
            agent.SetDestination(target.position);
        }
    }

    public IEnumerator Shot() {
        Vector3 fromTo = target.position - transform.position;
        Vector3 fromToXZ = new Vector3(fromTo.x, 0f, fromTo.z);

        transform.rotation = Quaternion.LookRotation(fromToXZ, Vector3.up);


        float x = fromToXZ.magnitude;
        float y = fromTo.y;

        float AngleInRadians = AngleInDegrees * Mathf.PI / 180;

        float v2 = (g * x * x) / (4 * (y - Mathf.Tan(AngleInRadians) * x) * Mathf.Pow(Mathf.Cos(AngleInRadians), 4));
        float v = Mathf.Sqrt(Mathf.Abs(v2));

        if (isCooldown)
        {
            Debug.Log("Атака на перезарядке!");
            yield break;
        }

        isCooldown = true;
        Debug.Log($"Атакуем {target.name}");

        if (target != null && target.TryGetComponent(out IFraction faction)) 
        {
            GameObject newBullet = Instantiate(Bullet, SpawnTransform.position, Quaternion.identity);
            newBullet.GetComponent<Rigidbody>().linearVelocity = SpawnTransform.forward * v;
        }


        yield return new WaitForSeconds(attackCooldown);

        isCooldown = false;
        Debug.Log("Атака готова!");
    }

    public void TakeDamage(int amount)
    {
        health -= amount;
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, stopChaseRadius);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
