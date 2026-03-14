using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro; 

public class PlayerMovement : MonoBehaviour
{
    [Header("Настройки движения")]
    public float speed = 7f;
    public float jumpForce = 6f;
    public float rotationSpeed = 50f;
    public float fallThreshold = 1.0f;

    [Header("Логика Сбора (Квест)")]
    public int spheresCollected = 0;
    public int totalSpheresNeeded = 10;
    public TextMeshProUGUI scoreText;

    [Header("Ссылки")]
    public Rigidbody rb;
    public Animator anim;

    private bool isGrounded;
    private float airTimer;

    void Start()
    {
        if (rb == null) rb = GetComponent<Rigidbody>();
        if (anim == null) anim = GetComponentInChildren<Animator>();
        UpdateScoreUI();
    }

    void Update()
    {
        isGrounded = Physics.Raycast(transform.position + Vector3.up * 0.1f, Vector3.down, 0.3f);
        if (anim != null) anim.SetBool("isGrounded", isGrounded);
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            if (anim != null) anim.SetTrigger("JumpTrigger");
        }
        if (!isGrounded)
        {
            airTimer += Time.deltaTime;
            if (airTimer > fallThreshold && rb.linearVelocity.y < -1f)
            {
                if (anim != null) anim.SetBool("isFalling", true);
            }
        }
        else
        {
            airTimer = 0;
            if (anim != null) anim.SetBool("isFalling", false);
        }
        if (transform.position.y < -10f)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    void FixedUpdate()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        Vector3 forward = Camera.main.transform.forward;
        Vector3 right = Camera.main.transform.right;

        forward.y = 0;
        right.y = 0;
        forward.Normalize();
        right.Normalize();

        Vector3 moveDirection = (forward * z + right * x).normalized;

        if (moveDirection.magnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
            rb.linearVelocity = new Vector3(moveDirection.x * speed, rb.linearVelocity.y, moveDirection.z * speed);
        }
        else
        {
            rb.linearVelocity = new Vector3(0, rb.linearVelocity.y, 0);
        }
        float currentSpeed = new Vector2(rb.linearVelocity.x, rb.linearVelocity.z).magnitude;
        if (anim != null) anim.SetFloat("Speed", currentSpeed);
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Finish"))
        {
            spheresCollected++;
            other.gameObject.SetActive(false);
            UpdateScoreUI();

            if (spheresCollected >= totalSpheresNeeded)
            {
                VictoryManager vm = Object.FindFirstObjectByType<VictoryManager>();
                if (vm != null) vm.ShowVictory();
            }
        }
    }
    void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "Сферы: " + spheresCollected + " / " + totalSpheresNeeded;
        }
    }
}