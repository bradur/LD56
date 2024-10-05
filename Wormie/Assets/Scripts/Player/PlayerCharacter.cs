using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{

    public static PlayerCharacter main;
    void Awake()
    {
        main = this;
    }

    [SerializeField]
    private Transform playerBody;

    [SerializeField]
    private Animator animator;

    private PlayerAnimation animState = PlayerAnimation.Idle;

    void Start()
    {

    }
    void Update()
    {

    }

    public Vector2Int GetMovementInput()
    {
        bool moveRight = Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow);
        bool moveLeft = Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow);
        bool moveUp = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow);
        bool moveDown = Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow);
        int xDirection = moveRight ? 1 : (moveLeft ? -1 : 0);
        int yDirection = moveUp ? 1 : (moveDown ? -1 : 0);
        return new Vector2Int(xDirection, yDirection);
    }

    public bool InputExists()
    {
        Vector2Int moveInput = GetMovementInput();
        return moveInput.x != 0 || moveInput.y != 0;
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
