using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickableTile : MonoBehaviour {
    public float tileX;
    public float tileY;
    public Map map;
    public bool IsHighlighted = false;
    public bool ContainsUnit = false;

    Color defaultColor;

    private void Start()
    {
        defaultColor = this.gameObject.GetComponent<SpriteRenderer>().color;
    }

    private void OnMouseUp()
    {
        if (IsHighlighted)
        {
            map.selectedUnit.GetComponent<Unit>().MakeCurrentPath(tileX, tileY);
            map.ClearAllMoves();
        }
        else if (map.addUnitMode)
        {
            map.SpawnUnits((int)tileX, (int)tileY);
        }
    }

    public void ClearMove()
    {
        this.gameObject.GetComponent<SpriteRenderer>().color = defaultColor;
        IsHighlighted = false;
    }

    public void HighlightPossibleMove()
    {
        this.gameObject.GetComponent<SpriteRenderer>().color = Color.blue;
        IsHighlighted = true;
    }

}
