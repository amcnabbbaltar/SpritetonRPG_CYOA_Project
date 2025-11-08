using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerContinuousGridMovement : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private float moveSpeed = 5f;        // Units per second
    [SerializeField] private float gridSize = 1f;         // Size of one tile
    [SerializeField] private LayerMask groundMask;        // For valid click areas

    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer visual;

    private bool isWalking = false;
    private bool movementDisabled = false;

    private Vector2 inputDir = Vector2.zero;
    private Vector2 moveTarget;
    private bool hasMouseTarget = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
        visual = GetComponentInChildren<SpriteRenderer>();
    }

    private void Start()
    {
        // Snap player to grid on start
        Vector2Int gridPos = WorldToGrid(rb.position);
        rb.position = GridToWorld(gridPos);
        moveTarget = rb.position;

        GameEventsManager.instance.playerEvents.onDisablePlayerMovement += DisablePlayerMovement;
        GameEventsManager.instance.playerEvents.onEnablePlayerMovement += EnablePlayerMovement;
    }

    private void OnDestroy()
    {
        GameEventsManager.instance.playerEvents.onDisablePlayerMovement -= DisablePlayerMovement;
        GameEventsManager.instance.playerEvents.onEnablePlayerMovement -= EnablePlayerMovement;
    }

    private void DisablePlayerMovement() => movementDisabled = true;
    private void EnablePlayerMovement() => movementDisabled = false;

    private void Update()
    {
        if (movementDisabled) return;

        HandleKeyboardInput();
        HandleMouseClick();
        UpdateAnimator();
    }

    private void FixedUpdate()
    {
        if (movementDisabled) return;

        Vector2 currentPos = rb.position;
        Vector2 nextPos = currentPos;

        // Mouse path movement
        if (hasMouseTarget)
        {
            Vector2 direction = (moveTarget - currentPos).normalized;
            float distance = Vector2.Distance(currentPos, moveTarget);

            if (distance > 0.05f)
            {
                nextPos = Vector2.MoveTowards(currentPos, moveTarget, moveSpeed * Time.fixedDeltaTime);
                isWalking = true;
                FlipSprite(direction.x);
            }
            else
            {
                nextPos = moveTarget;
                hasMouseTarget = false;
                isWalking = false;
            }
        }
        // Keyboard movement
        else if (inputDir != Vector2.zero)
        {
            nextPos = currentPos + inputDir * moveSpeed * Time.fixedDeltaTime;
            isWalking = true;
            FlipSprite(inputDir.x);
        }
        else
        {
            isWalking = false;
        }

        rb.MovePosition(nextPos);
    }

    private void HandleMouseClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 clickPos = new Vector2(mouseWorld.x, mouseWorld.y);

            // Optional: only accept clicks on ground
            RaycastHit2D hit = Physics2D.Raycast(clickPos, Vector2.zero, 0.1f, groundMask);
            moveTarget = hit.collider != null ? hit.point : clickPos;

            // Snap mouse target to grid if desired
            Vector2Int gridTarget = WorldToGrid(moveTarget);
            moveTarget = GridToWorld(gridTarget);

            hasMouseTarget = true;
        }
    }

    private void HandleKeyboardInput()
    {
        // If mouse movement is active, keyboard interrupts it
        if (hasMouseTarget && (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0))
        {
            hasMouseTarget = false;
        }

        inputDir = Vector2.zero;

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) inputDir.y = 1;
        else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) inputDir.y = -1;

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) inputDir.x = -1;
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) inputDir.x = 1;

        inputDir = inputDir.normalized;
    }

    private void FlipSprite(float xDir)
    {
        if (xDir < -0.05f) visual.flipX = true;
        else if (xDir > 0.05f) visual.flipX = false;
    }

    private void UpdateAnimator()
    {
        animator.SetBool("isWalking", isWalking);
        //!animator.SetFloat("velocity_x", inputDir.x);
       //!animator.SetFloat("velocity_y", inputDir.y);
    }

    // 🔹 Grid Helpers
    private Vector2Int WorldToGrid(Vector2 worldPos)
    {
        return new Vector2Int(
            Mathf.RoundToInt(worldPos.x / gridSize),
            Mathf.RoundToInt(worldPos.y / gridSize)
        );
    }

    private Vector2 GridToWorld(Vector2Int gridPos)
    {
        return new Vector2(gridPos.x * gridSize, gridPos.y * gridSize);
    }
}
