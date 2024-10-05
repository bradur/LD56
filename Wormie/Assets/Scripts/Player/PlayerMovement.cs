using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float minInput = 0.25f;
    [SerializeField]
    private float moveDuration = 0.15f;
    private float moveTimer = 0f;
    private bool isMoving = false;
    private bool isOnDiggingCooldown = false;

    private Vector2Int gridPosition = Vector2Int.zero;
    private Vector2Int targetGridPosition = Vector2Int.zero;

    [SerializeField]
    private Transform movingTransform;
    [SerializeField]
    private PlayerDigger digger;

    private Vector2 startPosition;
    private Vector2 targetPosition;


    private float waitBeforeIdle = 0.5f;
    private float waitBeforeIdleTimer = 0f;
    [SerializeField]
    private float diggingCooldown = 0.5f;
    private float diggingCooldownTimer = 0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (digger.IsDigging)
        {
            return;
        }
        if (isMoving)
        {
            Move();
            return;
        }
        if (isOnDiggingCooldown)
        {
            diggingCooldownTimer += Time.deltaTime;
            if (diggingCooldownTimer >= diggingCooldown)
            {
                diggingCooldownTimer = 0f;
                isOnDiggingCooldown = false;
            }
            return;
        }
        waitBeforeIdleTimer += Time.deltaTime;
        Vector2Int newDirection = PlayerCharacter.main.GetMovementInput();
        if (newDirection.magnitude > 0)
        {
            MoveResult result = PlayerCharacter.AttemptMove(gridPosition, newDirection);
            PlayerCharacter.main.FaceDirection(newDirection.x);
            if (result.Success)
            {
                if (result.Tile != null && result.Tile.Diggable(PlayerLevel.main.DigPower))
                {
                    digger.StartDigging(result);
                    isOnDiggingCooldown = true;
                }
                else
                {
                    StartMoving(result.Position);
                }
            }
            else if (result.Tile == null || !result.Tile.Diggable(PlayerLevel.main.DigPower))
            {
                PlayerCharacter.main.Animate(PlayerAnimation.Move);
            }
            else if (result.Tile != null && result.Tile.Diggable(PlayerLevel.main.DigPower))
            {
                digger.StartDigging(result);
                isOnDiggingCooldown = true;
            }
            waitBeforeIdleTimer = 0f;
        }
        else if (waitBeforeIdleTimer >= waitBeforeIdle)
        {
            PlayerCharacter.main.Animate(PlayerAnimation.Idle);
        }
        else
        {
            PlayerCharacter.main.AnimateReset();
        }
    }

    private void StartMoving(Vector2Int target)
    {
        isMoving = true;
        PlayerCharacter.main.Animate(PlayerAnimation.Move);
        targetGridPosition = target;
        startPosition = movingTransform.position;
        targetPosition = targetGridPosition;
        moveTimer = 0f;
    }

    private void Move()
    {
        moveTimer += Time.deltaTime;
        movingTransform.position = Vector2.Lerp(startPosition, targetPosition, moveTimer / moveDuration);
        if (moveTimer >= moveDuration)
        {
            movingTransform.position = targetPosition;
            isMoving = false;
            moveTimer = 0f;
            gridPosition = targetGridPosition;
            PlayerCharacter.main.AnimateReset();
        }
    }
}

public struct MoveAttempt
{
    public Vector2Int Origin;
    public Vector2Int Direction;
}

public struct MoveResult
{
    public MoveResult(MoveAttempt attempt, Vector2Int position, WorldTile tile, bool success)
    {
        Attempt = attempt;
        Position = position;
        Tile = tile;
        Success = success;
    }
    public MoveAttempt Attempt;
    public Vector2Int Position;
    public WorldTile Tile;
    public bool Success;
}