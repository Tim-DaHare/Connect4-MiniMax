using UnityEngine;
using UnityEngine.Events;

public class ChipButton : MonoBehaviour
{
    [SerializeField] private UnityEvent _onMouseDownEvent;

    private void OnMouseDown() => _onMouseDownEvent.Invoke();
}
