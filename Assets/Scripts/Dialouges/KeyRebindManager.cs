using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class KeyRebindManager : MonoBehaviour
{
    [SerializeField] PlayerControls playerControls;
    [SerializeField] List<CanvasGroup> startRebindButtons;
    //[SerializeField] GameObject waitingForInputObject;

    private InputActionRebindingExtensions.RebindingOperation rebindingOperation;
    private bool buttonSelected;
    private bool prebuttonSelected;

    private void Start()
    {
        playerControls = FindObjectOfType<PlayerInputManager>().GetPlayerControls();
        foreach (var button in startRebindButtons)
        {
            button.GetComponent<Button>().onClick.AddListener(() =>
            {
                buttonSelected = true;
                StartRebind(startRebindButtons.IndexOf(button));
            });
        }
    }

    private void Update()
    {
        if(buttonSelected && !prebuttonSelected)
            DisableAllButtons();

        prebuttonSelected = buttonSelected;
    }
    public void StartRebind(int index)
    {
        startRebindButtons[index].alpha = 0f;
        startRebindButtons[index].interactable = false;
        startRebindButtons[index].blocksRaycasts = false;
        //waitingForInputObject.SetActive(true);

        InputAction action = GetInputActionFromIndex(index);

        action.Disable();

        int bindingIndex = GetBindingIndexToRebind(action);


        rebindingOperation = action.PerformInteractiveRebinding(bindingIndex)
            .WithControlsExcluding("Mouse")
            .OnMatchWaitForAnother(0.1f)
            .OnComplete(operation =>
            {
                action.Enable();
                action.ApplyBindingOverride(bindingIndex, operation.selectedControl.path);
                RebindComplete(action, bindingIndex, index);
                operation.Dispose();
            })
            .Start();
    }

    private void RebindComplete(InputAction action, int bindingIndex, int index)
    {
        startRebindButtons[index].GetComponentInChildren<TextMeshProUGUI>().text = InputControlPath.ToHumanReadableString(
            action.bindings[bindingIndex].effectivePath,
            InputControlPath.HumanReadableStringOptions.OmitDevice);

        startRebindButtons[index].alpha = 1f;
        startRebindButtons[index].interactable = true;
        startRebindButtons[index].blocksRaycasts = true;
        EnableAllButtons();
        //waitingForInputObject.SetActive(false);
    }
    private int GetBindingIndexToRebind(InputAction action)
    {
        // Assumes the first non-composite binding is the one you want to rebind
        for (int i = 0; i < action.bindings.Count; i++)
        {
            if (!action.bindings[i].isComposite && !action.bindings[i].isPartOfComposite)
            {
                return i;
            }
        }
        return -1;
    }

    private InputAction GetInputActionFromIndex(int index)
    {
        InputAction action = index switch
        {
            0 => playerControls.Player.Jump,
            1 => playerControls.Player.Dash,
            2 => playerControls.Player.Sprint,
            3 => playerControls.Player.Shoot,
            4 => playerControls.Player.Aim,
            _ => playerControls.Player.Jump
        };

        return action;
    }

    private void DisableAllButtons()
    {
        startRebindButtons.ForEach(button =>
        {
            button.interactable = false;
            button.blocksRaycasts = false;
        });
    }
    private void EnableAllButtons()
    {
        startRebindButtons.ForEach(button =>
        {
            button.interactable = true;
            button.blocksRaycasts = true;
        });
    }
}
