using UnityEngine;
using UnityEngine.UI;

public class UIGoodie : MonoBehaviour
{
    private WorldTile wTile;
    public string Prefix { get { return wTile.Prefix; } }

    [SerializeField]
    private Image imgIcon;
    [SerializeField]
    private Image imgBorder;
    [SerializeField]
    private Color foundColor;
    void Start()
    {

    }

    public void Initialize(WorldTile wTile)
    {
        this.wTile = wTile;
        Sprite sprite = wTile.AfterDigPrefab.GetComponent<LootDrop>()?.Loot?.Sprite;

        imgIcon.sprite = sprite;
    }

    public void Consume()
    {
        imgIcon.color = foundColor;
        imgBorder.color = foundColor;
        Debug.Log("Consumed!");
    }

    // Update is called once per frame
    void Update()
    {

    }
}
