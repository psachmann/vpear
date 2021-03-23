using Fluxor;
using Microsoft.Extensions.DependencyInjection;
using UnityEngine;
using UnityEngine.UI;

public class ARScript : AbstractBase
{
    [SerializeField] private Text _frameDateText;
    [SerializeField] private Button _forwardButton;
    [SerializeField] private Button _backwardButton;
    [SerializeField] private SpriteRenderer _heatmapRenderer;

    private IState<ARState> _arState;
    private IState<SettingsState> _settingsState;

    private void Start()
    {
        _arState = s_provider.GetRequiredService<IState<ARState>>();
        _arState.StateChanged += ARStateChanged;
        _settingsState = s_provider.GetRequiredService<IState<SettingsState>>();
        _settingsState.StateChanged += SettingsStateChanges;

        Initialize();

        ARStateChanged(this, _arState.Value);
        SettingsStateChanges(this, _settingsState.Value);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            _dispatcher.Dispatch(new ChangeSceneAction(Constants.MenuSceneId));
        }
    }

    private void OnDestroy()
    {
        _arState.StateChanged -= ARStateChanged;
        _settingsState.StateChanged -= SettingsStateChanges;
    }

    private void ARStateChanged(object sender, ARState state)
    {
    }

    private void SettingsStateChanges(object sender, SettingsState state)
    {
    }

    private void Initialize()
    {
        var width = 20;
        var height = 10;
        var texture = new Texture2D(width, height);
        var colors = new Color32[width * height];

        for (var x = 0; x < width; x++)
        {
            for (var y = 0; y < height; y++)
            {
                colors[x + y * width] = Color.Lerp(Color.black, Color.white, (float)y / width);
            }
        }

        texture.SetPixels32(colors);
        texture.Apply();
        texture.wrapMode = TextureWrapMode.Clamp;
        texture.filterMode = FilterMode.Point;

        _heatmapRenderer.sprite = Sprite.Create(texture, new Rect(0f, 0f, width, height), Vector2.zero, 1f);
        _heatmapRenderer.transform.position = new Vector3(1 - width, 1.5f, 1f);
    }

    private static Vector3 GetHeatmapPosition(float x, float y, float width, float height)
    {
        var newX = x - width / 2;
        var newY = y + 1.5f;
        var newZ = height / 2;

        return new Vector3(newX, newY, newZ);
    }
}
