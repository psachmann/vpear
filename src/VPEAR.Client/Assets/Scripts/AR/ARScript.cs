using Fluxor;
using Microsoft.Extensions.DependencyInjection;
using UnityEngine;
using UnityEngine.UI;

public class ARScript : AbstractBase
{
    [SerializeField] private Button _backButton;
    [SerializeField ]private MeshRenderer _meshRenderer;

    private IState<ARState> _arState;
    private IState<SettingsState> _settingsState;
    private GridMesh _grid;

    private void Start()
    {
        _arState = s_provider.GetRequiredService<IState<ARState>>();
        _arState.StateChanged += ARStateChanged;
        // _backButton.onClick.AddListener(OnBackClick);
        _grid = new GridMesh(4, 4, 0, 100, 2f, new Vector3(0, 0));

        ARStateChanged(this, _arState.Value);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var position = GetMouseWorldPosition();
            Debug.Log($"left click: x={position.x} y={position.y}");
            Debug.Log($"left click raw: x={Input.mousePosition.x} y={Input.mousePosition.y}");
            _grid[position] += 10;
        }

        if (Input.GetMouseButtonDown(1))
        {
            var position = GetMouseWorldPosition();
            Debug.Log($"right click: x={position.x} y={position.y}");
            _grid[position] -= 10;
        }
    }

    private static Vector3 GetMouseWorldPosition()
    {
        var vector = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        return vector;
    }

    private void ARStateChanged(object sender, ARState state)
    {
    }

    private void OnBackClick()
    {
        _dispatcher.Dispatch(new ChangeSceneAction(Constants.MenuSceneName));
    }

    private void Initialize()
    {

    }
}
