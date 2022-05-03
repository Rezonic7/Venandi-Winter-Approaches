using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Player_Controller : Singleton<Player_Controller>
{
    private Player_Movement pMovement;
    private Player_Animations pAnimations;
    private Player_Variables pVariables;
    private Player_Equipment pEquipment;
    private Player_Inventory pInventory;
    private bool canChangeWeapon = false;
    private bool canChangeArmor = false;
    private bool canRunAttack = false;
    private bool HC_SpecialCharge = false;
    private float HC_SpecialChargeTime;

    private PlayerInput playerInput;

    [SerializeField] private GameObject inventoryCinemachine;
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
    public bool isDoingSpecialAttack = false;
    public Image chargingImage;

    public GameObject gatheringItem;
    public WeaponTypeData weaponData;
    public ArmorData armorData;

    private void Start()
    {
        HC_SpecialCharge = false;
        canAim = false;
        canRecieveInput = true;
        readyforFirstAttack = true;
        canGather = false;
        canChangeWeapon = false;
        isDoingSpecialAttack = false;

    inventory.SetActive(false);
        playerInput = GetComponent<PlayerInput>();

        pMovement = GetComponent<Player_Movement>();
        pAnimations = GetComponent<Player_Animations>();
        pVariables = GetComponent<Player_Variables>();
        pEquipment = GetComponent<Player_Equipment>();
        pInventory = GetComponent<Player_Inventory>();
    }

    private void Update()
    {
        IsRunning();
        ChargeHC_Special();
    }
    #region ButtonInputs
    public void OnToggleInventory(InputAction.CallbackContext value)
    {
        if(!value.started)
        {
            return;
        }
        if(aiming)
        {
            return;
        }
        if(inventory.activeSelf)
        {
            ShowInventory();
        }
        else
        {
            HideInventory();
        }
    }
    void ShowInventory()
    {
        playerInput.SwitchCurrentActionMap("Player3DMovement");
        defCinemachine.SetActive(true);
        inventoryCinemachine.SetActive(false);
        inventory.SetActive(false);
    }
    void HideInventory()
    {
        playerInput.SwitchCurrentActionMap("Inventory");
        defCinemachine.SetActive(false);
        inventoryCinemachine.SetActive(true);
        inventory.SetActive(true);
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
                DoAction();
                pAnimations.BowFire();
                return;
            }
            return;
        }

        if(pMovement.isRunning)
        {
            switch (pEquipment.WeaponData.WeaponType)
            {
                case WeaponTypeData.WeaponTypes.LightClub:
                    canRunAttack = true;
                    pAnimations.LCDrawn(true);
                    break;
                case WeaponTypeData.WeaponTypes.HeavyClub:
                    canRunAttack = true;
                    pAnimations.HCDrawn(true);
                    break;
                case WeaponTypeData.WeaponTypes.Bow:
                    canRunAttack = false;
                    break;
            }

            if (!canRunAttack)
            {
                return;
            }
            DoAction();
            DrawWeapon();
            pAnimations.RunAttack();
           
            return;
        }

        DoAction();

        if (!readyforFirstAttack)
        {
            return;
        }

        readyforFirstAttack = false;
        DrawWeapon();

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
    }
    public void OnSpecialAttack(InputAction.CallbackContext value)
    {
        if(canAim)
        {
            if (value.canceled)
            {
                Debug.Log("Not Aiming");
                aiming = false;
                pAnimations.IsAiming(aiming);
                defCinemachine.SetActive(true);
                aimCinemachine.SetActive(false);
                reticle.SetActive(false);
                pAnimations.StartAimBow(0);
            }
            if (!value.performed)
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
            return;
        }
        else
        {
            pMovement.isRunning = false;
            if (value.canceled)
            {
                if(HC_SpecialCharge)
                {
                    HC_SpecialCharge = false;
                    if (HC_SpecialChargeTime >= 3.2f)
                    {
                        pAnimations.HC_SP("HC_SP3");
                    }
                    else if (HC_SpecialChargeTime >= 1.75f)
                    {
                        pAnimations.HC_SP("HC_SP2");
                    }
                    else if (HC_SpecialChargeTime >= 0)
                    {
                        pAnimations.HC_SP("HC_SP1");
                    }
                    chargingImage.gameObject.SetActive(false);
                    return;
                }
            }
            if (!value.started)
            {
                return;
            }
            if (isDoingSpecialAttack)
            {
                return;
            }
            switch (pEquipment.WeaponData.WeaponType)
            {
                case WeaponTypeData.WeaponTypes.LightClub:
                    pAnimations.LCDrawn(true);
                    break;
                case WeaponTypeData.WeaponTypes.HeavyClub:
                    pAnimations.HCDrawn(true);
                    HC_SpecialCharge = true;
                    HC_SpecialChargeTime = 0;
                    chargingImage.gameObject.SetActive(true);
                    break;
            }
            DrawWeapon();
            pAnimations.SpecialTrigger();
            isDoingSpecialAttack = true;
        }

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
            pEquipment.UpdateSpawnWeapon(weaponData);
            SheathWeapon();
            return;
        }
        if(canChangeArmor)
        {
            CanvasManager.instance.ShowInfo("You changed your Armor!", 1f);
            pEquipment.UpdateSpawnArmor(armorData);
            return;
        }
        if (!canGather)
        {
            return;
        }
        if(weaponDrawn)
        {
            SheathWeapon();
        }
        DoAction();
        pAnimations.Gather();
       
    }
    #endregion ButtonInputs
    void IsRunning()
    {
        if (pMovement.isRunning)
        {
            if (pVariables.UseStamina(0.1f))
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
        if (pMovement.movement.magnitude >= 0.1f)
        {
            pAnimations.Walk(true);
        }
        else
        {
            pAnimations.Walk(false);
        }
    }
    void ChargeHC_Special()
    {
        if (HC_SpecialCharge)
        {
            HC_SpecialChargeTime += Time.deltaTime;
            if (HC_SpecialChargeTime >= 3.2f)
            {
                chargingImage.color = Color.red;
                if (!Player_Controller.instance.HC_SpecialCharge)
                {
                    Player_Animations.instance.HC_SP("HC_SP3");
                }
            }
            else if (HC_SpecialChargeTime >= 1.75f)
            {
                chargingImage.color = Color.yellow;
                if (!Player_Controller.instance.HC_SpecialCharge)
                {
                    Player_Animations.instance.HC_SP("HC_SP2");
                }
            }
            else if (HC_SpecialChargeTime >= 0)
            {
                chargingImage.color = Color.green;
                if (!Player_Controller.instance.HC_SpecialCharge)
                {
                    Player_Animations.instance.HC_SP("HC_SP1");
                }
            }
        }
    }
    void DoAction()
    {
        inputRecieved = true;
        canRecieveInput = false;
    }
    void DrawWeapon()
    {
        weaponDrawn = true;
        pAnimations.WeaponDrawn(weaponDrawn);
        pEquipment.WeaponDrawn(weaponDrawn);
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
