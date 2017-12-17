using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerManager : MonoBehaviour
{
    /*This class should mantain a constant 
    / record of the list of units controlled by the player */

    //List of Units currently under control of the player
    public List<Unit> player_unit_list;

    void Start()
    {
        player_unit_list = new List<Unit>();
    }

    //Utility functions for adding and removing units
    public void AddUnit(GameObject unit_to_add)
    {
        player_unit_list.Add(unit_to_add.GetComponent<Unit>());
        //Debug.Log("Added: " + unit_to_add);
    }

    public void RemoveUnit(GameObject unit_to_remove)
    {
        player_unit_list.Remove(unit_to_remove.GetComponent<Unit>());
        //Debug.Log("Removed: " + unit_to_remove);
    }

    public bool EndOfTurn()
    {
        foreach(Unit u in player_unit_list)
        {
            if(u.hasMoved == false)
            {
                return false;
            }
        }
        Debug.Log("A new turn has started");
        return true;
    }

    public void ResetTurn()
    {
        foreach(Unit u in player_unit_list)
        {
            u.hasMoved = false;
        }
    }
}
