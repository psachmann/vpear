using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using VPEAR.Core;
using VPEAR.Core.Wrappers;

public class DeviceListScript : AbstractView
{
    [SerializeField] private GameObject _content;
    [SerializeField] private Button _addButton;
    [SerializeField] private Button _itemTemplate;

    private void Start()
    {
        _addButton.onClick.AddListener(() => _dispatcher.Dispatch(new NavigateToAction(Constants.DeviceSearchViewName)));
        // this.itemTemplate.gameObject.SetActive(false);
    }
}
