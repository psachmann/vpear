using Fluxor;
using Microsoft.Extensions.DependencyInjection;
using UnityEngine;
using UnityEngine.UI;

public class ARScript : AbstractBase
{
    [SerializeField] private Button _backButton;
    [SerializeField] private MeshFilter _meshFilter;

    private Mesh _mesh;
    private IState<ARState> _arState;

    private void Start()
    {
        _arState = s_provider.GetRequiredService<IState<ARState>>();
        _arState.StateChanged += ARStateChanged;
        _mesh = new Mesh();
        _meshFilter.mesh = _mesh;

        ARStateChanged(this, _arState.Value);
    }

    private void Update()
    {
    }

    private void ARStateChanged(object sender, ARState state)
    {
        MeshHelpers.CreateEmptyMeshArrays(state.GridMesh.Width * state.GridMesh.Height, out var vertices, out var uv, out var triangles);
        var grid = new GridMesh(5, 5, 0, 100, 1f, Vector3.zero);

        for (var x = 0; x < grid.Width; x++)
        {
            for (var y = 0; y < grid.Height; y++)
            {
                var index = x * grid.Height + y;
                var squadSize = new Vector3(1, 1) * grid.CellSize;

                _logger.Information($"index: {index}");

                // MeshHelpers.AddToMeshArrays(vertices, uv, triangles, index, grid.GetWorldPosition(x, y), 0f, squadSize, Vector2.zero, Vector2.zero);
            }
        }
    }
}
