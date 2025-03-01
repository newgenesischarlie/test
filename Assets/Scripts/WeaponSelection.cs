using UnityEngine;

public class WeaponSelection : MonoBehaviour
{
    public int SelectedWeaponIndex { get; set; }
    
    public void SetCurrentPlot(MonoBehaviour plot)
    {
        // Implement the method that Plot.cs is trying to call
        if (plot != null)
        {
            // Add any necessary logic here
            Debug.Log($"Setting current plot: {plot.name}");
        }
    }
}