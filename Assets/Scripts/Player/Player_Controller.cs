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
    bool canChangeWeapon = false;
    bool canChangeArmor = false;
    public bool weaponDrawn = false;
    public bool canAim = false;
    public bool aiming = false;


    public GameObject defCinemachine;
    public GameObject aimCinemachine;
    public GameObject reticle;

    public GameObject gatheringItem;
    public WeaponTypeData weaponData;
    public ArmorData armorData;

    private void Start()
    {
        canRecieveInput = true;
        readyforFirstAttack = true;
        canGather = false;
        canChangeWeapon = false;

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

        if(aiming)
        {
            pAnimations.BowFire();
        }

        if (!readyforFirstAttack)
        {
            return;
        }

        readyforFirstAttack = false;
        weaponDrawn = true;

        switch (pEquipment.weaponData.WeaponType)
        {
            case WeaponTypeData.WeaponTypes.LightClub:
                pAnimations.LightClubStart();
                pAnimations.LCDrawn(weaponDrawn);
                break;
            case WeaponTypeData.WeaponTypes.HeavyClub:
                pAnimations.HeavyClubStart();
                pAnimations.HCDrawn(weaponDrawn);
                break;
            case WeaponTypeData.WeaponTypes.Bow:
                pAnimations.BowStart();
                pAnimations.BowDrawn(weaponDrawn);
                pEquipment.BowDrawn(weaponDrawn);
                canAim = true;
                return;
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
        if(!value.started)
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
        pAnimations.BowDrawn(weaponDrawn);
        pEquipment.BowDrawn(weaponDrawn);
        canAim = false;

        pAnimations.SheathWeight(1);
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
        if(canChangeWeapon)
        {
            CanvasManager.instance.ShowGatheredItem("You changed your weapon!", 1f);
            weaponDrawn = false;
            pAnimations.LCDrawn(weaponDrawn);
            pAnimations.HCDrawn(weaponDrawn);

            pAnimations.WeaponDrawn(weaponDrawn);
            pEquipment.WeaponDrawn(weaponDrawn);

            pAnimations.BowDrawn(weaponDrawn);
            pEquipment.UpdateSpawnWeapon(weaponData);
            return;
        }
        if(canChangeArmor)
        {
            CanvasManager.instance.ShowGatheredItem("You changed your Armor!", 1f);
            pEquipment.UpdateSpawnArmor(armorData);
        }
        if (!canGather)
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
    public void OnAim(InputAction.CallbackContext value)
    {
     
        if(!canAim)
        {
            return;
        }
        if(value.canceled)
        {
            Debug.Log("Not Aiming");
            aiming = false;
            pAnimations.IsAiming(aiming);
            defCinemachine.SetActive(true);
            aimCinemachine.SetActive(false);
            reticle.SetActive(false);
            pAnimations.StartAimBow(0);
        }
        if(!value.performed)
        {
            return;
        }

       


        pAnimations.StartAimBow(1);
        Debug.Log("Aiming");
        aiming = true;
        pAnimations.IsAiming(aiming);
        aimCinemachine.SetActive(true);
        defCinemachine.SetActive(false);
        reticle.SetActive(true);
        
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
            canChangeWeapon = true;
            weaponData = hit.GetComponent<ChangeWeapon>().weaponData;

        }
        else if (hit.transform.gameObject.GetComponent<ChangeArmor>() != null)
        {
            CanvasManager.instance.ShowGatheredItem("Press F to interact", 0.5f);
            canChangeArmor = true;
            armorData = hit.GetComponent<ChangeArmor>().armorData;

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
            canChangeWeapon = false;
        }
        else if (hit.transform.gameObject.GetComponent<ChangeArmor>() != null)
        {
            canChangeArmor = false;
        }
    } 
}
