using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerManager : MonoBehaviour
{
    /*This class should mantain a constant 
    / record of the list of units controlled by the player */

    //List of Units currently under control of the player
    public List<GameObject> player_unit_list;

    void Start()
    {
        player_unit_list = new List<GameObject>();
    }

    //Utility functions for adding and removing units
    public void AddUnit(GameObject unit_to_add)
    {
        player_unit_list.Add(unit_to_add);
        Debug.Log("Added: " + unit_to_add);
    }

    public void RemoveUnit(GameObject unit_to_remove)
    {
        player_unit_list.Remove(unit_to_remove);
        Debug.Log("Removed: " + unit_to_remove);
    }

}
