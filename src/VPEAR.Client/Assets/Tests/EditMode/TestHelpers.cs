public static class TestHelpers
{
    public static ARState ARState() => new ARState(default, default, default, default, default, default, default, default);

    public static DeviceListState DeviceListState() => new DeviceListState(default, default, default);

    public static DeviceDetailState DeviceDetailState() => new DeviceDetailState(default, default, default);

    public static LoginState LoginState() => new LoginState(default, default, default);

    public static NavigationState NavigationState() => new NavigationState(default);

    public static PopupState PopupState() => new PopupState(default, default, default, default);

    public static UserListState UserListState() => new UserListState(default, default, default);

    public static UserDetailState UserDetailState() => new UserDetailState(default, default);
}
