using System.Collections.Generic;
using UnityEngine;

public class BuildingObject
{
    private readonly GameObject go;

    private Mesh mesh;
    private Material material;

    public MeshFilter MeshFilter { get; private set; }
    public MeshRenderer MeshRenderer { get; private set; }

    public BuildingObject() : base()
    {
        go = new GameObject();
        MeshFilter = go.AddComponent<MeshFilter>();
        MeshRenderer = go.AddComponent<MeshRenderer>();
    }

    public void Clear()
    {
        Object.Destroy(go);
        Object.Destroy(material);
        Object.Destroy(mesh);
        mesh = null;
        material = null;
        MeshFilter = null;
        MeshRenderer = null;
    }

    public void SetActive(bool value)
    {
        go.SetActive(value);
    }

    public Mesh CreateMesh(List<Vector3> vertices, List<int> triangles)
    {
        if (mesh == null)
        {
            mesh = new Mesh();
        }

        mesh.Clear();
        mesh.SetVertices(vertices);
        mesh.SetTriangles(triangles, 0, false);
        mesh.RecalculateNormals();

        MeshFilter.sharedMesh = mesh;

        return mesh;
    }

    public void MaterialSetting(int height)
    {
        if (material == null)
        {
            material = MeshRenderer.material;
        }

        material.SetTextureScale("_BaseMap", new Vector2(1, height));
        MeshRenderer.sharedMaterial = material;
    }

    public string Name
    {
        get => go.name;
        set => go.name = value;
    }
}