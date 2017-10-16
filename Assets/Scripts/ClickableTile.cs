using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickableTile : MonoBehaviour {
    public float tileX;
    public float tileY;
    public Map map;
    private bool IsHighlighted = false;

    private void OnMouseUp()
    {
        if (IsHighlighted)
        {
            map.selectedUnit.GetComponent<Unit>().MoveNextTile();
        }
    }

    public void HighlightPossibleMove()
    {
        this.gameObject.GetComponent<SpriteRenderer>().color = Color.blue;
        IsHighlighted = true;
    }

}
