using System;
using System.Collections.Generic;
using UnityEngine;

public class Util : MonoBehaviour
{
    //주어진 sourceList에서 count개를 중복없이 반환
    public static List<T> GetRandomElementsInList<T>(List<T> sourceList, int count)
    {
        if (sourceList == null)
        {
            Debug.LogError("[Util.GetRandom] : SourceList is Empty");
            return null;
        }

        if (count <= 0)
        {
            Debug.LogError("[Util.GetRandom] : count is zero or minus");
            return null;
        }
            

        // 요청 개수가 리스트보다 크면 리스트 전체 반환
        if (count >= sourceList.Count)
        {
            Debug.LogError("[Util.GetRandom] : sourceList's count is less than count");
            return new List<T>(sourceList);
        }
            

        // 리스트 복사
        List<T> tempList = new List<T>(sourceList);
        List<T> result = new List<T>();

        for (int i = 0; i < count; i++)
        {
            int index = UnityEngine.Random.Range(0, tempList.Count);
            result.Add(tempList[index]);
            tempList.RemoveAt(index); // 중복 방지
        }

        return result;
    }
}
