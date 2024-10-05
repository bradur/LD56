using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float minInput = 0.25f;
    [SerializeField]
    private float moveDuration = 0.15f;
    private float moveTimer = 0f;
    private bool isMoving = false;

    private Vector2Int gridPosition = Vector2Int.zero;

    private Vector2Int targetGridPosition;

    [SerializeField]
    private Transform movingTransform;

    private Vector2 startPosition;
    private Vector2 targetPosition;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    private Vector2Int GetMovementInput()
    {
        bool moveRight = Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow);
        bool moveLeft = Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow);
        bool moveUp = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow);
        bool moveDown = Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow);
        int xDirection = moveRight ? 1 : (moveLeft ? -1 : 0);
        int yDirection = moveUp ? 1 : (moveDown ? -1 : 0);
        return new Vector2Int(xDirection, yDirection);
    }


    // Update is called once per frame
    void Update()
    {
        if (isMoving)
        {
            Move();
            return;
        }
        Vector2Int newDirection = GetMovementInput();
        if (newDirection.magnitude > 0)
        {
            isMoving = true;
            PlayerCharacter.main.Animate(PlayerAnimation.Move);
            PlayerCharacter.main.FaceDirection(newDirection.x);
            targetGridPosition = gridPosition + newDirection;
            startPosition = movingTransform.position;
            targetPosition = targetGridPosition;
            moveTimer = 0f;
        }
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
            Vector2Int newDirection = GetMovementInput();
            if (newDirection.magnitude < 1)
            {
                PlayerCharacter.main.Animate(PlayerAnimation.Idle);
            }
        }
    }
}
