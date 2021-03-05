using System;
using UnityEngine;
using UnityEngine.UI;
using VPEAR.Core.Wrappers;

public class UserDetailScript : AbstractView
{
    [SerializeField] private Text userIdText;
    [SerializeField] private Text userNameText;
    [SerializeField] private Text userRolesText;
    [SerializeField] private Toggle userIsVerifiedToggle;
    [SerializeField] private InputField oldPasswordInput;
    [SerializeField] private InputField newPasswordInput;
    [SerializeField] private Button saveButton;

    private void Start()
    {
    }
}
