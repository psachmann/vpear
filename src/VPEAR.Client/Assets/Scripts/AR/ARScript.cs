using Fluxor;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using VPEAR.Core.Wrappers;

public class ARScript : AbstractBase
{
    [SerializeField] private Text _frameDateText;
    [SerializeField] private Button _forwardButton;
    [SerializeField] private Button _backwardButton;
    [SerializeField] private SpriteRenderer _heatmapRenderer;

    private IState<ARState> _arState;
    private ARState _arStateValue;

    private void Start()
    {
        _arState = s_provider.GetRequiredService<IState<ARState>>();
        _arState.StateChanged += ARStateChanged;
        _arStateValue = _arState.Value;
        _forwardButton.onClick.AddListener(OnForwardClick);
        _backwardButton.onClick.AddListener(OnBackwardClick);

        Initialize();

        // ARStateChanged(this, _arStateValue);
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
    }

    private void ARStateChanged(object sender, ARState state)
    {
        _arStateValue = state;
        _frameDateText.text = state.Current.ToString();
        // UpdateSprite();
    }

    private void OnBackwardClick()
    {
        _dispatcher.Dispatch(new MoveBackwardAction(_arStateValue.StepSize));
    }

    private void OnForwardClick()
    {
        _dispatcher.Dispatch(new MoveForwardAction(_arStateValue.StepSize));
    }

    private void UpdateSprite()
    {
        var values = Heatmap.CreateHeatmapValues(
            (int)_arStateValue.Sensors.FirstOrDefault()?.Width,
            (int)_arStateValue.Sensors.FirstOrDefault()?.Height,
            _arStateValue.DeltaMinutes,
            _arStateValue.Current,
            _arStateValue.History);

        var sprite = Heatmap.CreateHeatmapSprite(
            (float)_arStateValue.Sensors.FirstOrDefault()?.Minimum,
            (float)_arStateValue.Sensors.FirstOrDefault()?.Maximum,
            _arStateValue.Threshold,
            values,
            _arStateValue.ColorScale);

        _heatmapRenderer.sprite = sprite;
    }

    private void Initialize()
    {
        var width = 10;
        var height = 10;
        var values = new float[width, height];

        for (var x = 0; x < width; x++)
        {
            for (var y = 0; y < height; y++)
            {
                values[x, y] = (float)y * width + 9;
            }
        }

        _heatmapRenderer.sprite = Heatmap.CreateHeatmapSprite(0f, 100f, 80f, values, ColorScale.RedToGreen);
        _heatmapRenderer.transform.position = new Vector3(1 - width, 1.5f, 1f);
    }
}
