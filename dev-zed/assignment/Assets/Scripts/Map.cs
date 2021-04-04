using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Map
{
    private static Map instance;

    private ObjectPool objectPool;
    private Material material;
    private Texture texture;

    public static Map Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new Map();
            }

            return instance;
        }
    }

    public void Init()
    {
        objectPool = new ObjectPool(50);
        material = new Material(Shader.Find("Universal Render Pipeline/Lit"));

        texture = Resources.Load<Texture>("texture/buildingTester_d");
        material.SetTexture("_BaseMap", texture);
    }

    public void Clear()
    {
        objectPool.Clear();
        UnityEngine.Object.Destroy(material);
        material = null;
        texture = null;
        objectPool = null;
        instance = null;
    }

    public void Generate(Response response)
    {
        if (!response.success)
        {
            return;
        }

        objectPool.PushAll();

        foreach (var building in response.data)
        {
            List<Vector3> vertices = CalculateVertices(building);
            List<int> triangles = new List<int>(Enumerable.Range(0, vertices.Count));

            var obj = objectPool.Pop;
            obj.Name = $"{building.meta.bd_id}";
            obj.MeshRenderer.sharedMaterial = material;
            obj.MaterialSetting((int)((vertices.Max(t => t.y) - vertices.Min(t => t.y)) / 3));

            var mesh = obj.CreateMesh(vertices, triangles);
            CalculateUVs(mesh);
        }
    }

    private List<Vector3> CalculateVertices(Building building)
    {
        List<Vector3> vertices = new List<Vector3>();

        foreach (var roomtype in building.roomtypes)
        {
            foreach (var base64s in roomtype.coordinatesBase64s)
            {
                var byteArray = Convert.FromBase64String(base64s);
                var floatArray = new float[byteArray.Length / 4];
                Buffer.BlockCopy(byteArray, 0, floatArray, 0, byteArray.Length);

                for (int i = 0; i < floatArray.Length; i += 3)
                {
                    var vertex = new Vector3(floatArray[i], floatArray[i + 2], floatArray[i + 1]);
                    vertices.Add(vertex);
                }
            }
        }

        return vertices;
    }

    private void CalculateUVs(Mesh mesh)
    {
        List<Vector2> uvs = new List<Vector2>();

        for (int i = 0; i < mesh.vertices.Length; i += 6)
        {
            var normal = mesh.normals[i];
            var angle = Vector3.Angle(normal, Vector3.forward);
            if (normal.x < 0)
            {
                angle = 360 - angle;
            }

            var quad = mesh.vertices.Skip(i).Take(6).ToArray();
            float xMin = quad.Min(t => t.x);
            float xMax = quad.Max(t => t.x);
            float yMin = quad.Min(t => t.y);
            float yMax = quad.Max(t => t.y);
            float zMin = quad.Min(t => t.z);
            float zMax = quad.Max(t => t.z);

            foreach (var ver in quad)
            {
                Vector2 uv = new Vector2();

                if (180 <= angle && angle <= 220)
                {
                    uv.x = Mathf.InverseLerp(xMin, xMax, ver.x) * 0.5f;
                    uv.y = Mathf.InverseLerp(yMin, yMax, ver.y);
                }
                else if (normal == Vector3.up || normal == Vector3.down)
                {
                    uv.x = Mathf.InverseLerp(xMin, xMax, ver.x) * 0.25f + 0.75f;
                    uv.y = Mathf.InverseLerp(zMin, zMax, ver.z);
                }
                else
                {
                    uv.x = Mathf.InverseLerp(yMin, yMax, ver.z) * 0.25f + 0.5f;
                    uv.y = Mathf.InverseLerp(zMin, zMax, ver.y);
                }

                uvs.Add(uv);
            }
        }

        mesh.SetUVs(0, uvs);
    }
}