using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

public class UIInput : Singleton<UIInput>
{
    [SerializeField] PlayerInput playerInput;
    InputSystemUIInputModule UIInputModule;

    protected override void Awake()
    {
        base.Awake();
        ButtonPressedBehaviour.buttonFunctionTable = new Dictionary<string, System.Action>();
        UIInputModule = GetComponent<InputSystemUIInputModule>();
        UIInputModule.enabled = false;
    }

    public void SelectUI(Selectable UIObject)
    {
        UIObject.Select();
        UIObject.OnSelect(null);
        UIInputModule.enabled = true;
    }

    public void DisableAllUIInputs()
    {
        playerInput.DisableAllInputs();
        UIInputModule.enabled = false;
    }
}
