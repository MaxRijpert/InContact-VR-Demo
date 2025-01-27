using UnityEngine;
using UnityEngine.InputSystem; // Required for Input System (optional if you're using classic Input)

public class CarSwitcher : MonoBehaviour
{
    // Assign your car models in the Inspector
    public GameObject TeslaModel3;
    public GameObject TeslaModelS;

    // Assign an Input Action for toggling cars
    public InputAction toggleCarAction;

    private bool isModel3Active = true;

    private void Start()
    {
        // Ensure the correct model is active at the start
        SetActiveCar(true);

        // Enable the toggle action
        toggleCarAction.Enable();
        toggleCarAction.performed += ToggleCar;
    }

    private void OnDestroy()
    {
        // Unsubscribe to avoid memory leaks
        toggleCarAction.performed -= ToggleCar;
    }

    private void ToggleCar(InputAction.CallbackContext context)
    {
        // Switch between the two cars
        isModel3Active = !isModel3Active;
        SetActiveCar(isModel3Active);
    }

    private void SetActiveCar(bool activateModel3)
    {
        if (TeslaModel3 != null) TeslaModel3.SetActive(activateModel3);
        if (TeslaModelS != null) TeslaModelS.SetActive(!activateModel3);
    }
}
