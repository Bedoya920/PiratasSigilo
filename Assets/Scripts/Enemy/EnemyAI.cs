using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [Header("Ruta y Visión")]
    public Transform[] pathPoints;
    public Transform player;
    public float visionRange = 10f;
    public float visionAngle = 45f;

    [Header("Tiempos")]
    public float agroTime = 2f;
    public float restPatrol = 2f;  
    public float investigateTime = 10f;

    [Header("Rangos")]
    public float closeRangeDetection = 3f; // Rango cercano para activar Agro de una
    public float hearingRange = 5f; // Rango de escucha del enemigo
    public float shootingRange = 10f; // Rango de disparo

    public LayerMask obstacleMask;

    //NavMesh data
    private NavMeshAgent agent;
    private int currentPathIndex;
    
    private float agroCounter;
    private float outAgroCounter;

    // Estados del enemigo
    private bool isWaiting;
    private bool isInvestigating;
    [HideInInspector] public bool isAgro;
    private Vector3 lastKnownPosition;
    private float searchCounter;

    // Componente de disparo
    private EnemyShooting enemyShooting;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        enemyShooting = GetComponent<EnemyShooting>();
        currentPathIndex = 0;
        agent.destination = pathPoints[currentPathIndex].position;
        isAgro = false;
        agroCounter = 0f;
        outAgroCounter = 0f;
        isWaiting = false;
        isInvestigating = false;
        searchCounter = 0f;
    }

    void Update()
    {
        if (isAgro)
        {
            ChasePlayer();
        }
        else if (isInvestigating)
        {
            // El enemigo está investigando un punto de sonido
            if (agent.remainingDistance < 0.5f && !isWaiting)
            {
                StartCoroutine(InvestigateSound());
            }
        }
        else
        {
            if (!isWaiting)
            {
                Patrol();
                CheckForPlayer();
            }
        }
    }

    void PlayerFound()
    {
        isAgro = true;
        agroCounter = 0f;
    }

    void Patrol()
    {
        if (agent.remainingDistance < 0.5f && !isWaiting)
        {
            StartCoroutine(WaitAtPatrolPoint());
        }
    }

    IEnumerator WaitAtPatrolPoint()
    {
        isWaiting = true;
        this.gameObject.GetComponent<Renderer>().material.color = Color.blue;
        yield return new WaitForSeconds(restPatrol);
        currentPathIndex = (currentPathIndex + 1) % pathPoints.Length;
        agent.destination = pathPoints[currentPathIndex].position;
        
        isWaiting = false;
    }

    IEnumerator InvestigateSound()
    {
        print("Sellamo investigate Sound0");
        isWaiting = true;
        float timer = investigateTime;

        while (timer > 0 && agent.remainingDistance > 0.5f)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        print("Acabo el ciclo de investigate Sound0");
        if (IsPlayerInSight())
        {
            PlayerFound();
        }

        isWaiting = false;
        isInvestigating = false;
        agent.destination = pathPoints[currentPathIndex].position;
    }

    void ChasePlayer()
    {
        agent.destination = player.position;
        lastKnownPosition = player.position;

        // Pierde al jugador de vista
        if (!IsPlayerInSight())
        {
            // Si no lo encuentra en este tiempo, busca en la última posición conocida
            outAgroCounter += Time.deltaTime;
            if (outAgroCounter >= agroTime)
            {
                isAgro = false;
                outAgroCounter = 0f;
                searchCounter = investigateTime;
                StartCoroutine(SearchForPlayer());
            }
        }
        else
        {
            outAgroCounter = 0f;

            // Si el jugador está dentro del rango de disparo
            if (Vector3.Distance(transform.position, player.position) <= shootingRange)
            {
                enemyShooting.Shoot(player.transform);
            }
        }
    }

    IEnumerator SearchForPlayer()
    {
        agent.destination = lastKnownPosition;
        while (searchCounter > 0)
        {
            searchCounter -= Time.deltaTime;
            yield return null;

            if (IsPlayerInSight())
            {
                PlayerFound();
                yield break;
            }
        }
        agent.destination = pathPoints[currentPathIndex].position;
    }

    void CheckForPlayer()
    {
        // Se tantea si el player está en la mira y si NO está en un safe spot
        if (IsPlayerInSight() && !player.GetComponent<ThirdPersonController>().isInSafeSpot)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);
            
            if (distanceToPlayer <= closeRangeDetection)
            {
                print("Agro activo, te estoy persiguiendo");
                PlayerFound();
            }
            else
            {
                agroCounter += Time.deltaTime;
                if (agroCounter >= agroTime)
                {
                    PlayerFound();
                }
            }
        }
        else
        {
            agroCounter = 0f;
        }
    }

    public void OnHearSound(Vector3 soundPosition)
    {
        float distanceToSound = Vector3.Distance(transform.position, soundPosition);
        if (distanceToSound <= hearingRange)
        {
            isInvestigating = true;
            agent.destination = soundPosition;

            float distanceToPlayer = Vector3.Distance(transform.position, player.position);
            
            if (distanceToPlayer <= closeRangeDetection)
            {
                print("Voy por ti sucia, gracias al sonido que hiciste");
                PlayerFound();
            }
        }
    } 

    public bool IsPlayerInSight()
    {
        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer < visionRange)
        {
            float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer);
            if (angleToPlayer < visionAngle / 2)
            {
                if (!Physics.Raycast(transform.position, directionToPlayer, distanceToPlayer, obstacleMask))
                {
                    return true;
                }
            }
        }
        return false;
    }

    // Dibuja los círculos en la escena
    void OnDrawGizmos()
    {
        // Amarillo = Rango de la visión
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, visionRange);

        Vector3 leftBoundary = Quaternion.Euler(0, -visionAngle / 2, 0) * transform.forward * visionRange;
        Vector3 rightBoundary = Quaternion.Euler(0, visionAngle / 2, 0) * transform.forward * visionRange;

        //Rango vision
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + leftBoundary);
        Gizmos.DrawLine(transform.position, transform.position + rightBoundary);

        // Dibuja línea si ve al jugador
        if (player != null && IsPlayerInSight())
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, player.position);
        }

        // Rojo = Rango de detección inmediata
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, closeRangeDetection);

        // Cyan (Azulito claro) = Rango de detección de sonido
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, hearingRange);
    }
}
