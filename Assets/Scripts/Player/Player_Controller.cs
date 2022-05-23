using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Player_Controller : Singleton<Player_Controller>
{
    private GameObject inventoryCinemachine;
    private GameObject defCinemachine;
    private GameObject aimCinemachine;
    private GameObject reticle;
    private GameObject inventory;

    //debug purposes remove when final
    private Image chargingImage;

    private Player_Movement pMovement;
    private Player_Animations pAnimations;
    private Player_Variables pVariables;
    private Player_Equipment pEquipment;
    private Player_Inventory pInventory;
    private PlayerInput playerInput;
    
    private float HC_SpecialChargeTime;
    private bool canChangeWeapon = false;
    private bool canChangeArmor = false;
    private bool canRunAttack = false;
    private bool HC_SpecialCharge = false;
    private bool _canRecieveInput = true;
    private bool _inputRecieved;
    private bool _readyforFirstAttack = true;
    private bool _canGather = false;
    private bool _weaponDrawn = false;
    private bool _canAim = false;
    private bool _aiming = false;
    private bool _isDoingSpecialAttack = false;
    private bool _isUsingItem = false;
    private bool _canWalk = true;
    private bool _canRoll = true;
    private bool _isRolling = false;

    public bool IsRolling { get { return _isRolling; } set { _isRolling = value; } }
    public bool CanRoll { get { return _canRoll; } set { _canRoll = value; } }
    public bool CanRecieveInput { get { return _canRecieveInput; } set { _canRecieveInput = value; } }
    public bool InputRecieved { get { return _inputRecieved; } set { _inputRecieved = value; } }
    public bool ReadyforFirstAttack { get { return _readyforFirstAttack; } set { _readyforFirstAttack = value; } }
    public bool CanGather { get { return _canGather; } set { _canGather = value; } }
    public bool WeaponDrawn { get { return _weaponDrawn; } set { _weaponDrawn = value; } }
    public bool CanAim { get { return _canAim; } set { _canAim = value; } }
    public bool Aiming { get { return _aiming; } set { _aiming = value; } }
    public bool IsDoingSpecialAttack { get { return _isDoingSpecialAttack; } set { _isDoingSpecialAttack = value; } }
    public bool IsUsingItem { get { return _isUsingItem; } set { _isUsingItem = value; } }
    public bool CanWalk { get { return _canWalk; } set { _canWalk = value; } }


    //debugs remove when final
    private GameObject _gatheringItem;
    private WeaponTypeData _weaponData;
    private  ArmorData _armorData;
    public GameObject GatheringItem { get { return _gatheringItem; } set { _gatheringItem = value; } }
    public WeaponTypeData WeaponData { get { return _weaponData; } }
    public ArmorData ArmorData { get { return _armorData; } }
    //debugs remove when final


    private void Start()
    {
        defCinemachine = GameObject.FindWithTag("DefaultCinemachin")?.gameObject;
        inventoryCinemachine = GameObject.FindWithTag("InventoryCinemachine")?.gameObject;
        aimCinemachine = GameObject.FindWithTag("AimCinemachine")?.gameObject;
        reticle = GameObject.FindWithTag("Reticle")?.gameObject;
        inventory = GameObject.FindWithTag("Inventory")?.gameObject;
        chargingImage = GameObject.FindWithTag("ChargingImage")?.GetComponent<Image>();

        if(reticle || chargingImage)
        {
            reticle.SetActive(false);
            chargingImage.gameObject.SetActive(false);
        }
        else
        {
            Debug.Log("Heads up! Some UI elements may not work properly, some UI are missing.");
        }

        if(defCinemachine || inventoryCinemachine || aimCinemachine)
        {
            defCinemachine.SetActive(true);
            inventoryCinemachine.SetActive(false);
            aimCinemachine.SetActive(false);
        }
        else
        {
            Debug.Log("Heads up! Cinemachines may not work propely, some Cinemachine's are missing.");
        }

        _canRoll = true;
        _canWalk = true;
        _isUsingItem = false;
        HC_SpecialCharge = false;
        _canAim = false;
        _canRecieveInput = true;
        _readyforFirstAttack = true;
        _canGather = false;
        canChangeWeapon = false;
        _isDoingSpecialAttack = false;

        playerInput = GetComponent<PlayerInput>();
        pMovement = GetComponent<Player_Movement>();
        pAnimations = GetComponent<Player_Animations>();
        pVariables = GetComponent<Player_Variables>();
        pEquipment = GetComponent<Player_Equipment>();
        pInventory = GetComponent<Player_Inventory>();
        
        if(inventory)
        {
            inventory.SetActive(false);
        }
        else
        {
            Debug.Log("Heads Up! Inventory not found in scene.");
        }
        
    }

    private void Update()
    {
        IsRunning();
        if(chargingImage)
        {
            ChargeHC_Special();
        }
    }
    #region ButtonInputs
    public void OnToggleInventory(InputAction.CallbackContext value)
    {
        if(!inventory)
        {
            return;
        }
        if (!defCinemachine || !inventoryCinemachine || !aimCinemachine)
        {
            return;
        }
        if (!value.started)
        {
            return;
        }
        if(_aiming)
        {
            return;
        }
        if(inventory.activeSelf)
        {
            HideInventory();
        }
        else
        {
            ShowInventory();
        }
    }
    void HideInventory()
    {
        if (Player_Inventory.instance.IsMovingItem)
        {
            Player_Inventory.instance.ReturnToOriginalSlot();
        }
        playerInput.SwitchCurrentActionMap("Player3DMovement");
        defCinemachine.SetActive(true);
        inventoryCinemachine.SetActive(false);
        inventory.SetActive(false);
    }
    void ShowInventory()
    {
        playerInput.SwitchCurrentActionMap("Inventory");
        defCinemachine.SetActive(false);
        inventoryCinemachine.SetActive(true);
        inventory.SetActive(true);
    }
    public void InputManager()
    {
        if(!_canRecieveInput)
        {
            _inputRecieved = true;
        }
        else
        {
            _inputRecieved = false;
        }
    }
    public void OnMovement(InputAction.CallbackContext value)
    {
        Vector2 inputMovement = value.ReadValue<Vector2>();
        pMovement.Movement = inputMovement;
    }
    public void OnRun(InputAction.CallbackContext value)
    {
        if (value.canceled)
        {
            if(!pMovement.IsRunning)
            {
                return;
            }
            if(_weaponDrawn)
            {
                return;
            }
            pMovement.IsRunning = false;
        }

        if (!value.started)
        {
            return;
        }
        if(!_canRecieveInput)
        {
            return;
        }
        if(_isUsingItem)
        {
            return;
        }
        if(!_canWalk)
        {
            return;
        }
        if (pMovement.Movement.magnitude <= 0.1f)
        {
            return;
        }
        if (!pVariables.UseStamina(0.1f))
        {
            return;
        }
        if (_weaponDrawn)
        {
            if (_aiming)
            {
                return;
            }
            SheathWeapon();
        }
        pMovement.IsRunning = true;
    }
    public void OnRoll(InputAction.CallbackContext value)
    {
        if (!_canRoll)
        {
            return;
        }
        if (!value.started)
        {
            return;
        }
        
        if (pMovement.Movement.magnitude <= 0.1f)
        {
            return;
        }
        if(_aiming)
        {
            return;
        }
        if (!pVariables.UseStamina(25))
        {
            return;
        }
        Debug.Log("I should roll");
        pMovement.CaptureDirection = pMovement.Movement;
        pAnimations.Roll();
        _isRolling = true;
        SetRoll(0);
    }
    public void OnAttack(InputAction.CallbackContext value)
    {
        if (!value.started)
        {
            return;
        }
        if(!_canRecieveInput)
        {
            return;
        }
        if(_isUsingItem)
        {
            return;
        }
        if(_canAim)
        {
            if (_aiming)
            {
                DoAction();
                pAnimations.BowFire();
                return;
            }
            return;
        }
        _canWalk = false;

        if (pMovement.IsRunning)
        {
            DoAction();
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

        if (!_readyforFirstAttack)
        {
            return;
        }

        _readyforFirstAttack = false;
        DrawWeapon();

        switch (pEquipment.WeaponData.WeaponType)
        {
            case WeaponTypeData.WeaponTypes.LightClub:
                pAnimations.LightClubStart();
                pAnimations.LCDrawn(_weaponDrawn);
                break;
            case WeaponTypeData.WeaponTypes.HeavyClub:
                pAnimations.HeavyClubStart();
                pAnimations.HCDrawn(_weaponDrawn);
                break;
            case WeaponTypeData.WeaponTypes.Bow:
                pAnimations.BowStart();
                pAnimations.BowDrawn(_weaponDrawn);
                pEquipment.BowDrawn(_weaponDrawn);
                _canAim = true;
                break;
        }

        if (_canAim)
        {
            return;
        }
    }
    public void OnSpecialAttack(InputAction.CallbackContext value)
    {
        if(_isUsingItem)
        {
            return;
        }
        if(_canAim)
        {
            if (!defCinemachine || !inventoryCinemachine || !aimCinemachine)
            {
                return;
            }
            if (!reticle)
            {
                return;
            }
            if (value.canceled)
            {
                Debug.Log("Not Aiming");
                _aiming = false;
                pAnimations.IsAiming(_aiming);
                defCinemachine.SetActive(true);
                aimCinemachine.SetActive(false);
                pAnimations.StartAimBow(0);
                reticle.SetActive(false);
            }
            if (!value.performed)
            {
                return;
            }
            _canWalk = false;
            pAnimations.StartAimBow(1);
            Debug.Log("Aiming");
            _aiming = true;
            pAnimations.IsAiming(_aiming);
            aimCinemachine.SetActive(true);
            defCinemachine.SetActive(false);
            reticle.SetActive(true);
            return;
        }
        else
        {
            pMovement.IsRunning = false;
            if (value.canceled)
            {
                if(HC_SpecialCharge)
                {
                    DoAction();
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
                    if(!chargingImage)
                    {
                        return;
                    }
                    chargingImage.gameObject.SetActive(false);
                    return;
                }
            }
            if (!value.started)
            {
                return;
            }
            if (_isDoingSpecialAttack)
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
                    if(!chargingImage)
                    {
                        return;
                    }
                    chargingImage.gameObject.SetActive(true);
                    break;
            }
            _canWalk = false;
            DrawWeapon();
            pAnimations.SpecialTrigger();
            _isDoingSpecialAttack = true;
        }

    }

    public void OnUseItem(InputAction.CallbackContext value)
    {
        if(!value.started)
        {
            return;
        }
        if(!pInventory.CanUseItem)
        {
            return;
        }
        if(_isUsingItem)
        {
            return;
        }
        _isUsingItem = true;
        SheathWeapon();
        pAnimations.UseConsumableWeight(1);
    }

    public void OnHideWeapon(InputAction.CallbackContext value)
    {
        if(!value.started)
        {
            return;
        }
        if(!_canRecieveInput)
        {
            return;
        }
        if(!_weaponDrawn)
        {
            return;
        }
        if(_aiming)
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
        if(!_canRecieveInput)
        {
            return;
        }
        if(canChangeWeapon)
        {
            CanvasManager.instance.ShowInfo("You changed your weapon!");
            pEquipment.UpdateSpawnWeapon(_weaponData);
            SheathWeapon();
            return;
        }
        if(canChangeArmor)
        {
            CanvasManager.instance.ShowInfo("You changed your Armor!");
            pEquipment.UpdateSpawnArmor(_armorData);
            return;
        }
        if (!_canGather)
        {
            return;
        }
        if(_weaponDrawn)
        {
            SheathWeapon();
        }
        DoAction();
        pAnimations.Gather();
       
    }
    public void OnReadScroll(InputAction.CallbackContext value)
    {
        if (!value.performed)
        {
            return;
        }
        if (value.ReadValue<Vector2>().y > 0)
        {
            pInventory.ScrollLeft();
        }
        else
        {
            pInventory.ScrollRight();
        }
    }

    #endregion ButtonInputs
    void IsRunning()
    {
        if (pMovement.IsRunning)
        {
            if (pVariables.UseStamina(0.1f))
            {
                pAnimations.Run(true);
                pMovement.RunModifier = 1.5f;
            }
            else
            {
                pMovement.IsRunning = false;
                pAnimations.Run(false);
                pMovement.RunModifier = 1f;
            }
        }
        else
        {
            pAnimations.Run(false);
            pMovement.RunModifier = 1f;
        }
        if (pMovement.Movement.magnitude >= 0.1f)
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
        _inputRecieved = true;
        _canRecieveInput = false;

        _canRoll = false;
    }
    void SetRoll(int checkTrue)
    {
        if(checkTrue <= 0)
        {
            _canRoll = false;
        }
        else
        {
            _canRoll = true;
        }
    }
    void DrawWeapon()
    {
        _weaponDrawn = true;
        pAnimations.WeaponDrawn(_weaponDrawn);
        pEquipment.WeaponDrawn(_weaponDrawn);
    }
    void SheathWeapon()
    {
        _weaponDrawn = false;
        pAnimations.LCDrawn(_weaponDrawn);
        pAnimations.HCDrawn(_weaponDrawn);

        pAnimations.BowDrawn(_weaponDrawn);
        pEquipment.BowDrawn(_weaponDrawn);
        _canAim = false;

        pAnimations.SheathWeight(1);
        pAnimations.WeaponDrawn(_weaponDrawn);
        pEquipment.WeaponDrawn(_weaponDrawn);
    }

    private void OnTriggerEnter(Collider hit)
    {
        if(hit.transform.tag == "Puddle")
        {
            pMovement.SpeedModifer = 0.8f;
        }
        if(hit.transform.gameObject.GetComponent<GatheringSpots>() != null)
        {
            CanvasManager.instance.ShowInfo("Press F to interact");
            _canGather = true;
            _gatheringItem = hit.gameObject;
            
        }
        else if(hit.transform.gameObject.GetComponent<ChangeWeapon>() != null)
        {
            CanvasManager.instance.ShowInfo("Press F to interact");
            canChangeWeapon = true;
            _weaponData = hit.GetComponent<ChangeWeapon>().WeaponData;

        }
        else if (hit.transform.gameObject.GetComponent<ChangeArmor>() != null)
        {
            CanvasManager.instance.ShowInfo("Press F to interact");
            canChangeArmor = true;
            _armorData = hit.GetComponent<ChangeArmor>().ArmorData;

        }
    }
    private void OnTriggerExit(Collider hit)
    {
        if (hit.transform.tag == "Puddle")
        {
            pMovement.SpeedModifer = 1f;
        }
        if (hit.transform.gameObject.GetComponent<GatheringSpots>() != null)
        {
            _canGather = false;
            _gatheringItem = null;
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
