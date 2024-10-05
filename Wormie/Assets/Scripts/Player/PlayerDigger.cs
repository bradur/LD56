using UnityEngine;

public class PlayerDigger : MonoBehaviour
{
    private bool isDigging = false;
    public bool IsDigging { get { return isDigging; } }
    private Vector2Int targetPosition;
    private float digTimer = 0f;
    [SerializeField]
    private float digDuration = 0.5f;
    public void StartDigging(MoveResult result)
    {
        targetPosition = result.Position;
        isDigging = true;
        PlayerCharacter.main.Animate(PlayerAnimation.Dig);
        Debug.Log("started diggin'");
    }

    void Update()
    {
        if (!isDigging)
        {
            return;
        }
        digTimer += Time.deltaTime;
        if (!PlayerCharacter.main.InputExists())
        {
            isDigging = false;
            digTimer = 0f;
        }
        if (digTimer >= digDuration)
        {
            bool digFinished = WorldGrid.main.Dig(targetPosition);
            if (digFinished)
            {
                PlayerCharacter.main.Animate(PlayerAnimation.Empty);
            }
            else
            {
                PlayerCharacter.main.AnimateReset();
            }

            isDigging = false;
            digTimer = 0f;
        }
    }
}
