using UnityEngine;

public class PlayerDigger : MonoBehaviour
{
    private bool isDigging = false;
    public bool IsDigging { get { return isDigging; } }
    private MoveResult moveResult;
    private float digTimer = 0f;
    [SerializeField]
    private float digDuration = 0.5f;

    private float tileHardness = 0f;
    public void StartDigging(MoveResult result)
    {
        digTimer = 0f;
        moveResult = result;
        isDigging = true;
        PlayerCharacter.main.Animate(PlayerAnimation.Dig);
        tileHardness = result.Tile != null ? result.Tile.DigPowerRequired * 0.1f : 0f;
        //Debug.Log("started diggin'");
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
        if (digTimer >= (tileHardness + digDuration - PlayerLevel.main.DigSpeed))
        {
            var digResult = WorldGrid.main.Dig(moveResult.Position);
            if (!digResult.Success)
            {
                UIManager.main.ShowMessage("*", moveResult.Attempt.Origin, Color.red);
                SoundManager.main.PlaySound(GameSoundType.Denied);
                isDigging = false;
                digTimer = 0f;
                return;
            }
            SoundManager.main.PlaySound(GameSoundType.Dig);
            if (digResult.Finished)
            {
                PlayerCharacter.main.Animate(PlayerAnimation.Empty);
                PlayerLevel.main.GainXp(moveResult.Tile.XpFinish, moveResult.Position);
                if (digResult.AfterDigPrefab)
                {
                    GameObject afterDig = Instantiate(digResult.AfterDigPrefab);
                    LootDrop lootDrop = afterDig.GetComponent<LootDrop>();
                    if (lootDrop != null)
                    {
                        lootDrop.Initialize(moveResult.Position, moveResult.Tile);
                    }
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
