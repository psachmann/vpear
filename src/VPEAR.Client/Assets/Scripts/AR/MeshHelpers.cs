using UnityEngine;

public static class MeshHelpers
{
    public static Mesh CreateMesh()
    {
        throw new System.NotImplementedException();
    }

    public static Mesh CreateEmptyMesh()
    {
        var mesh = new Mesh();

        mesh.vertices = new Vector3[0];
        mesh.uv = new Vector2[0];
        mesh.triangles = new int[0];

        return mesh;
    }

    public static void CreateEmptyMeshArrays(int quedCount, out Vector3[] vertices, out Vector2[] uv, out int[] triangles)
    {
        vertices = new Vector3[quedCount * 4];
        uv = new Vector2[quedCount * 4];
        triangles = new int[quedCount * 6];
    }
}
