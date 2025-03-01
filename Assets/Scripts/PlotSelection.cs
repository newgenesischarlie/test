using UnityEngine;
using UnityEngine.UI;

public class PlotSelection : MonoBehaviour
{
    private PlotScript currentPlot;  // The plot we are currently selecting a weapon for

    // References to the weapon buttons in the UI
    [SerializeField] private Button weapon1Button;
    [SerializeField] private Button weapon2Button;
    [SerializeField] private Button weapon3Button;

    [SerializeField] private GameObject weapon1Prefab;  // Weapon prefabs
    [SerializeField] private GameObject weapon2Prefab;
    [SerializeField] private GameObject weapon3Prefab;

    private void Start()
    {
        // Add listeners for button clicks
        weapon1Button.onClick.AddListener(() => PlaceWeapon(weapon1Prefab));
        weapon2Button.onClick.AddListener(() => PlaceWeapon(weapon2Prefab));
        weapon3Button.onClick.AddListener(() => PlaceWeapon(weapon3Prefab));
    }

    // Set the current plot to which the weapon will be placed
    public void SetCurrentPlot(PlotScript plot)
    {
        currentPlot = plot;
    }

    // Place the selected weapon on the plot
    private void PlaceWeapon(GameObject weaponPrefab)
    {
        if (currentPlot != null)
        {
            currentPlot.PlaceWeapon(weaponPrefab);  // Call PlaceWeapon on the plot
        }
    }
}
