using UnityEngine;

public class PlayerDigger : MonoBehaviour
{
    private bool isDigging = false;
    public bool IsDigging { get { return isDigging; } }
    private MoveResult moveResult;
    private float digTimer = 0f;
    [SerializeField]
    private float digDuration = 0.5f;
    public void StartDigging(MoveResult result)
    {
        digTimer = 0f;
        moveResult = result;
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
        if (!PlayerCharacter.main.InputExists() && !PlayerCharacter.main.InputMatches(moveResult))
        {
            isDigging = false;
            digTimer = 0f;
        }
        if (digTimer >= digDuration)
        {
            var digResult = WorldGrid.main.Dig(moveResult.Position);
            if (digResult.Finished)
            {
                PlayerCharacter.main.Animate(PlayerAnimation.Empty);
                PlayerLevel.main.GainXp(moveResult.Tile.XpFinish, moveResult.Position);
                if (digResult.AfterDigPrefab)
                {
                    Instantiate(digResult.AfterDigPrefab);
                }
            }
            else
            {
                PlayerLevel.main.GainXp(moveResult.Tile.Xp, moveResult.Position);
                PlayerCharacter.main.AnimateReset();
            }

            isDigging = false;
            digTimer = 0f;
        }
    }
}
