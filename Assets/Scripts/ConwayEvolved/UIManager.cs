using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    [SerializeField] private CellGrid _cellGrid;

    //UI elements
    public Slider reloadSlider;

    // Singleton
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void RestartLevel()
    {
        _cellGrid.InitializeGrid();
    }
}
