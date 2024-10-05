using UnityEngine;

public class PlayerLevel : MonoBehaviour
{
    public static PlayerLevel main;
    void Awake()
    {
        main = this;
    }

    private int xp = 0;
    private int digPower = 0;
    public int DigPower { get { return digPower; } }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }


    public void GainXp(int value, Vector2 pos)
    {
        xp += value;
        Vector2 offset = new Vector2(-1f, 0f);
        UIManager.main.ShowMessage($"+ {value}xp", pos + offset, Color.cyan);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
