using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuPanelScript : MonoBehaviour
{
    [Tooltip("The button that will be selected when the menu is open")]
    [SerializeField]
    private Button buttonToBeSelected = null;

    private void OnEnable()
    {
        //Select the button when the menu opens
        buttonToBeSelected.Select();
    }
}
