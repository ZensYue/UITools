
//
// @Author: ZensYue
// @Date: 2022 - 01 - 24 11:00:47
//

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// 在Game窗口快速选中带Raycast的UI节点
/// </summary>
public class QuickSelectUI : MonoBehaviour {

    /// <summary>
    /// 连按超过改时间 重新开始
    /// </summary>
    public float interval = 2.0f;
    public KeyCode selKeyCode = KeyCode.LeftControl;
#if UNITY_EDITOR
    // Use this for initialization
    void Awake() {
        DontDestroyOnLoad(this.gameObject);
	}

    List<RaycastResult> curResults = new List<RaycastResult>();
    int index = 0;
    float lastTime = -1;

    /// <summary>
    /// 是否和上次选中是同一个
    /// 超过间隔或者获取符合添加的节点列表变化 均重新开始选中。否则跳到下一个
    /// </summary>
    /// <param name="raycastResults"></param>
    /// <returns></returns>
    private bool IsSameRaycastResult(List<RaycastResult> raycastResults)
    {
        if (Time.time - lastTime > interval) return false;
        if (curResults.Count != raycastResults.Count)
        {
            return false;
        }
        for (int i = 0; i < raycastResults.Count; i++)
        {
            if (curResults[i].gameObject != raycastResults[i].gameObject)
                return false;
        }
        return true;
    }

    // Update is called once per frame
    void Update () {
        if (Input.GetKeyDown(selKeyCode))
        {
            PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
            eventDataCurrentPosition.position = Input.mousePosition;

            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
            if (results.Count > 0)
            {
                if (IsSameRaycastResult(results))
                {
                    index++;
                    if (index >= results.Count)
                        index = 0;
                }
                else
                {
                    curResults = results;
                    index = 0;
                }
                lastTime = Time.time;
                UnityEditor.EditorGUIUtility.PingObject(results[index].gameObject);
            }
        }
    }
#endif
}
