using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Composites;
using UnityEngine.UI;
using UnityEngine.VFX;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    private CharacterController controller;
    private PlayerInputManager inputManager;
    private CinemachineFreeLook freeLook;

    [SerializeField] private CinemachineCameraOffset camOffset;
    [SerializeField] private float speed;
    [SerializeField] private bool canMove;
    [SerializeField] private bool faceCam;
    [SerializeField] private float smoothTime;
    [SerializeField] private Transform cam;
    [SerializeField] private VisualEffect sprintEffect;
    [SerializeField] private float gravity;
    [SerializeField] private float jumpHeight;
    [SerializeField] private ParticleSystem afterImage;
    [SerializeField] private float dashSpeed;
    [SerializeField] private float dashDuration;
    [SerializeField] private Transform holster;
    [SerializeField] private Image reticle;

    #region Is Grounded Property
    [SerializeField] private bool isGrounded;
    public bool IsGrounded
    {
        get => isGrounded;
        set
        {
            if (isGrounded != value)
                isGrounded = value;
        }
    }

    #endregion

    [HideInInspector]
    public Vector3 direction;

    private float turnVelocity;
    private Vector3 velocity;
    private Vector3 moveDirection;
    private float initialHolsterX;
    private bool isDashing;
    private bool hasTurned;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        inputManager = GetComponent<PlayerInputManager>();
        freeLook = FindObjectOfType<CinemachineFreeLook>();
        initialHolsterX = holster.transform.localEulerAngles.y;
        sprintEffect.Stop();
    }

    #region Sprint
    public void Sprint()
    {
        speed *= 5;
        if (sprintEffect != null)
            sprintEffect.Play(); 
    }
    public void CancelSprint()
    {
        speed /= 5;
        if(sprintEffect != null)
            sprintEffect.Stop(); 
    }
    #endregion

    private void Update()
    {
        if (inputManager.aim)
        {
            camOffset.m_Offset = Vector3.Lerp(camOffset.m_Offset, new(1f, 0f, 12f), 0.1f);
            faceCam = true;
            //cam.rotation = Quaternion.Euler(Mathf.Clamp(cam.localEulerAngles.x, 0f, 20f), cam.localEulerAngles.y, cam.localEulerAngles.z);
            holster.transform.localRotation = Quaternion.Euler(cam.localEulerAngles.x,
                                                          holster.transform.localEulerAngles.y,
                                                          holster.transform.localEulerAngles.z);
        }
        else
        {
            camOffset.m_Offset = Vector3.Lerp(camOffset.m_Offset, Vector3.zero, 0.1f);
            faceCam = false;
            holster.transform.localRotation = Quaternion.Euler(initialHolsterX,
                                                          holster.transform.localEulerAngles.y,
                                                          holster.transform.localEulerAngles.z);
        }

        // Cursor
        //if (Application.isPlaying && Cursor.visible)
        //    Cursor.visible = false;
            

        PlayerMove();
        PlayerJump();
        PlayerDash();
    }

    #region Player Dash Logic
    private void PlayerDash()
    {
        if (!isGrounded && inputManager.dash)
        {
            if (!isDashing)
                StartCoroutine(DashCR());

            controller.Move(moveDirection * dashSpeed * Time.deltaTime);
        }
    }

    IEnumerator DashCR()
    {
        isDashing = true;
        afterImage.Play();
        yield return new WaitForSeconds(dashDuration);
        inputManager.dash = false;
        afterImage.Stop();
        isDashing = false;
    } 
    #endregion

    #region Player Jump Logic
    private void PlayerJump()
    {
        // Simulate gravity
        if (isGrounded && velocity.y < 0)
            velocity.y = -2f;

        if (inputManager.jump && isGrounded)
            velocity.y = Mathf.Sqrt(2 * -gravity * jumpHeight);

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        inputManager.jump = false;
    } 
    #endregion

    #region Player Move Logic
    private void PlayerMove()
    {
        if (canMove && direction.magnitude > 0.1f)
        {
            // Initial turn 
            if (!hasTurned)
            {
                transform.rotation = Quaternion.Euler(transform.localEulerAngles.x,
                                                                  cam.localEulerAngles.y,
                                                                  transform.localEulerAngles.z); 
                hasTurned = true;
            }

            faceCam = true;

            // Make player face direction
            var angle = (faceCam) ? (Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.localEulerAngles.y)
                                  : (Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg);

            var smoothedAngle = Mathf.SmoothDampAngle(transform.localEulerAngles.y,
                                                      angle,
                                                      ref turnVelocity,
                                                      smoothTime);

            moveDirection = Quaternion.Euler(transform.localEulerAngles.x,
                                             angle,
                                             transform.localEulerAngles.z) * Vector3.forward;

            controller.Move(speed * Time.deltaTime * moveDirection.normalized);

            transform.rotation = Quaternion.Euler(transform.localEulerAngles.x,
                                                  smoothedAngle,
                                                  transform.localEulerAngles.z);

            // Look rotation
            if(inputManager.aim)
            {
                transform.rotation = Quaternion.Euler(transform.localEulerAngles.x,
                                                          cam.localEulerAngles.y,
                                                          transform.localEulerAngles.z);
            }
        }
        else
        {
            hasTurned = false;
            if (inputManager.aim)
            {
                transform.rotation = Quaternion.Euler(transform.localEulerAngles.x,
                                                          cam.localEulerAngles.y,
                                                          transform.localEulerAngles.z);
            }
            else
                faceCam = false;
        }
    } 
    #endregion

    public void SetRecenter(bool state)
    {
        freeLook.m_YAxisRecentering.m_enabled = state;
    }
}
