using UnityEngine;

public static class MeshHelpers
{
    public static Mesh CreateMesh(Vector3 position, float rotation, Vector3 baseSize, Vector2 uv00, Vector2 uv11)
    {
        return AddToMesh(null, position, rotation, baseSize, uv00, uv11);
    }

    public static Mesh CreateEmptyMesh()
    {
        var mesh = new Mesh()
        {
            vertices = new Vector3[0],
            uv = new Vector2[0],
            triangles = new int[0]
        };

        return mesh;
    }

    public static void CreateEmptyMeshArrays(int quedCount, out Vector3[] vertices, out Vector2[] uv, out int[] triangles)
    {
        vertices = new Vector3[quedCount * 4];
        uv = new Vector2[quedCount * 4];
        triangles = new int[quedCount * 6];
    }

    public static Mesh AddToMesh(Mesh mesh, Vector3 position, float rotation, Vector3 baseSize, Vector2 uv00, Vector2 uv11)
    {
        if (mesh == null)
        {
            mesh = CreateEmptyMesh();
        }

        var vertices = new Vector3[4 + mesh.vertices.Length];
        var uv = new Vector2[4 + mesh.uv.Length];
        var triangles = new int[6 + mesh.triangles.Length];

        mesh.vertices.CopyTo(vertices, 0);
        mesh.uv.CopyTo(uv, 0);
        mesh.triangles.CopyTo(triangles, 0);

        //Relocate vertices
        var index = vertices.Length / 4 - 1;
        var vIndex = index * 4;
        var vIndex0 = vIndex;
        var vIndex1 = vIndex + 1;
        var vIndex2 = vIndex + 2;
        var vIndex3 = vIndex + 3;

        baseSize *= .5f;

        var skewed = baseSize.x != baseSize.y;
        if (skewed)
        {
            vertices[vIndex0] = position + GetQuaternionEuler(rotation) * new Vector3(-baseSize.x, baseSize.y);
            vertices[vIndex1] = position + GetQuaternionEuler(rotation) * new Vector3(-baseSize.x, -baseSize.y);
            vertices[vIndex2] = position + GetQuaternionEuler(rotation) * new Vector3(baseSize.x, -baseSize.y);
            vertices[vIndex3] = position + GetQuaternionEuler(rotation) * baseSize;
        }
        else
        {
            vertices[vIndex0] = position + GetQuaternionEuler(rotation - 270) * baseSize;
            vertices[vIndex1] = position + GetQuaternionEuler(rotation - 180) * baseSize;
            vertices[vIndex2] = position + GetQuaternionEuler(rotation - 90) * baseSize;
            vertices[vIndex3] = position + GetQuaternionEuler(rotation - 0) * baseSize;
        }

        //Relocate UVs
        uv[vIndex0] = new Vector2(uv00.x, uv11.y);
        uv[vIndex1] = new Vector2(uv00.x, uv00.y);
        uv[vIndex2] = new Vector2(uv11.x, uv00.y);
        uv[vIndex3] = new Vector2(uv11.x, uv11.y);

        //Create triangles
        int tIndex = index * 6;

        triangles[tIndex + 0] = vIndex0;
        triangles[tIndex + 1] = vIndex3;
        triangles[tIndex + 2] = vIndex1;

        triangles[tIndex + 3] = vIndex1;
        triangles[tIndex + 4] = vIndex3;
        triangles[tIndex + 5] = vIndex2;

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uv;

        return mesh;
    }

    public static void AddToMeshArrays(Vector3[] vertices, Vector2[] uv, int[] triangles, int index, Vector3 position, float rotation, Vector3 baseSize, Vector2 uv00, Vector2 uv11)
    {
        //Relocate vertices
        var vIndex = index * 4;
        var vIndex0 = vIndex;
        var vIndex1 = vIndex + 1;
        var vIndex2 = vIndex + 2;
        var vIndex3 = vIndex + 3;

        baseSize *= .5f;

        var skewed = baseSize.x != baseSize.y;
        if (skewed)
        {
            vertices[vIndex0] = position + GetQuaternionEuler(rotation) * new Vector3(-baseSize.x, baseSize.y);
            vertices[vIndex1] = position + GetQuaternionEuler(rotation) * new Vector3(-baseSize.x, -baseSize.y);
            vertices[vIndex2] = position + GetQuaternionEuler(rotation) * new Vector3(baseSize.x, -baseSize.y);
            vertices[vIndex3] = position + GetQuaternionEuler(rotation) * baseSize;
        }
        else
        {
            vertices[vIndex0] = position + GetQuaternionEuler(rotation - 270) * baseSize;
            vertices[vIndex1] = position + GetQuaternionEuler(rotation - 180) * baseSize;
            vertices[vIndex2] = position + GetQuaternionEuler(rotation - 90) * baseSize;
            vertices[vIndex3] = position + GetQuaternionEuler(rotation - 0) * baseSize;
        }

        //Relocate UVs
        uv[vIndex0] = new Vector2(uv00.x, uv11.y);
        uv[vIndex1] = new Vector2(uv00.x, uv00.y);
        uv[vIndex2] = new Vector2(uv11.x, uv00.y);
        uv[vIndex3] = new Vector2(uv11.x, uv11.y);

        //Create triangles
        var tIndex = index * 6;

        triangles[tIndex + 0] = vIndex0;
        triangles[tIndex + 1] = vIndex3;
        triangles[tIndex + 2] = vIndex1;
        triangles[tIndex + 3] = vIndex1;
        triangles[tIndex + 4] = vIndex3;
        triangles[tIndex + 5] = vIndex2;
    }

    private static Quaternion[] s_cachedQuaternionEulerArray;

    private static void CacheQuaternionEuler()
    {
        if (s_cachedQuaternionEulerArray != null)
        {
            return;
        }

        s_cachedQuaternionEulerArray = new Quaternion[360];

        for (int i = 0; i < 360; i++)
        {
            s_cachedQuaternionEulerArray[i] = Quaternion.Euler(0, 0, i);
        }
    }

    private static Quaternion GetQuaternionEuler(float rotationFloat)
    {
        var rotation = Mathf.RoundToInt(rotationFloat);
        rotation = rotation % 360;

        if (rotation < 0)
        {
            rotation += 360;
        }
        if (rotation >= 360)
        {
            rotation -= 360;
        }

        if (s_cachedQuaternionEulerArray == null)
        {
            CacheQuaternionEuler();
        }

        return s_cachedQuaternionEulerArray[rotation];
    }
}
