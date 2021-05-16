using Fluxor;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class ARScript : AbstractBase
{
    [SerializeField] private Text _frameDateText;
    [SerializeField] private Text _fistDateText;
    [SerializeField] private Text _lastDateText;
    [SerializeField] private Text _maxHeatmapValueText;
    [SerializeField] private Button _forwardButton;
    [SerializeField] private Button _backwardButton;
    [SerializeField] private Image _heatmapLegendVisual;
    [SerializeField] private RectTransform _heatmap;
    [SerializeField] private SpriteRenderer _heatmapVisual;

    private IState<ARState> _arState;
    private ARState _arStateValue;

    private void Start()
    {
        _arState = s_provider.GetRequiredService<IState<ARState>>();
        _arState.StateChanged += ARStateChanged;
        _forwardButton.onClick.AddListener(OnForwardClick);
        _backwardButton.onClick.AddListener(OnBackwardClick);

        ARStateChanged(this, _arState.Value);
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
        _frameDateText.text = state.Current.Time
            .ToLocalTime()
            .ToString("T");
        _fistDateText.text = state.History.First()
            .Time
            .ToLocalTime()
            .ToString("T");
        _lastDateText.text = state.History.Last()
            .Time
            .ToLocalTime()
            .ToString("T");
        _maxHeatmapValueText.text = $"- {state.Threshold} mmHg";

        if (state.Current == state.History.First())
        {
            _fistDateText.text = string.Empty;
        }

        if (state.Current == state.History.Last())
        {
            _lastDateText.text = string.Empty;
        }

        UpdateHeatmapLegendVisual(state);
        UpdateHeatmapSprite(state);
    }

    private void OnBackwardClick()
    {
        _dispatcher.Dispatch(new MoveBackwardAction(_arStateValue.StepSize));
    }

    private void OnForwardClick()
    {
        _dispatcher.Dispatch(new MoveForwardAction(_arStateValue.StepSize));
    }

    private async void UpdateHeatmapSprite(ARState state)
    {
        _logger.Warning("Hard coded values for width, height, min and max!");

        var width = 64;
        var height = 27;
        var min = 0f;
        var max = 100f;
        // this allows to run the interpolation in another thread and won't freeze the UI
        var values = await Task.Run(() => Heatmap.CreateHeatmapValues(width, height, state.DeltaMinutes, state.Current, state.History));
        values = await Task.Run(() =>  Heatmap.Scale(8, values, Heatmap.InterpolationMehtod.Bicubic));
        var colors = Heatmap.CreateHeatmapColors(min, max, values, state.ColorScale);
        var texture = Heatmap.CreateHeatmapTexture(values.GetLength(0), values.GetLength(1), colors, FilterMode.Trilinear);
        var sprite = Sprite.Create(texture, new Rect(0f, 0f, values.GetLength(0), values.GetLength(1)), Vector2.one * 0.5f);

        _heatmapVisual.sprite = sprite;

        var scaleX = _heatmap.rect.width / _heatmapVisual.size.x;
        var scaleY = _heatmap.rect.height / _heatmapVisual.size.y;

        _heatmapVisual.transform.localScale = new Vector3(scaleX, scaleY);
    }

    private void UpdateHeatmapLegendVisual(ARState state)
    {
        var texture = Heatmap.GetColorScale(state.ColorScale);
        var sprite = Sprite.Create(
            texture,
            new Rect(0f, 0f, texture.width, texture.height),
            Vector2.one * 0.5f);
        _heatmapLegendVisual.sprite = sprite;
    }
}
