using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    public bool isCanMove = true;
    //[SerializeField] CharacterController controller;
    [SerializeField] Rigidbody rb;
    [SerializeField] float initialViewAngle = 90f;

    [SerializeField] Transform groundChecker;
    [SerializeField] float groundCheckRadius;
    [SerializeField] LayerMask groundmask;
    //bool isGrounded;
    [SerializeField] float gravity;
    //Vector3 velocity;

    [SerializeField] GameObject staminaBarParent;
    [SerializeField] Image staminaBarFill;
    public float runSpeed, initialRunSpeed;
    public float speed, initialSpeed;
    [SerializeField] float maxStamina, stamina, staminaMultiplier;
    bool isRunning;
    Animator animator;
    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        //staminaBar.gameObject.SetActive(false);
        staminaBarParent.SetActive(false); 
        speed = initialSpeed;
        stamina = maxStamina;    
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isCanMove)
        {
            MoveCharacter();
        }

        //AnimationHandler();
    }
    public void MoveCharacter()
    {
        float Vertical = Input.GetAxis("Vertical");
        float Horizontal = Input.GetAxis("Horizontal");
        Vector3 movementDir = new Vector3(Horizontal, 0, Vertical) * speed * Time.deltaTime;

        rb.velocity = movementDir;
        if (movementDir.magnitude != 0) animator.SetTrigger("Walk");
        else animator.SetTrigger("Idle");
        //controller.Move(movementDir);

        //isGrounded = Physics.CheckSphere(groundChecker.position, groundCheckRadius, groundmask);
        //if (isGrounded && velocity.y < 0)
        //{
        //    velocity.y = -2f;
        //}

        //velocity.y += gravity * Time.deltaTime;
        //controller.Move(velocity * Time.deltaTime);

        if(Input.GetKey(KeyCode.LeftShift) && movementDir.magnitude != 0)
        {
            Run();
            //staminaBarParent.SetActive(true);
            if(movementDir.magnitude == 0 && stamina == 0)
            {
                stamina += Time.deltaTime * staminaMultiplier;
            }
            Debug.Log("isRunning : " + isRunning);
        }
        else
        {
            isRunning = false;
            speed = initialSpeed;
            stamina += Time.deltaTime * staminaMultiplier;
            //staminaBarParent.SetActive(false);
            //StartCoroutine(DisableStaminaUI());
            staminaBarFill.fillAmount = stamina / maxStamina;
            if (stamina > maxStamina) stamina = maxStamina;
        }
    }

    public void Run()
    {
        stamina -= Time.deltaTime * staminaMultiplier;
        speed = runSpeed;
        staminaBarFill.fillAmount = stamina / maxStamina;
        isRunning = true;

        animator.ResetTrigger("Walk");
        animator.SetTrigger("Run");

        if (stamina < 0)
        {
            stamina = 0;
            speed = initialSpeed;
            stamina += Time.deltaTime * staminaMultiplier;
        }
    }

    public void AnimationHandler()
    {
        animator.SetFloat("charSpeed", rb.velocity.sqrMagnitude);
        animator.SetBool("isRunning", isRunning);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Hider"))
        {
            Debug.Log("Game Over");
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(groundChecker.position, groundCheckRadius);
    }

    private IEnumerator DisableStaminaUI()
    {
        if(staminaBarParent.activeInHierarchy == true)
        {
            yield return new WaitForSeconds(3);
            staminaBarParent.SetActive(false);
            StopCoroutine(DisableStaminaUI());
        }
    }
}