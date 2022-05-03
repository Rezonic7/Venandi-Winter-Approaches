//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.3.0
//     from Assets/InputAction/PlayerInputs.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public partial class @PlayerInputs : IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @PlayerInputs()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerInputs"",
    ""maps"": [
        {
            ""name"": ""Player3DMovement"",
            ""id"": ""c0e71480-64dc-48f4-8f6a-9e455c868b06"",
            ""actions"": [
                {
                    ""name"": ""Walk"",
                    ""type"": ""PassThrough"",
                    ""id"": ""5e99d89a-5085-4659-974b-e5b51726af8d"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Toggle Inventory"",
                    ""type"": ""Button"",
                    ""id"": ""370448a1-a35a-4e17-9814-c309b469f1ce"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Roll"",
                    ""type"": ""Button"",
                    ""id"": ""106016ac-dc2f-4e84-aea4-2945bc93ce18"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press"",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Run"",
                    ""type"": ""Button"",
                    ""id"": ""e61b9224-a47d-4690-a6c5-64f714490844"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press"",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""MeleeAttack"",
                    ""type"": ""Button"",
                    ""id"": ""06ed648e-1bf0-43f1-8812-0c3d83160afa"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""LookAround"",
                    ""type"": ""PassThrough"",
                    ""id"": ""541291cc-efb3-408f-a281-b4875b4ca053"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Interact"",
                    ""type"": ""Button"",
                    ""id"": ""1ff5650e-9737-47b5-9ab2-86c66ec28d9b"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Hide Weapon"",
                    ""type"": ""Button"",
                    ""id"": ""b6b7b0ab-eb0c-4701-822c-12b9f9171958"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Special"",
                    ""type"": ""Button"",
                    ""id"": ""8a0340a9-c3e6-4e3e-b941-dcb93b446ae7"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Hold"",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""44838af0-8f57-4105-9e12-91da02c2effe"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Roll"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""ce844b64-be71-4643-a948-41a2e7100b12"",
                    ""path"": ""<Keyboard>/leftShift"",
                    ""interactions"": ""Hold"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Run"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""7a9c093a-c72e-47e1-84f8-84c449627a08"",
                    ""path"": ""<Mouse>/delta"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""LookAround"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""WASD"",
                    ""id"": ""a2dc080d-ab79-4146-82aa-b3edc2332d7b"",
                    ""path"": ""2DVector(mode=2)"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Walk"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""Up"",
                    ""id"": ""88f6376b-a01e-4890-b645-2fe5d3c4685c"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Walk"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Down"",
                    ""id"": ""b4de2668-5f51-49d8-bff6-c8d74668aab8"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Walk"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Left"",
                    ""id"": ""0118377a-0bca-4487-8ad5-994358a19383"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Walk"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Right"",
                    ""id"": ""7b090ad1-599c-40dd-a303-164cee28e8ff"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Walk"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""93c98d7f-d467-495d-ad85-fb3b51c6adfc"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MeleeAttack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""544b22c8-3531-47d4-b0c7-1169299fd923"",
                    ""path"": ""<Keyboard>/f"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Interact"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""68856cf7-1a83-4fd8-80a5-84b27c2acdcc"",
                    ""path"": ""<Keyboard>/t"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Hide Weapon"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""8ffa5dd2-2a9c-463f-9616-748929a17611"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Special"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""8ca500b9-e95e-49d2-8508-f34b1020447d"",
                    ""path"": ""<Keyboard>/tab"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Toggle Inventory"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""Inventory"",
            ""id"": ""c81f70ad-c81c-430f-b7b8-36938ab9f3a4"",
            ""actions"": [
                {
                    ""name"": ""Select"",
                    ""type"": ""Button"",
                    ""id"": ""f17cdc52-f375-4c6a-b21c-6c873f9ebd5e"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""MousePosition"",
                    ""type"": ""Value"",
                    ""id"": ""1249b58c-49ce-4ecf-b506-4172774e9e19"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Toggle Inventory"",
                    ""type"": ""Button"",
                    ""id"": ""50b55982-7071-40fb-b286-044d601b29fb"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""d3b8b589-2be3-4324-9e2a-efa7e53a415e"",
                    ""path"": ""<Keyboard>/tab"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Toggle Inventory"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c3ba42e1-04c0-4f9a-b0ae-12232fc0e393"",
                    ""path"": ""<Mouse>/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MousePosition"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c675a102-86bc-4e3a-8853-a45672df247e"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Select"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Player3DMovement
        m_Player3DMovement = asset.FindActionMap("Player3DMovement", throwIfNotFound: true);
        m_Player3DMovement_Walk = m_Player3DMovement.FindAction("Walk", throwIfNotFound: true);
        m_Player3DMovement_ToggleInventory = m_Player3DMovement.FindAction("Toggle Inventory", throwIfNotFound: true);
        m_Player3DMovement_Roll = m_Player3DMovement.FindAction("Roll", throwIfNotFound: true);
        m_Player3DMovement_Run = m_Player3DMovement.FindAction("Run", throwIfNotFound: true);
        m_Player3DMovement_MeleeAttack = m_Player3DMovement.FindAction("MeleeAttack", throwIfNotFound: true);
        m_Player3DMovement_LookAround = m_Player3DMovement.FindAction("LookAround", throwIfNotFound: true);
        m_Player3DMovement_Interact = m_Player3DMovement.FindAction("Interact", throwIfNotFound: true);
        m_Player3DMovement_HideWeapon = m_Player3DMovement.FindAction("Hide Weapon", throwIfNotFound: true);
        m_Player3DMovement_Special = m_Player3DMovement.FindAction("Special", throwIfNotFound: true);
        // Inventory
        m_Inventory = asset.FindActionMap("Inventory", throwIfNotFound: true);
        m_Inventory_Select = m_Inventory.FindAction("Select", throwIfNotFound: true);
        m_Inventory_MousePosition = m_Inventory.FindAction("MousePosition", throwIfNotFound: true);
        m_Inventory_ToggleInventory = m_Inventory.FindAction("Toggle Inventory", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }
    public IEnumerable<InputBinding> bindings => asset.bindings;

    public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
    {
        return asset.FindAction(actionNameOrId, throwIfNotFound);
    }
    public int FindBinding(InputBinding bindingMask, out InputAction action)
    {
        return asset.FindBinding(bindingMask, out action);
    }

    // Player3DMovement
    private readonly InputActionMap m_Player3DMovement;
    private IPlayer3DMovementActions m_Player3DMovementActionsCallbackInterface;
    private readonly InputAction m_Player3DMovement_Walk;
    private readonly InputAction m_Player3DMovement_ToggleInventory;
    private readonly InputAction m_Player3DMovement_Roll;
    private readonly InputAction m_Player3DMovement_Run;
    private readonly InputAction m_Player3DMovement_MeleeAttack;
    private readonly InputAction m_Player3DMovement_LookAround;
    private readonly InputAction m_Player3DMovement_Interact;
    private readonly InputAction m_Player3DMovement_HideWeapon;
    private readonly InputAction m_Player3DMovement_Special;
    public struct Player3DMovementActions
    {
        private @PlayerInputs m_Wrapper;
        public Player3DMovementActions(@PlayerInputs wrapper) { m_Wrapper = wrapper; }
        public InputAction @Walk => m_Wrapper.m_Player3DMovement_Walk;
        public InputAction @ToggleInventory => m_Wrapper.m_Player3DMovement_ToggleInventory;
        public InputAction @Roll => m_Wrapper.m_Player3DMovement_Roll;
        public InputAction @Run => m_Wrapper.m_Player3DMovement_Run;
        public InputAction @MeleeAttack => m_Wrapper.m_Player3DMovement_MeleeAttack;
        public InputAction @LookAround => m_Wrapper.m_Player3DMovement_LookAround;
        public InputAction @Interact => m_Wrapper.m_Player3DMovement_Interact;
        public InputAction @HideWeapon => m_Wrapper.m_Player3DMovement_HideWeapon;
        public InputAction @Special => m_Wrapper.m_Player3DMovement_Special;
        public InputActionMap Get() { return m_Wrapper.m_Player3DMovement; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(Player3DMovementActions set) { return set.Get(); }
        public void SetCallbacks(IPlayer3DMovementActions instance)
        {
            if (m_Wrapper.m_Player3DMovementActionsCallbackInterface != null)
            {
                @Walk.started -= m_Wrapper.m_Player3DMovementActionsCallbackInterface.OnWalk;
                @Walk.performed -= m_Wrapper.m_Player3DMovementActionsCallbackInterface.OnWalk;
                @Walk.canceled -= m_Wrapper.m_Player3DMovementActionsCallbackInterface.OnWalk;
                @ToggleInventory.started -= m_Wrapper.m_Player3DMovementActionsCallbackInterface.OnToggleInventory;
                @ToggleInventory.performed -= m_Wrapper.m_Player3DMovementActionsCallbackInterface.OnToggleInventory;
                @ToggleInventory.canceled -= m_Wrapper.m_Player3DMovementActionsCallbackInterface.OnToggleInventory;
                @Roll.started -= m_Wrapper.m_Player3DMovementActionsCallbackInterface.OnRoll;
                @Roll.performed -= m_Wrapper.m_Player3DMovementActionsCallbackInterface.OnRoll;
                @Roll.canceled -= m_Wrapper.m_Player3DMovementActionsCallbackInterface.OnRoll;
                @Run.started -= m_Wrapper.m_Player3DMovementActionsCallbackInterface.OnRun;
                @Run.performed -= m_Wrapper.m_Player3DMovementActionsCallbackInterface.OnRun;
                @Run.canceled -= m_Wrapper.m_Player3DMovementActionsCallbackInterface.OnRun;
                @MeleeAttack.started -= m_Wrapper.m_Player3DMovementActionsCallbackInterface.OnMeleeAttack;
                @MeleeAttack.performed -= m_Wrapper.m_Player3DMovementActionsCallbackInterface.OnMeleeAttack;
                @MeleeAttack.canceled -= m_Wrapper.m_Player3DMovementActionsCallbackInterface.OnMeleeAttack;
                @LookAround.started -= m_Wrapper.m_Player3DMovementActionsCallbackInterface.OnLookAround;
                @LookAround.performed -= m_Wrapper.m_Player3DMovementActionsCallbackInterface.OnLookAround;
                @LookAround.canceled -= m_Wrapper.m_Player3DMovementActionsCallbackInterface.OnLookAround;
                @Interact.started -= m_Wrapper.m_Player3DMovementActionsCallbackInterface.OnInteract;
                @Interact.performed -= m_Wrapper.m_Player3DMovementActionsCallbackInterface.OnInteract;
                @Interact.canceled -= m_Wrapper.m_Player3DMovementActionsCallbackInterface.OnInteract;
                @HideWeapon.started -= m_Wrapper.m_Player3DMovementActionsCallbackInterface.OnHideWeapon;
                @HideWeapon.performed -= m_Wrapper.m_Player3DMovementActionsCallbackInterface.OnHideWeapon;
                @HideWeapon.canceled -= m_Wrapper.m_Player3DMovementActionsCallbackInterface.OnHideWeapon;
                @Special.started -= m_Wrapper.m_Player3DMovementActionsCallbackInterface.OnSpecial;
                @Special.performed -= m_Wrapper.m_Player3DMovementActionsCallbackInterface.OnSpecial;
                @Special.canceled -= m_Wrapper.m_Player3DMovementActionsCallbackInterface.OnSpecial;
            }
            m_Wrapper.m_Player3DMovementActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Walk.started += instance.OnWalk;
                @Walk.performed += instance.OnWalk;
                @Walk.canceled += instance.OnWalk;
                @ToggleInventory.started += instance.OnToggleInventory;
                @ToggleInventory.performed += instance.OnToggleInventory;
                @ToggleInventory.canceled += instance.OnToggleInventory;
                @Roll.started += instance.OnRoll;
                @Roll.performed += instance.OnRoll;
                @Roll.canceled += instance.OnRoll;
                @Run.started += instance.OnRun;
                @Run.performed += instance.OnRun;
                @Run.canceled += instance.OnRun;
                @MeleeAttack.started += instance.OnMeleeAttack;
                @MeleeAttack.performed += instance.OnMeleeAttack;
                @MeleeAttack.canceled += instance.OnMeleeAttack;
                @LookAround.started += instance.OnLookAround;
                @LookAround.performed += instance.OnLookAround;
                @LookAround.canceled += instance.OnLookAround;
                @Interact.started += instance.OnInteract;
                @Interact.performed += instance.OnInteract;
                @Interact.canceled += instance.OnInteract;
                @HideWeapon.started += instance.OnHideWeapon;
                @HideWeapon.performed += instance.OnHideWeapon;
                @HideWeapon.canceled += instance.OnHideWeapon;
                @Special.started += instance.OnSpecial;
                @Special.performed += instance.OnSpecial;
                @Special.canceled += instance.OnSpecial;
            }
        }
    }
    public Player3DMovementActions @Player3DMovement => new Player3DMovementActions(this);

    // Inventory
    private readonly InputActionMap m_Inventory;
    private IInventoryActions m_InventoryActionsCallbackInterface;
    private readonly InputAction m_Inventory_Select;
    private readonly InputAction m_Inventory_MousePosition;
    private readonly InputAction m_Inventory_ToggleInventory;
    public struct InventoryActions
    {
        private @PlayerInputs m_Wrapper;
        public InventoryActions(@PlayerInputs wrapper) { m_Wrapper = wrapper; }
        public InputAction @Select => m_Wrapper.m_Inventory_Select;
        public InputAction @MousePosition => m_Wrapper.m_Inventory_MousePosition;
        public InputAction @ToggleInventory => m_Wrapper.m_Inventory_ToggleInventory;
        public InputActionMap Get() { return m_Wrapper.m_Inventory; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(InventoryActions set) { return set.Get(); }
        public void SetCallbacks(IInventoryActions instance)
        {
            if (m_Wrapper.m_InventoryActionsCallbackInterface != null)
            {
                @Select.started -= m_Wrapper.m_InventoryActionsCallbackInterface.OnSelect;
                @Select.performed -= m_Wrapper.m_InventoryActionsCallbackInterface.OnSelect;
                @Select.canceled -= m_Wrapper.m_InventoryActionsCallbackInterface.OnSelect;
                @MousePosition.started -= m_Wrapper.m_InventoryActionsCallbackInterface.OnMousePosition;
                @MousePosition.performed -= m_Wrapper.m_InventoryActionsCallbackInterface.OnMousePosition;
                @MousePosition.canceled -= m_Wrapper.m_InventoryActionsCallbackInterface.OnMousePosition;
                @ToggleInventory.started -= m_Wrapper.m_InventoryActionsCallbackInterface.OnToggleInventory;
                @ToggleInventory.performed -= m_Wrapper.m_InventoryActionsCallbackInterface.OnToggleInventory;
                @ToggleInventory.canceled -= m_Wrapper.m_InventoryActionsCallbackInterface.OnToggleInventory;
            }
            m_Wrapper.m_InventoryActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Select.started += instance.OnSelect;
                @Select.performed += instance.OnSelect;
                @Select.canceled += instance.OnSelect;
                @MousePosition.started += instance.OnMousePosition;
                @MousePosition.performed += instance.OnMousePosition;
                @MousePosition.canceled += instance.OnMousePosition;
                @ToggleInventory.started += instance.OnToggleInventory;
                @ToggleInventory.performed += instance.OnToggleInventory;
                @ToggleInventory.canceled += instance.OnToggleInventory;
            }
        }
    }
    public InventoryActions @Inventory => new InventoryActions(this);
    public interface IPlayer3DMovementActions
    {
        void OnWalk(InputAction.CallbackContext context);
        void OnToggleInventory(InputAction.CallbackContext context);
        void OnRoll(InputAction.CallbackContext context);
        void OnRun(InputAction.CallbackContext context);
        void OnMeleeAttack(InputAction.CallbackContext context);
        void OnLookAround(InputAction.CallbackContext context);
        void OnInteract(InputAction.CallbackContext context);
        void OnHideWeapon(InputAction.CallbackContext context);
        void OnSpecial(InputAction.CallbackContext context);
    }
    public interface IInventoryActions
    {
        void OnSelect(InputAction.CallbackContext context);
        void OnMousePosition(InputAction.CallbackContext context);
        void OnToggleInventory(InputAction.CallbackContext context);
    }
}
