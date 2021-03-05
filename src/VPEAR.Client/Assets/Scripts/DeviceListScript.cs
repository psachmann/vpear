using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using VPEAR.Core;
using VPEAR.Core.Wrappers;

public class DeviceListScript : AbstractView
{
    [SerializeField] private GameObject content;
    [SerializeField] private Button addButton;
    [SerializeField] private Button itemTemplate;

    private void Start()
    {
        // this.itemTemplate.gameObject.SetActive(false);
    }
}
