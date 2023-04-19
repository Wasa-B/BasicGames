using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu]
public class SiblingsRuleTile : RuleTile<SiblingsRuleTile.Neighbor>
{
    public List<TileBase> sibings = new List<TileBase>();

    public class Neighbor : RuleTile.TilingRule.Neighbor
    {
        public const int Sbing = 3;
    }



    public override bool RuleMatch(int neighbor, TileBase tile)
    {

        switch (neighbor)
        {
            case Neighbor.Sbing: return sibings.Contains(tile);
        }
        return base.RuleMatch(neighbor, tile);
    }
}