using System;
using UnityEngine;

public class InputController : MonoBehaviour
{
    public Action<Vector3> OnPointerClick;
    private void OnMouseDown()
    {
        var worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        worldPosition.z = 0;
        OnPointerClick?.Invoke(worldPosition);
    }
}
