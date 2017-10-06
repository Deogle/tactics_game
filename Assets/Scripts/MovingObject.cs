using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MovingObject : MonoBehaviour
{
    private float moveTime = 0.1f;

    private Rigidbody2D rb2D;               //The Rigidbody2D component attached to this object.
    private float inverseMoveTime;

    public virtual void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        inverseMoveTime = 1f / moveTime;
    }

    public void Move(Vector3 loc)
    {
        Debug.Log("calling move");
        SmoothMovement(loc);
    }

    protected IEnumerator SmoothMovement(Vector3 end)
    {
        float sqrRemainingDistance = (transform.position - end).sqrMagnitude;
               
        while (sqrRemainingDistance > float.Epsilon)
        {
        
            Vector3 newPostion = Vector3.MoveTowards(rb2D.position, end, inverseMoveTime * Time.deltaTime);

            rb2D.MovePosition(newPostion);
            
            sqrRemainingDistance = (transform.position - end).sqrMagnitude;

            yield return null;
        }
    }
}
