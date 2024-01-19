using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuPanelScript : MonoBehaviour
{
    public Button button = null;

    private void OnEnable()
    {
        button.Select();
    }
}
