using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MovingObject {

    public int moveRange;
    public int damage;
    public int health;
    public int xp;
    public int speed;
    public bool isSelected = false;

    private Color thisColor;

    public int x, y;

    public override void Start()
    {
        base.Start();
        thisColor = this.GetComponent<SpriteRenderer>().material.color;
    }

    public void Select()
    {
        isSelected = !isSelected;
        if (isSelected)
            UnitSelected();
        else
            UnitDeselected();
    }

    private void UnitSelected()
    {
        //TODO Highlight unit and list possible moves
        SpriteRenderer mat = this.GetComponent<SpriteRenderer>();
        Color newColor = new Color(250,255,0);
        mat.material.SetColor("_Color", newColor);
    }

    private void UnitDeselected()
    {
        SpriteRenderer mat = this.GetComponent<SpriteRenderer>();
        mat.material.SetColor("_Color", thisColor);
    }
}
