using NUnit.Framework;
using System;
using System.Collections.Generic;
using VPEAR.Core.Wrappers;

public class ReducersTest
{
    [Test]
    public void ReduceApplaySettingsActionTest()
    {
        var currentState = TestHelpers.ARState();
        var action = new ApplySettingsAction(10, 80, 120f, ColorScale.Plasma);
        var newState = Reducers.ReduceApplySettingsAction(currentState, action);

        Assert.AreEqual(10, newState.StepSize);
        Assert.AreEqual(80, newState.Threshold);
        Assert.AreEqual(TimeSpan.FromMinutes(120f), newState.DeltaMinutes);
        Assert.AreEqual(ColorScale.Plasma, newState.ColorScale);
    }

    [Test]
    public void ReduceClosePopupActionTest()
    {
        var currentState = TestHelpers.PopupState();
        var action = new ClosePopupAction();
        var newState = Reducers.ReduceClosePopupAction(currentState, action);

        Assert.False(newState.IsVisible);
    }

    [Test]
    public void ReduceShowPopupActionTest()
    {
        var currentState = TestHelpers.PopupState();
        var action = new ShowPopupAction("title", "message");
        var newState = Reducers.ReduceShowPopupAction(currentState, action);

        Assert.True(newState.IsVisible);
        Assert.AreEqual("title", newState.Title);
        Assert.AreEqual("message", newState.Message);
    }

    [Test]
    public void ReduceFetchingDeviceActionTest()
    {
        var currentState = TestHelpers.DeviceDetailState();
        var action = new FetchingDeviceAction(new GetDeviceResponse());
        var newState = Reducers.ReduceFetchingDeviceAction(currentState, action);

        Assert.True(newState.IsLoading);
    }

    [Test]
    public void ReduceFetchedDeviceActionTest()
    {
        var currentState = TestHelpers.DeviceDetailState();
        var action = new FetchedDeviceAction(new GetDeviceResponse(), new GetFiltersResponse());
        var newState = Reducers.ReduceFetchedDeviceAction(currentState, action);

        Assert.False(newState.IsLoading);
        Assert.NotNull(newState.Device);
        Assert.NotNull(newState.Filters);
    }

    [Test]
    public void ReduceFetchingDevicesActionTest()
    {
        var currentState = TestHelpers.DeviceListState();
        var action = new FetchingDevicesAction(default);
        var newState = Reducers.ReduceFetchingDevicesAction(currentState, action);

        Assert.True(newState.IsLoading);
    }

    [Test]
    public void ReduceFetchedDevicesActionTest()
    {
        var currentState = TestHelpers.DeviceListState();
        var action = new FetchedDevicesAction(default, new List<GetDeviceResponse>());
        var newState = Reducers.ReduceFetchedDevicesAction(currentState, action);

        Assert.False(newState.IsLoading);
        Assert.Null(newState.Status);
        Assert.NotNull(newState.Devices);
    }

    [Test]
    public void ReduceFetchingUsersActionTest()
    {
        var currentState = TestHelpers.UserListState();
        var action = new FetchingUsersAction(default);
        var newState = Reducers.ReduceFetchingUsersAction(currentState, action);

        Assert.True(newState.IsLoading);
    }

    [Test]
    public void ReduceFetchedUsersActionTest()
    {
        var currentState = TestHelpers.UserListState();
        var action = new FetchedUsersAction(new List<GetUserResponse>(), default);
        var newState = Reducers.ReduceFetchedUsersAction(currentState, action);

        Assert.False(newState.IsLoading);
        Assert.Null(newState.Role);
        Assert.NotNull(newState.Users);
    }

    [Test]
    public void ReduceFetchingFramesActionTest()
    {
        var currentState = TestHelpers.ARState();
        var action = new FetchingFramesAction(new GetDeviceResponse());
        var newState = Reducers.ReduceFetchingFramesAction(currentState, action);

        Assert.True(newState.IsLoading);
    }

    [Test]
    public void ReduceFetchedFramesActionTest()
    {
        var currentState = TestHelpers.ARState();
        var action = new FetchedFramesAction(new List<GetFrameResponse>(), new List<GetSensorResponse>());
        var newState = Reducers.ReduceFetchedFramesaction(currentState, action);

        Assert.False(newState.IsLoading);
        Assert.NotNull(newState.History);
        Assert.NotNull(newState.Sensors);
    }

    [Test]
    public void ReduceLoginActionTest()
    {
        var currentState = TestHelpers.LoginState();
        var action = new LoginAction("name", "password");
        var newState = Reducers.ReduceLoginAction(currentState, action);

        Assert.False(newState.IsSignedIn);
        Assert.AreEqual("name", newState.Name);
    }

    [Test]
    public void ReduceLoginSucceededActionTest()
    {
        var currentState = TestHelpers.LoginState();
        var action = new LoginSucceededAction(true);
        var newState = Reducers.ReduceLoginSucceededAction(currentState, action);

        Assert.True(newState.IsSignedIn);
        Assert.True(newState.IsAdmin);
    }

    [Test]
    public void ReduceLogoutActionTest()
    {
        var currentState = TestHelpers.LoginState();
        var action = new LogoutAction();
        var newState = Reducers.ReduceLogoutActin(currentState, action);

        Assert.False(newState.IsSignedIn);
        Assert.False(newState.IsAdmin);
        Assert.IsEmpty(newState.Name);
    }

    [Test]
    public void ReduceVerifingUserActionTest()
    {
        var currentState = TestHelpers.UserDetailState();
        var action = new VerifingUserAction(new GetUserResponse());
        var newState = Reducers.ReduceVerfingUserAction(currentState, action);

        Assert.True(newState.IsLoading);
    }

    [Test]
    public void ReduceVerfiedUserActionTest()
    {
        var currentState = TestHelpers.UserDetailState();
        var action = new VerifiedUserAction(new GetUserResponse());
        var newState = Reducers.ReduceVerfiedUserAction(currentState, action);

        Assert.False(newState.IsLoading);
    }

    [Test]
    public void ReduceNavigateToActionTest()
    {
        var currentState = TestHelpers.NavigationState();
        var action = new NavigateToAction("nextView");
        var newState = Reducers.ReduceNavigateTo(currentState, action);

        Assert.AreEqual("nextView", newState.ViewName);
    }

    [Test]
    public void ReduceUpdatingDeviceActionTest()
    {
        var currentState = TestHelpers.DeviceDetailState();
        var action = new UpdatingDeviceAction(default, default, default, default, default, default, default, default);
        var newState = Reducers.ReduceUpdatingDeviceAction(currentState, action);

        Assert.True(newState.IsLoading);
    }

    [Test]
    public void ReduceUpdatedDeviceActionTest()
    {
        var currentState = TestHelpers.DeviceDetailState();
        var action = new UpdatedDeviceAction(new GetDeviceResponse(), new GetFiltersResponse());
        var newState = Reducers.ReduceFetchedDeviceAction(currentState, action);

        Assert.False(newState.IsLoading);
        Assert.NotNull(newState.Device);
        Assert.NotNull(newState.Filters);
    }

    [Test]
    public void ReduceSelectDeviceActionTest()
    {
        var currentState = TestHelpers.DeviceDetailState();
        var action = new SelectDeviceAction(new GetDeviceResponse());
        var newState = Reducers.ReduceSelectDeviceAction(currentState, action);

        Assert.NotNull(newState.Device);
    }

    [Test]
    public void ReduceSelectUserActionTest()
    {
        var currentState = TestHelpers.UserDetailState();
        var action = new SelectUserAction(new GetUserResponse());
        var newState = Reducers.ReduceSelectUserAction(currentState, action);

        Assert.NotNull(newState.User);
    }
}
