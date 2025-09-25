using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public abstract class InteractableObject:MonoBehaviour
{
    Collider2D Objectcollider;
    private bool interactable = false;

    private void Start()
    {
        Objectcollider = GetComponent<BoxCollider2D>();
        Objectcollider.isTrigger = true;
        gameObject.layer = LayerMask.NameToLayer("Interactable");
    }

/*    private void OnTriggerStay2D(Collider2D collision)
    {
        Debug.Log("Player stay");
        if (Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log("F pressed");
            if (collision.tag == "Player")
            {
                Debug.Log("F pressed");
                OnInteractable();
            }
        }
    }*/

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            if(_GamePlayer.Instance.interactTarget != this)
            {
                _GamePlayer.Instance.interactTarget = this;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (_GamePlayer.Instance.interactTarget == this)
            {
                _GamePlayer.Instance.interactTarget = null;
            }
        }
    }


    public abstract void OnInteractable();

    
}
