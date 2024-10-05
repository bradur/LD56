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

    void Start()
    {

    }
    void Update()
    {

    }

    public void Animate(PlayerAnimation animation)
    {

        animator.Play($"player{animation}");
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
    Eat
}
