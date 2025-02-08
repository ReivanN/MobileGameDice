using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class AI : MonoBehaviour, IFraction
{
    [Header("Основные")]
    public int health = 10;
    public int damage = 1;
    public Transform target;
    public Fraction.Team team = Fraction.Team.Ally;


    [Header("Настройки Атаки")]
    public float attackCooldown = 2f; 
    private bool isCooldown = false;


    [Header("Настройки AI")]
    public float detectionRadius = 25f;
    public float stopChaseRadius = 50f;
    public float attackRange = 1f;
    public Transform attackRangePosition;

    [Header("Ссылки")]
    public NavMeshAgent agent;
    public GridSystem gridSystem;

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
        gridSystem = FindFirstObjectByType<GridSystem>();
    }
    void Update()
    {
        HP.text = health.ToString();
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

            if(team == Fraction.Team.Ally) 
            {
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
            else if(team == Fraction.Team.Enemy) 
            {
                if (fraction != null && fraction.GetTeam() == Fraction.Team.Ally)
                {
                    float distanceToCollider = Vector3.Distance(transform.position, collider.transform.position);
                    if (distanceToCollider < closestDistance)
                    {
                        closestDistance = distanceToCollider;
                        closestTarget = collider.transform;
                    }
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

        // Если цель в зоне атаки, атакуем
        if (distanceToTarget <= attackRange)
        {
            StartCoroutine(AttackTarget());
            agent.ResetPath();
        }
        else
        {
            agent.SetDestination(target.position);
        }
    }

    public IEnumerator AttackTarget()
    {

        if (isCooldown)
        {
            Debug.Log("Атака на перезарядке!");
            yield break;
        }

        isCooldown = true;
        Debug.Log($"Атакуем {target.name}");

        if (target != null && target.TryGetComponent(out IFraction faction))
        {
            faction.TakeDamage(damage);
            audioSource.Play();
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
