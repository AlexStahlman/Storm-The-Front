using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private int currentHealth, maxHealth;
    private float rotationSpeed;
    public Transform orientation;
    [SerializeField] private Animator animator;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Animator enemyAnimator;
    [SerializeField] private float moveSpeed;
    private Vector3 dashPos;
    Rigidbody playerRb;
    private Vector3 playerInput;
    public static bool isGameOver;
    [SerializeField] public GameObject GameOverScreen;



    // Start is called before the first frame update
    void Start()
    {
        rotationSpeed = 10;
        maxHealth = 100;
        currentHealth = 50;
        ChangeHealth(100);
        moveSpeed = 5f;
        animator.fireEvents = false;
        playerRb= GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {

        playerInput = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
        //movement
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Punch")|| animator.GetCurrentAnimatorStateInfo(0).IsName("Taunt") || animator.GetCurrentAnimatorStateInfo(0).IsName("Heal"))
        {
            GuiManager.instance.GrayOutAttack(6/10);
            return;
        }
        else
        {
            GuiManager.instance.ChangeText("");
            GuiManager.instance.GrayOutAttack(1);
            MovePlayer();
        }

        //random roll button
        //issues: Moving while animation is playing, feels like there is a delay
        if (Input.GetKeyDown(KeyCode.J))
        {
            int random = Random.Range(0, 3);
            if(random == 1)
            {
                GuiManager.instance.ChangeText("Healing");
                animator.SetFloat("Speed", 0);
                animator.SetTrigger("Heal");
                ChangeHealth(10);
                return;
            }

            GameObject closestEnemy = FindClosestEnemy();
            if (Vector3.Distance(closestEnemy.transform.position, transform.position) <= 3)
            {
                //deal damage to the enemy or one shot
                if (random == 2)
                {
                    GuiManager.instance.ChangeText("Attacking");
                    animator.SetFloat("Speed", 0);
                    animator.SetTrigger("Attack");
                    enemyAnimator.SetTrigger("hit");
                    return;
                }
            }
            else
            {
                GuiManager.instance.ChangeText("Taunting");
                animator.SetFloat("Speed", 0);
                animator.SetTrigger("Taunt");
            }
        }

        if (isGameOver)
        {
            GameOverScreen.SetActive(true);
        }
        
    }
    

    /// adds the supplied value to players health, pass in a anegative value to deal damage
    public void ChangeHealth(int value){
        currentHealth += value;

        if(currentHealth< 0)
        {
            isGameOver = true;
            print("Player is dead");
        }
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }

        GuiManager.instance.UpdateHealthBar((float)currentHealth / maxHealth);
    }
    public GameObject FindClosestEnemy()
    {
        GameObject[] enemy;
        enemy = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject close = null;
        float distance = Mathf.Infinity;
        Vector3 position = transform.position;
        foreach (GameObject go in enemy)
        {
            Vector3 diff = go.transform.position - position;
            float curDistance = diff.sqrMagnitude;
            if (curDistance < distance)
            {
                close = go;
                distance = curDistance;
                enemyAnimator = go.GetComponent<Animator>();
            }
        }
        return close;
    }

    private void MovePlayer()
    {
        
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 inputLocation = new Vector3(h, 0, v);
        Vector3 inputDirection = new Vector3(h, 0, v) + playerTransform.position;

        if (h != 0 || v != 0)
        {
            
            //get vector from player to the input direction position
            Vector3 toFaceDirection = inputDirection - playerTransform.position;

            //Rotate player to face input direction
            Quaternion targetRotation = Quaternion.LookRotation(toFaceDirection);
            //playerTransform.rotation = Quaternion.Lerp(playerTransform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            //move player
            //transform.Translate(inputLocation * moveSpeed * Time.deltaTime, Space.World);
            
            Vector3 moveVector = transform.TransformDirection(playerInput) * moveSpeed;
            playerRb.velocity = new Vector3(moveVector.x, moveVector.y, moveVector.z);
            playerRb.MoveRotation(Quaternion.LookRotation(playerRb.velocity));

            if (Input.GetKeyDown(KeyCode.K))
            {
                dashPos = transform.position;
                if(v > 0)
                {
                    dashPos.z += 2;
                    transform.position = dashPos;
                    return;
                }
                if (h > 0)
                {
                    dashPos.x += 2;
                    transform.position = dashPos;
                    return;
                }
                if (h < 0)
                {
                    dashPos.x -= 2;
                    transform.position = dashPos;
                    return;
                }
                if (v < 0)
                {
                    dashPos.z -= 2;
                    transform.position = dashPos;
                    return;
                }


            }
        }


        float speedValue = Mathf.Max(Mathf.Abs(h), Mathf.Abs(v));

        animator.SetFloat("Speed", speedValue);

    }

    public void ReturnToState()
    {
        animator.SetTrigger("ReturnState");
    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "level")
        {
            Debug.Log("Hit");
            animator.SetFloat("Speed", 0);
        }
        if(collision.gameObject.name == "Cubed")
        {
            Debug.Log("Level Complete");
            isGameOver = true;
            transform.position = new Vector3(0,0,-125);
        }
        if (collision.gameObject.name == "Cube"|| collision.gameObject.name == "Cube1"|| collision.gameObject.name == "Cube2")
        {
            Debug.Log("Returning to Playable area");
            transform.position = new Vector3(0, 0, -125);
        }
        
    }


}
