// GENERATED AUTOMATICALLY FROM 'Assets/Scripts/InputController.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @InputController : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @InputController()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""InputController"",
    ""maps"": [
        {
            ""name"": ""Player"",
            ""id"": ""520219da-0119-4dcf-87af-966721e6b8b3"",
            ""actions"": [
                {
                    ""name"": ""TurnLeft"",
                    ""type"": ""PassThrough"",
                    ""id"": ""de0d35c2-8fb9-420b-aa90-374ee98038fd"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""TurnRIght"",
                    ""type"": ""PassThrough"",
                    ""id"": ""9d304913-3001-4d4e-94b1-43ba4eed873f"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""f3931ae5-9198-4c59-b7c6-cac9a3c9fd60"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""TurnLeft"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""dbd6b7df-2ecf-4517-a6cc-ed384c267cc6"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""TurnRIght"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Player
        m_Player = asset.FindActionMap("Player", throwIfNotFound: true);
        m_Player_TurnLeft = m_Player.FindAction("TurnLeft", throwIfNotFound: true);
        m_Player_TurnRIght = m_Player.FindAction("TurnRIght", throwIfNotFound: true);
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

    // Player
    private readonly InputActionMap m_Player;
    private IPlayerActions m_PlayerActionsCallbackInterface;
    private readonly InputAction m_Player_TurnLeft;
    private readonly InputAction m_Player_TurnRIght;
    public struct PlayerActions
    {
        private @InputController m_Wrapper;
        public PlayerActions(@InputController wrapper) { m_Wrapper = wrapper; }
        public InputAction @TurnLeft => m_Wrapper.m_Player_TurnLeft;
        public InputAction @TurnRIght => m_Wrapper.m_Player_TurnRIght;
        public InputActionMap Get() { return m_Wrapper.m_Player; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayerActions set) { return set.Get(); }
        public void SetCallbacks(IPlayerActions instance)
        {
            if (m_Wrapper.m_PlayerActionsCallbackInterface != null)
            {
                @TurnLeft.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnTurnLeft;
                @TurnLeft.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnTurnLeft;
                @TurnLeft.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnTurnLeft;
                @TurnRIght.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnTurnRIght;
                @TurnRIght.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnTurnRIght;
                @TurnRIght.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnTurnRIght;
            }
            m_Wrapper.m_PlayerActionsCallbackInterface = instance;
            if (instance != null)
            {
                @TurnLeft.started += instance.OnTurnLeft;
                @TurnLeft.performed += instance.OnTurnLeft;
                @TurnLeft.canceled += instance.OnTurnLeft;
                @TurnRIght.started += instance.OnTurnRIght;
                @TurnRIght.performed += instance.OnTurnRIght;
                @TurnRIght.canceled += instance.OnTurnRIght;
            }
        }
    }
    public PlayerActions @Player => new PlayerActions(this);
    public interface IPlayerActions
    {
        void OnTurnLeft(InputAction.CallbackContext context);
        void OnTurnRIght(InputAction.CallbackContext context);
    }
}
