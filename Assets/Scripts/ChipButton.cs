using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ChipButton : MonoBehaviour
{
    [SerializeField] private UnityEvent _onMouseDownEvent;

    public Material buttonMaterial;
    public Material hoverButtonMaterial;
    public Material disabledButtonMaterial;

    public bool isDisabled;
    

    private MeshRenderer _renderer;

    private void Start()
    {
        _renderer = GetComponent<MeshRenderer>();
    }

    public void SetIsDisabled(bool value)
    {
        isDisabled = value;

        _renderer.material = value ? disabledButtonMaterial : buttonMaterial;
    }

    private void OnMouseEnter()
    {
        if (isDisabled) return;
        _renderer.material = hoverButtonMaterial;
    }

    private void OnMouseExit()
    {
        if (isDisabled) return;
        _renderer.material = buttonMaterial;
    }

    private void OnMouseDown()
    {
        if (isDisabled) return;

        _onMouseDownEvent.Invoke();
    } 
}
