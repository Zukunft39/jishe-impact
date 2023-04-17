using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    public float interactRayLength;
    public LayerMask interactableMask;

    private Player playerCon;
    private void Awake()
    {
        if (GetComponent<Player>() != null)
        {
            playerCon = GetComponent<Player>();
        }
    }
    private void Update()
    {
        if (playerCon.isChatting) return;
        if (Input.GetKeyDown(KeyCode.E)||Input.GetKeyDown(KeyCode.Joystick1Button0))
        {
            Interact();
        }
    }

    void Interact()
    {
        Ray2D ray;
        RaycastHit2D hit;
        if (playerCon.Playertran.x > 0)
        {
            ray = new Ray2D(transform.position, Vector2.right);
        }
        else
        {
            ray = new Ray2D(transform.position, Vector2.left);
        }
        hit= Physics2D.Raycast(transform.position,
            playerCon.Playertran.x > 0 ? Vector2.right : Vector2.left, interactRayLength, interactableMask);
        if (hit.collider != null&&hit.collider.GetComponent<InteractableObject>()!=null)
        {
            InteractableObject hitObj = hit.collider.GetComponent<InteractableObject>();
            if (hitObj.interactEvent != null) hitObj.interactEvent.Invoke();
        }
    }
}
