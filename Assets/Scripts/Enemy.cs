using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Enemy : MonoBehaviour
{
    Transform target;
    [SerializeField] private GameObject Player;

    private float rotationSpeed = 5f;
    private float radiusOfSatisfaction = 1f;

    [SerializeField] private Transform trans;
    [SerializeField] private Animator animator;
    [SerializeField] private Animator PlayerAnimator;

    [SerializeField] float moveSpeed = 3f;

    private float attackHitRange;

    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        target = Player.transform;
        attackHitRange = 0.9f;
    }

    private void Update()
    {
        MoveToTarget();
        FaceTarget();

    }

    private void MoveToTarget()
    {
        if (target == null)
        {
            animator.SetFloat("Speed", 0);
        }
        //attack if close
        else if (Vector3.Distance(trans.position, target.position) < radiusOfSatisfaction)
        {

            animator.SetFloat("Speed", 0);
            animator.SetTrigger("Attack");
            //next step is return to running to player

        }
        //move towards
        else
        {
            animator.SetFloat("Speed", 1);
        }



    }

    private void FaceTarget()
    {
        if (target == null)
        {
            trans.rotation = Quaternion.Lerp(trans.rotation, Quaternion.identity, rotationSpeed * Time.deltaTime);
        }
        else
        {
            Vector3 toTarget = target.position - trans.position;
            Quaternion targetRotate = Quaternion.LookRotation(toTarget);
            trans.rotation = Quaternion.Lerp(trans.rotation, targetRotate, rotationSpeed * Time.deltaTime);
        }
    }

    public void CheckForHit()
    {
        if(Vector3.Distance(trans.position, target.position) < attackHitRange)
        {
            Player.GetComponent<PlayerMovement>().ChangeHealth(-10);
            PlayerAnimator.SetTrigger("TakeDamage");
        }
    }



    public void AttackDone()
    {
        animator.SetTrigger("attackDone");
        animator.SetFloat("Speed", 0);
    }
    public void DeleteInstance()
    {
        Destroy(this);
    }
}
