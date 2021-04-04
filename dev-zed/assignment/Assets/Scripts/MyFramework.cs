using UnityEngine;

public class MyFramework : MonoBehaviour
{
    private Response response;
    private Response response2;

    private void Awake()
    {
        var jsonData = Resources.Load<TextAsset>("json/dong");  // 원본 데이터
        var jsonData2 = Resources.Load<TextAsset>("json/dong2"); // 데이터가 바뀐 상황을 테스트 하기 위한 데이터

        response = JsonUtility.FromJson<Response>(jsonData.text);
        response2 = JsonUtility.FromJson<Response>(jsonData2.text);
    }

    private void Start()
    {
        Map.Instance.Init();                // 데이터 초기화
        Map.Instance.Generate(response);    // 지도 생성
    }

    /// <summary>
    /// 테스트를 위한 인터페이스
    /// </summary>
    private void OnGUI()
    {
        if (GUI.Button(new Rect(0, 0, 100, 50), "지도 생성"))
        {
            Map.Instance.Generate(response);
        }
        else if (GUI.Button(new Rect(110, 0, 100, 50), "지도2 생성"))
        {
            Map.Instance.Generate(response2);
        }
        else if (GUI.Button(new Rect(220, 0, 100, 50), "지도 삭제"))
        {
            Map.Instance.Clear();           // 삭제
            Map.Instance.Init();            // 데이터 초기화
        }
    }
}