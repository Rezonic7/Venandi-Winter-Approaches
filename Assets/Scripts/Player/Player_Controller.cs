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
    private Player_Inventory pInventory;
    private bool canChangeWeapon = false;
    private bool canChangeArmor = false;

    [SerializeField] private GameObject defCinemachine;
    [SerializeField] private GameObject aimCinemachine;
    [SerializeField] private GameObject reticle;
    [SerializeField] private GameObject inventory;

    public bool canRecieveInput = true;
    public bool inputRecieved;
    public bool readyforFirstAttack = true;
    public bool canGather = false;
    public bool weaponDrawn = false;
    public bool canAim = false;
    public bool aiming = false;

    public GameObject gatheringItem;
    public WeaponTypeData weaponData;
    public ArmorData armorData;

    private void Start()
    {
        canAim = false;
        canRecieveInput = true;
        readyforFirstAttack = true;
        canGather = false;
        canChangeWeapon = false;
        inventory.SetActive(false);

        pMovement = GetComponent<Player_Movement>();
        pAnimations = GetComponent<Player_Animations>();
        pVariables = GetComponent<Player_Variables>();
        pEquipment = GetComponent<Player_Equipment>();
        pInventory = GetComponent<Player_Inventory>();
    }

    private void Update()
    {
        if(pMovement.isRunning)
        {
            if(pVariables.UseStamina(0.1f))
            {
                pAnimations.Run(true);
                pMovement.maxSpeed = pMovement.maxSpeed = pMovement.runSpeed;
            }
            else
            {
                pMovement.isRunning = false;
                pAnimations.Run(false);
                pMovement.maxSpeed = pMovement.maxSpeed = pMovement.walkSpeed;
            }
        }
        else
        {
            pAnimations.Run(false);
            pMovement.maxSpeed = pMovement.maxSpeed = pMovement.walkSpeed;
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
    public void OnToggleInventory(InputAction.CallbackContext value)
    {
        if(!value.started)
        {
            return;
        }
        if(inventory.activeSelf)
        {
            inventory.SetActive(false);
        }
        else
        {
            inventory.SetActive(true);
        }
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
    public void OnMovement(InputAction.CallbackContext value)
    {
        Vector2 inputMovement = value.ReadValue<Vector2>();
        pMovement.movement = inputMovement;
    }
    public void OnRun(InputAction.CallbackContext value)
    {
        if (value.canceled)
        {
            if(!pMovement.isRunning)
            {
                return;
            }
            if(weaponDrawn)
            {
                return;
            }
            pMovement.isRunning = false;
        }

        if (!value.started)
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
        if (!pVariables.UseStamina(0.1f))
        {
            return;
        }
        if (weaponDrawn)
        {
            if (aiming)
            {
                return;
            }
            SheathWeapon();
        }
        pMovement.isRunning = true;
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
        if(aiming)
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
        if(canAim)
        {
            if (aiming)
            {
                inputRecieved = true;
                canRecieveInput = false;
                pAnimations.BowFire();
                return;
            }
            return;
        }

        inputRecieved = true;
        canRecieveInput = false;
        weaponDrawn = true;

        if(pMovement.isRunning)
        {
            pAnimations.JumpAttack();
            switch (pEquipment.WeaponData.WeaponType)
            {
                case WeaponTypeData.WeaponTypes.LightClub:
                    pAnimations.LCDrawn(weaponDrawn);
                    break;
                case WeaponTypeData.WeaponTypes.HeavyClub:
                    pAnimations.HCDrawn(weaponDrawn);
                    break;
            }
            pAnimations.WeaponDrawn(weaponDrawn);
            pEquipment.WeaponDrawn(weaponDrawn);
            return;
        }

        if (!readyforFirstAttack)
        {
            return;
        }

        readyforFirstAttack = false;

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
            case WeaponTypeData.WeaponTypes.Bow:
                pAnimations.BowStart();
                pAnimations.BowDrawn(weaponDrawn);
                pEquipment.BowDrawn(weaponDrawn);
                canAim = true;
                break;
        }

        if (canAim)
        {
            return;
        }
        pAnimations.WeaponDrawn(weaponDrawn);
        pEquipment.WeaponDrawn(weaponDrawn);
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
        if(aiming)
        {
            return;
        }
        SheathWeapon();
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
            CanvasManager.instance.ShowInfo("You changed your weapon!", 1f);
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
            CanvasManager.instance.ShowInfo("You changed your Armor!", 1f);
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
    void SheathWeapon()
    {
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

    private void OnTriggerEnter(Collider hit)
    {
        if(hit.transform.gameObject.GetComponent<GatheringSpots>() != null)
        {
            CanvasManager.instance.ShowInfo("Press F to interact", 0.5f);
            canGather = true;
            gatheringItem = hit.gameObject;
            
        }
        else if(hit.transform.gameObject.GetComponent<ChangeWeapon>() != null)
        {
            CanvasManager.instance.ShowInfo("Press F to interact", 0.5f);
            canChangeWeapon = true;
            weaponData = hit.GetComponent<ChangeWeapon>().WeaponData;

        }
        else if (hit.transform.gameObject.GetComponent<ChangeArmor>() != null)
        {
            CanvasManager.instance.ShowInfo("Press F to interact", 0.5f);
            canChangeArmor = true;
            armorData = hit.GetComponent<ChangeArmor>().ArmorData;

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
