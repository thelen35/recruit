using System;

[Serializable]
public class Response
{
    public bool success;
    public int code;
    public Building[] data;
}

[Serializable]
public class Building
{
    public RoomType[] roomtypes;
    public DongMeta meta;
}

[Serializable]
public class RoomType
{
    public string[] coordinatesBase64s;
    public RoomTypeMeta meta;
}

[Serializable]
public class DongMeta
{
    public int bd_id;
    public string 동;
    public int 지면높이;
}

[Serializable]
public class RoomTypeMeta
{
    public int 룸타입id;
}