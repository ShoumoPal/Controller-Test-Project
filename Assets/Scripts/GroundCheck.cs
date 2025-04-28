using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    private PlayerMovement player;
    [SerializeField] float distance;
    [SerializeField] LayerMask groundLayer;

    private void Start()
    {
        player = FindObjectOfType<PlayerMovement>();
    }

    private void Update()
    {
        //Debug.DrawRay(transform.position, Vector3.down, Color.red, distance);
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hitInfo, distance, groundLayer))
        {
            player.IsGrounded = true;
        }
        else
        {
            player.IsGrounded = false;
        }
    }
}
