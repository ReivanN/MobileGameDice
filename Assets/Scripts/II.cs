using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class II : MonoBehaviour, IFraction
{
    [Header("Основные")]
    public int health = 10;
    public int damage = 1;
    public Transform target;
    public Fraction.Team team = Fraction.Team.Ally;


    [Header("Настройки Атаки")]
    public float attackCooldown = 2f; // Время перезарядки атаки
    private bool isCooldown = false; // Флаг, идет ли перезарядка


    [Header("Настройки AI")]
    public float detectionRadius = 25f; // Радиус обнаружения цели
    public float stopChaseRadius = 50f; // Радиус, после которого бот прекращает преследование
    public float attackRange = 1f;     // Дистанция для атаки

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
            // Прекращаем преследование, если цель слишком далеко
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
            yield break; // Выходим, если CoolDown ещё не завершен
        }

        isCooldown = true; // Устанавливаем флаг CoolDown
        Debug.Log($"Атакуем {target.name}");

        // Наносим урон цели, если она реализует IFaction
        if (target != null && target.TryGetComponent(out IFraction faction))
        {
            faction.TakeDamage(damage);
            audioSource.Play();
        }

        // Ждем завершения CoolDown
        yield return new WaitForSeconds(attackCooldown);

        isCooldown = false; // Снимаем флаг CoolDown
        Debug.Log("Атака готова!");
    }

    public void TakeDamage(int amount)
    {
        health -= amount;
        if (health <= 0)
        {
            Destroy(gameObject); // Уничтожить объект, если здоровье <= 0
        }
    }



    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, detectionRadius); // Радиус обнаружения

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, stopChaseRadius); // Радиус прекращения преследования

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, attackRange); // Радиус атаки
    }
}
