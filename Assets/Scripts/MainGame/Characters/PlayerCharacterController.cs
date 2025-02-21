using System;
using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class PlayerCharacterController : MonoBehaviour
{
    public event UnityAction<int> onTakeDamageEventAction;
    [SerializeField] private UnityEvent<int> onTakeDamageEvent;

    [Header("Navigation")] 
    private NavMeshAgent navMeshAgent;

    [SerializeField] private Transform waypoint;
    [SerializeField] private Transform[] pathWaypoints;
    
    [SerializeField] private Camera playerMainCamera;
    
    private Animator animator;

    public int Hp
    {
        get => hp;
        set => hp = value;
    }

    public int CurrentWaypointIndex
    {
        get => currentWaypointIndex;
        set => currentWaypointIndex = value;
    }

    private bool isMoving = true;
    private int currentWaypointIndex = 0;

    private bool hasBloodyBoots = true;


    private int hp;
    private int startingHp;

    public void ToggleMoving(bool shouldMove)
    {
        isMoving = shouldMove;
        if (navMeshAgent) navMeshAgent.enabled = shouldMove;
    }

    public void SetDestination(Transform targetTransformWaypoint)
    {
        if (navMeshAgent)
            navMeshAgent.SetDestination(targetTransformWaypoint.position);
    }

    public void SetDestination(int waypointIndex)
    {
        SetDestination(pathWaypoints[waypointIndex]);
    }

    public void TakeDamage(int damageAmount)
    {
        hp -= damageAmount;
        float hpPercentLeft = (float) hp / startingHp;
        animator.SetLayerWeight(1, (1 - hpPercentLeft));
        onTakeDamageEvent.Invoke(hp);
        onTakeDamageEventAction?.Invoke(hp);
    }

    private void Start()
    {
        hp = 100;
        animator = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        startingHp = hp;
        SetMudAreaCost();
        ToggleMoving(true);
        SetDestination(pathWaypoints[0]);
    }

    private void SetMudAreaCost()
    {
        if (hasBloodyBoots)
        {
            navMeshAgent.SetAreaCost(3, 1);
        }
    }

    [ContextMenu("Take Damage Test")]
    private void TakeDamageTesting()
    {
        TakeDamage(10);
    }


    private void Update()
    {
        if (isMoving && !navMeshAgent.isStopped && navMeshAgent.remainingDistance <= 0.1f)
        {
            currentWaypointIndex++;
            if (currentWaypointIndex >= pathWaypoints.Length)
                currentWaypointIndex = 0;
            SetDestination(pathWaypoints[currentWaypointIndex]);
        }

        if (animator)
            animator.SetFloat("Speed", navMeshAgent.velocity.magnitude);

        //HoverTool();
    }

    private void HoverTool() // we extracted this out of update. We also commented it out because it was causing errors.
    {
        Ray ray = playerMainCamera.ScreenPointToRay(Input.mousePosition); // This is unnecessary, and we don't see it used, we see it's a hover-tool of sorts.
        if (Physics.Raycast(ray, out RaycastHit hit, 100f))
        {
            //We want to know what the mouse is hovering now
            Debug.Log($"Hit: {hit.collider.name}");
        }
    }

    private void OnValidate()
    {
        if (playerMainCamera == null)
        {
            Debug.LogWarning("PlayerCharacterController::OnValidate::playerMainCamera is null");
        }
    }
}