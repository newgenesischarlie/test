using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    // Singleton instance of UIManager
    public static UIManager Instance { get; private set; }

    // The panels to manage
    [SerializeField] private GameObject turretShopPanel;  // Example panel for the turret shop

    // Awake is used to set up the singleton pattern
    private void Awake()
    {
        // Ensure there's only one instance of UIManager
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);  // If there's already an instance, destroy the duplicate
        }
        else
        {
            Instance = this;  // Set this instance as the singleton
            DontDestroyOnLoad(gameObject);  // Optional: Keep the UIManager across scene loads
        }
    }

    // Method to open the turret shop panel
    public void OpenTurretShopPanel()
    {
        if (turretShopPanel != null)
        {
            turretShopPanel.SetActive(true);
        }
    }

    // Method to close the turret shop panel
    public void CloseTurretShopPanel()
    {
        if (turretShopPanel != null)
        {
            turretShopPanel.SetActive(false);
        }
    }

    // Additional UI-related methods can be added here, for example, to open other panels
    public void OpenPauseMenu()
    {
        // Logic for opening the pause menu panel
    }

    public void ClosePauseMenu()
    {
        // Logic for closing the pause menu panel
    }
}
