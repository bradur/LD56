using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UIGoodieDisplay : MonoBehaviour
{
    [SerializeField]
    private UIGoodie uiGoodiePrefab;
    private List<UIGoodie> uiGoodies = new();
    [SerializeField]
    private Transform goodieContainer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    public void Initialize(List<WorldTile> wTiles)
    {
        List<WorldTile> sorted = wTiles.OrderBy(wTile => wTile.Prefix).ToList();
        foreach (WorldTile wTile in sorted)
        {
            UIGoodie goodie = Instantiate(uiGoodiePrefab, goodieContainer);
            goodie.Initialize(wTile);
            uiGoodies.Add(goodie);
        }
    }

    public void Consume(WorldTile wTile)
    {
        UIGoodie goodie = uiGoodies.Find(uiGoodie => uiGoodie.Prefix == wTile.Prefix);
        goodie.Consume();
        uiGoodies.Remove(goodie);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
