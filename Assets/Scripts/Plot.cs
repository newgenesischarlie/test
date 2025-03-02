using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plot : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private Color hoverColor;

    private GameObject tower;
    private Color startColor;

    private void Start()
    {
        startColor = sr.color;
    }

    private void OnMouseEnter()
    {
        sr.color = hoverColor;
    }

    private void OnMouseExit()
    {
        sr.color = startColor;
    }

    private void OnMouseDown()
    {
        if (tower != null) return; // Prevent building multiple towers in the same plot.

        Debug.Log("Build tower here:" + name);

        // Get the selected tower from the BuildManager
        GameObject towerToBuild = BuildManager.main.GetSelectedTower();

        // Instantiate the selected tower at this plot's position with no rotation
        tower = Instantiate(towerToBuild, transform.position, Quaternion.identity);
    }
}
