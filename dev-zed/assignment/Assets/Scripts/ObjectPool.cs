using System.Collections.Generic;

public class ObjectPool
{
    private readonly Stack<BuildingObject> pool;
    private readonly HashSet<BuildingObject> used;

    public ObjectPool(int defaultCount)
    {
        pool = new Stack<BuildingObject>();
        used = new HashSet<BuildingObject>();

        for (int i = 0; i < defaultCount; i++)
        {
            CreateGameObject();
        }
    }

    public void Clear()
    {
        PushAll();

        while (pool.Count > 0)
        {
            var obj = pool.Pop();
            obj.Clear();
        }
        pool.Clear();
    }

    public void PushAll()
    {
        foreach (var obj in used)
        {
            obj.SetActive(false);
            pool.Push(obj);
        }

        used.Clear();
    }

    public BuildingObject Pop
    {
        get
        {
            if (pool.Count == 0)
            {
                CreateGameObject();
            }

            var obj = pool.Pop();
            obj.SetActive(true);
            used.Add(obj);

            return obj;
        }
    }

    public BuildingObject Push
    {
        set
        {
            value.SetActive(false);
            pool.Push(value);
            used.Remove(value);
        }
    }

    private void CreateGameObject()
    {
        BuildingObject bo = new BuildingObject();
        bo.SetActive(false);
        pool.Push(bo);
    }
}