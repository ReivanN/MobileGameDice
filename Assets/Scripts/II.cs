using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class II : MonoBehaviour, IFraction
{
    [Header("��������")]
    public int health = 10;
    public int damage = 1;
    public Transform target;
    public Fraction.Team team = Fraction.Team.Ally;


    [Header("��������� �����")]
    public float attackCooldown = 2f; // ����� ����������� �����
    private bool isCooldown = false; // ����, ���� �� �����������


    [Header("��������� AI")]
    public float detectionRadius = 25f; // ������ ����������� ����
    public float stopChaseRadius = 50f; // ������, ����� �������� ��� ���������� �������������
    public float attackRange = 1f;     // ��������� ��� �����

    [Header("������")]
    public NavMeshAgent agent;

    [Header("�����")]
    public AudioSource audioSource;
    public AudioClip audioClip;

    [Header("�������� ������")]
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
        FindTarget();
    }

    void FindTarget()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRadius);
        Transform closestTarget = null;
        float closestDistance = Mathf.Infinity;

        foreach (var collider in colliders)
        {
            IFraction fraction = collider.GetComponent<IFraction>();

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
            // ���������� �������������, ���� ���� ������� ������
            Debug.Log("���� ����� �� ������ �������������");
            target = null;
            agent.ResetPath();
            return;
        }

        // ���� ���� � ���� �����, �������
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
            Debug.Log("����� �� �����������!");
            yield break; // �������, ���� CoolDown ��� �� ��������
        }

        isCooldown = true; // ������������� ���� CoolDown
        Debug.Log($"������� {target.name}");

        // ������� ���� ����, ���� ��� ��������� IFaction
        if (target != null && target.TryGetComponent(out IFraction faction))
        {
            faction.TakeDamage(damage);
            audioSource.Play();
        }

        // ���� ���������� CoolDown
        yield return new WaitForSeconds(attackCooldown);

        isCooldown = false; // ������� ���� CoolDown
        Debug.Log("����� ������!");
    }

    public void TakeDamage(int amount)
    {
        health -= amount;
        if (health <= 0)
        {
            Destroy(gameObject); // ���������� ������, ���� �������� <= 0
        }
    }



    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, detectionRadius); // ������ �����������

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, stopChaseRadius); // ������ ����������� �������������

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, attackRange); // ������ �����
    }
}
