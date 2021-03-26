using Fluxor;
using Microsoft.Extensions.DependencyInjection;
using UnityEngine;
using UnityEngine.UI;

public class ARScript : AbstractBase
{
    [SerializeField] private Text _frameDateText;
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
        var width = (int)_arStateValue.Sensors[0].Width;
        var height = (int)_arStateValue.Sensors[0].Height;
        var min = (float)_arStateValue.Sensors[0].Minimum;
        var max = (float)_arStateValue.Sensors[0].Maximum;
        var threshold = (float)_arStateValue.Threshold;
        var values = Heatmap.CreateHeatmapValues(width, height, _arStateValue.DeltaMinutes, _arStateValue.Current, _arStateValue.History);
        var colors = Heatmap.CreateHeatmapColors(min, max, threshold, values, ColorScale.RedToGreen);
        var texture = Heatmap.CreateHeatmapTexture(width, height, colors);
        var sprite = Sprite.Create(texture, new Rect(0f, 0f, width, height), Vector2.one * 0.5f);

        _heatmapVisual.sprite = sprite;

        var scaleX = _heatmap.rect.width / _heatmapVisual.size.x;
        var scaleY = _heatmap.rect.height / _heatmapVisual.size.y;

        _heatmapVisual.transform.localScale = new Vector3(scaleX, scaleY);
    }

    private void Initialize()
    {
        var width = 64;
        var height = 27;
        var values = new float[width, height];

        for (var x = 0; x < width; x++)
        {
            for (var y = 0; y < height; y++)
            {
                values[x, y] = Random.Range(0f, 100f);
            }
        }

        var colors = Heatmap.CreateHeatmapColors(0f, 100f, 80f, values, ColorScale.RedToGreen);
        var texture = Heatmap.CreateHeatmapTexture(width, height, colors, FilterMode.Bilinear);
        var sprite = Sprite.Create(texture, new Rect(0f, 0f, width, height), Vector2.one * 0.5f);

        _heatmapVisual.sprite = sprite;

        var scaleX = _heatmap.rect.width / _heatmapVisual.size.x;
        var scaleY = _heatmap.rect.height / _heatmapVisual.size.y;

        _heatmapVisual.transform.localScale = new Vector3(scaleX, scaleY);
    }
}
