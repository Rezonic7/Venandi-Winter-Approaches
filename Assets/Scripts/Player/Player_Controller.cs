using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player_Controller : Singleton<Player_Controller>
{
    private Player_Movement pMovement;
    private Player_Animations pAnimations;
    private Player_Variables pVariables;
    private Player_Equipment pEquipment;

    public bool canRecieveInput = true;
    public bool inputRecieved;
    public bool readyforFirstAttack = true;
    public bool canGather = false;
    bool canChangeEquipment = false;
    public bool weaponDrawn = false;

    public GameObject gatheringItem;
    public WeaponTypeData weaponData;

    private void Start()
    {
        canRecieveInput = true;
        readyforFirstAttack = true;
        canGather = false;
        canChangeEquipment = false;

        pMovement = GetComponent<Player_Movement>();
        pAnimations = GetComponent<Player_Animations>();
        pVariables = GetComponent<Player_Variables>();
        pEquipment = GetComponent<Player_Equipment>();
    }

    private void Update()
    {
        if(pMovement.isRunning)
        {
            if(pVariables.UseStamina(0.1f))
            {
                pAnimations.Run(true);
            }
            else
            {
                pMovement.isRunning = false;
                pAnimations.Run(false);
                pMovement.maxSpeed = pMovement.maxSpeed / 1.5f;
            }
        }
        if(pMovement.movement.magnitude >= 0.1f)
        {
            pAnimations.Walk(true);
        }
        else
        {
            pAnimations.Walk(false);
        }
    }

    public void OnMovement(InputAction.CallbackContext value)
    {
        Vector2 inputMovement = value.ReadValue<Vector2>();
        pMovement.movement = inputMovement;
    }
    public void OnAttack(InputAction.CallbackContext value)
    {
        if (!value.started)
        {
            return;
        }
        if(!canRecieveInput)
        {
            return;
        }

        inputRecieved = true;
        canRecieveInput = false;

        if (!readyforFirstAttack)
        {
            return;
        }

        readyforFirstAttack = false;
        weaponDrawn = true;

        switch (pEquipment.WeaponData.WeaponType)
        {
            case WeaponTypeData.WeaponTypes.LightClub:
                pAnimations.LightClubStart();
                pAnimations.LCDrawn(weaponDrawn);
                break;
            case WeaponTypeData.WeaponTypes.HeavyClub:
                pAnimations.HeavyClubStart();
                pAnimations.HCDrawn(weaponDrawn);
                break;
        }
        if(!weaponDrawn)
        {
            return;
        }
        pAnimations.WeaponDrawn(weaponDrawn);
        pEquipment.WeaponDrawn(weaponDrawn);

    }

    public void OnHideWeapon(InputAction.CallbackContext value)
    {
        if(!value.performed)
        {
            return;
        }
        if(!canRecieveInput)
        {
            return;
        }
        if(!weaponDrawn)
        {
            return;
        }
        weaponDrawn = false;
        pAnimations.LCDrawn(weaponDrawn);
        pAnimations.HCDrawn(weaponDrawn);

        pAnimations.WeaponDrawn(weaponDrawn);
        pEquipment.WeaponDrawn(weaponDrawn);
    }

    public void InputManager()
    {
        if(!canRecieveInput)
        {
            inputRecieved = true;
        }
        else
        {
            inputRecieved = false;
        }
    }
    public void OnRun(InputAction.CallbackContext value)
    {
        if (value.canceled)
        {
            if(weaponDrawn)
            {
                return;
            }
            pMovement.isRunning = false;
            pAnimations.Run(false);
            pMovement.maxSpeed = pMovement.maxSpeed / 1.5f;
        }

        if (!value.started)
        {
            return;
        }
        if (pMovement.movement.magnitude <= 0.1f)
        {
            return;
        }
        if (!pVariables.UseStamina(0.1f))
        {
            return;
        }
        if (weaponDrawn)
        {
            return;
        }
        pMovement.isRunning = true;
        pMovement.maxSpeed = pMovement.maxSpeed * 1.5f;

        
    }

    public void OnInteract(InputAction.CallbackContext value)
    {
        if(!value.started)
        {
            return;
        }
        if(!canRecieveInput)
        {
            return;
        }
        if(canChangeEquipment)
        {
            CanvasManager.instance.ShowGatheredItem("You changed your weapon!", 1f);
            weaponDrawn = false;
            pAnimations.LCDrawn(weaponDrawn);
            pAnimations.HCDrawn(weaponDrawn);

            pAnimations.WeaponDrawn(weaponDrawn);
            pEquipment.WeaponDrawn(weaponDrawn);

            pEquipment.UpdateSpawnWeapon(weaponData);
        }
        if(!canGather)
        {
            return;
        }
        if(weaponDrawn)
        {
            return;
        }
        inputRecieved = true;
        canRecieveInput = false;
        pAnimations.Gather();
       
    }

    public void OnRoll(InputAction.CallbackContext value)
    {
        if(!value.started)
        {
            return;
        }
        if(!canRecieveInput)
        {
            return;
        }
        if (pMovement.movement.magnitude <= 0.1f)
        {
            return;
        }
        if(pMovement.isRolling)
        {
            return;
        }
        if (!pVariables.UseStamina(25))
        {
            return;
        }
        pMovement.captureDirection = pMovement.movement;
        pAnimations.Roll();
        pMovement.Roll();
    }

   

    private void OnTriggerEnter(Collider hit)
    {
        if(hit.transform.gameObject.GetComponent<GatheringSpots>() != null)
        {
            CanvasManager.instance.ShowGatheredItem("Press F to interact", 0.5f);
            canGather = true;
            gatheringItem = hit.gameObject;
            
        }
        else if(hit.transform.gameObject.GetComponent<ChangeWeapon>() != null)
        {
            CanvasManager.instance.ShowGatheredItem("Press F to interact", 0.5f);
            canChangeEquipment = true;
            weaponData = hit.GetComponent<ChangeWeapon>().weaponData;

        }
    }
    private void OnTriggerExit(Collider hit)
    {
        if (hit.transform.gameObject.GetComponent<GatheringSpots>() != null)
        {
            canGather = false;
            gatheringItem = null;
        }
        else if (hit.transform.gameObject.GetComponent<ChangeWeapon>() != null)
        {
            canChangeEquipment = false;
        }
    } 
}
