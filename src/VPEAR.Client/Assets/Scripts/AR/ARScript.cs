using Fluxor;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class ARScript : AbstractBase
{
    [SerializeField] private Text _frameDateText;
    [SerializeField] private Text _fistDateText;
    [SerializeField] private Text _lastDateText;
    [SerializeField] private Button _forwardButton;
    [SerializeField] private Button _backwardButton;
    [SerializeField] private RectTransform _heatmap;
    [SerializeField] private SpriteRenderer _heatmapVisual;

    private IState<ARState> _arState;
    private ARState _arStateValue;

    private void Start()
    {
        _arState = s_provider.GetRequiredService<IState<ARState>>();
        _arState.StateChanged += ARStateChanged;
        _arStateValue = _arState.Value;
        _forwardButton.onClick.AddListener(OnForwardClick);
        _backwardButton.onClick.AddListener(OnBackwardClick);

        ARStateChanged(this, _arStateValue);
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
        _frameDateText.text = state.Current.Time.ToLocalTime().ToString();
        _fistDateText.text = state.History.First().Time.ToLocalTime().ToString();
        _lastDateText.text = state.History.Last().Time.ToLocalTime().ToString();

        UpdateSprite();
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
        _logger.Warning("Hard coded values for width, height, min and max!");

        var width = 64;
        var height = 27;
        var min = 0f;
        var max = 100f;
        var values = Heatmap.CreateHeatmapValues(width, height, _arStateValue.DeltaMinutes, _arStateValue.Current, _arStateValue.History);
        values = Heatmap.Scale(4, values, Heatmap.InterpolationMehtod.Bicubic);
        var colors = Heatmap.CreateHeatmapColors(min, max, values, _arStateValue.ColorScale);
        var texture = Heatmap.CreateHeatmapTexture(values.GetLength(0), values.GetLength(1), colors);
        var sprite = Sprite.Create(texture, new Rect(0f, 0f, values.GetLength(0), values.GetLength(1)), Vector2.one * 0.5f);

        LogValues(values);

        _heatmapVisual.sprite = sprite;

        var scaleX = _heatmap.rect.width / _heatmapVisual.size.x;
        var scaleY = _heatmap.rect.height / _heatmapVisual.size.y;

        _heatmapVisual.transform.localScale = new Vector3(scaleX, scaleY);
    }

    private void LogValues(double[,] values)
    {
        var builder = new StringBuilder();

        for (var x = 0; x < values.GetLength(0); x++)
        {
            builder.Append($"{x}: ");
            for (var y = 0; y < values.GetLength(1); y++)
            {
                builder.Append(string.Format("{0:0.##} ", values[x, y]));
            }
            builder.Append("\n");
        }

        _logger.Warning(builder.ToString());
    }
}
