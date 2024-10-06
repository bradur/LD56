using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{

    public static PlayerCharacter main;
    void Awake()
    {
#if !UNITY_EDITOR
    Cursor.visible = false;
#endif
        main = this;
    }

    [SerializeField]
    private Transform playerBody;

    [SerializeField]
    private Animator animator;

    private bool moveRight = false;
    private bool moveLeft = false;
    private bool moveUp = false;
    private bool moveDown = false;
    private float inputWindow = 0.1f;

    private int xDirection = 0;
    private int yDirection = 0;

    private PlayerAnimation animState = PlayerAnimation.Idle;

    [SerializeField]
    private PlayerMovement playerMovement;

    public Vector2Int GridPosition { get { return playerMovement.GridPosition; } }

    void Start()
    {

    }
    void FixedUpdate()
    {
        float xAxis = Input.GetAxis("Horizontal");
        float yAxis = Input.GetAxis("Vertical");
        xDirection = xAxis > inputWindow ? 1 : (xAxis < -inputWindow ? -1 : 0);
        yDirection = yAxis > inputWindow ? 1 : (yAxis < -inputWindow ? -1 : 0);
        UIDebugText.main?.ShowMessage($"x[{xAxis}] = {xDirection}\ny[{yAxis}] = {yDirection}");
    }

    public Vector2Int GetMovementInput()
    {
        return new Vector2Int(xDirection, yDirection);
    }

    public static MoveResult AttemptMove(Vector2Int start, Vector2Int direction)
    {
        MoveAttempt attempt = new() { Origin = start, Direction = direction };
        return WorldGrid.main.MoveAttempt(attempt);
    }

    public bool InputExists()
    {
        bool right = Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow);
        bool left = Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow);
        bool up = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow);
        bool down = Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow);
        return right || left || up || down;
    }
    public bool InputMatches(MoveResult result)
    {
        Vector2Int moveInput = GetMovementInput();
        MoveAttempt attempt = result.Attempt;
        return moveInput == attempt.Direction;
    }

    public void Animate(PlayerAnimation animation)
    {
        animator.Play($"player{animation}");
        animState = animation;
    }

    public void AnimateReset()
    {
        if (!InputExists())
        {
            if (animState != PlayerAnimation.Idle)
            {
                Animate(PlayerAnimation.Empty);
            }
        }
    }

    public void FaceDirection(int direction)
    {
        if (direction == 0)
        {
            return;
        }
        Vector2 scale = playerBody.transform.localScale;
        scale.x = direction;
        playerBody.transform.localScale = scale;
    }
}

public enum PlayerAnimation
{
    Idle,
    Move,
    Dig,
    Empty
}
