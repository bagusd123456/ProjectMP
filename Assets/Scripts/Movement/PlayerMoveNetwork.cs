using System;
using System.Collections;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;
using System.Linq;
using System.Collections.Generic;

public class PlayerMoveNetwork : NetworkBehaviour
{
    public delegate void OnAnyPlayerSpawned();
    public static OnAnyPlayerSpawned onAnyPlayerSpawned;
    public Vector3 spawnPos;

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
    
    [HideInInspector]
    public NetworkVariable<bool> isDead = new NetworkVariable<bool>(false,NetworkVariableReadPermission.Everyone,NetworkVariableWritePermission.Owner);
    Animator animator;

    [Header("Hider Setup")]
    public SkinnedMeshRenderer meshRenderer;
    public SkinnedMeshRenderer additionalMaterial;
    public List<GameObject> circleView = new List<GameObject>();
    public void AssignCamera()
    {
        if (IsOwner && IsClient)
        {
            Debug.Log(CameraFollow.Instance);

            CameraFollow.Instance.player = this.transform;
            GameSetupController.Singleton.player = gameObject;
        }
    }

    public override void OnNetworkSpawn()
    {

        AssignCamera();
        GameSetupController.onFinishedSpawn += GameSetupController.Singleton.SetMaterial;
        AddPlayerConnectedClientRpc();
    }

    [ClientRpc]
    public void AddPlayerConnectedClientRpc()
    {
        GameSetupController.Singleton.playerSpawned++;
        Timer.Instance.StartTimer();

        Debug.Log(GameSetupController.Singleton.playerSpawned);
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        staminaBarParent.SetActive(false);
        speed = initialSpeed;
        stamina = maxStamina;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!IsOwner) return;
        if (isCanMove)
        {
            MoveCharacter();
        }
        if (isDead.Value)
            gameObject.SetActive(false);   
    }

    public void MoveCharacter()
    {
        float Vertical = Input.GetAxis("Vertical");
        float Horizontal = Input.GetAxis("Horizontal");
        Vector3 movementDir = new Vector3(Horizontal, 0, Vertical) * speed * Time.deltaTime;

        rb.velocity = movementDir;
        //controller.Move(movementDir);

        if (movementDir.magnitude != 0) animator.SetTrigger("Walk");
        else animator.SetTrigger("Idle");

        //velocity.y += gravity * Time.deltaTime;
        //controller.Move(velocity * Time.deltaTime);

        if (Input.GetKey(KeyCode.LeftShift) && movementDir.magnitude != 0)
        {
            Run();
            if (movementDir.magnitude == 0 && stamina == 0)
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

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Hider"))
        {
            collision.gameObject.GetComponentInParent<PlayerMoveNetwork>().Invoke("TriggerDeadServerRpc",0f);
            Debug.Log("Game Over");
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void TriggerDeadServerRpc()
    {
        isDead.Value = true;
    }

    public void DisableView()
    {
        foreach (var item in circleView)
        {
            item.SetActive(false);
        }
    }

    private IEnumerator DisableStaminaUI()
    {
        if (staminaBarParent.activeInHierarchy == true)
        {
            yield return new WaitForSeconds(3);
            staminaBarParent.SetActive(false);
            StopCoroutine(DisableStaminaUI());
        }
    }
}