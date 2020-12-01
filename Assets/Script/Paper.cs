using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paper : MonoBehaviour
{
    private static string prefabPath = "Prefabs/Paper";

    // 整頓状態(enum)
    public enum AlignState { Excellent, Good, Bad }

    // 綴じた針
    private List<Needle> NeedleList = new List<Needle>();

    // 表裏
    // true=表, false=裏
    private bool isFaceUp;
    public bool IsFaceUp
    {
        get
        {
            return isFaceUp;
        }
    }

    // 整頓状態
    private AlignState alignStatus;
    public AlignState AlignStatus
    {
        get
        {
            return alignStatus;
        }
    }

    /// <summary>
    /// 指定された裏表状態・整頓状態の紙を生成
    /// </summary>
    /// <param name="isFaceUp">裏表状態</param>
    /// <param name="alignStatus">整頓状態</param>
    /// <returns>Paperスクリプト</returns>
    public static Paper instance(bool isFaceUp, AlignState alignStatus)
    {
        GameObject instance = Instantiate((GameObject)Resources.Load(prefabPath));
        Paper script = (Paper)instance.GetComponent(typeof(Paper).Name);
        script.isFaceUp = isFaceUp;
        script.alignStatus = alignStatus;
        return script;
    }

    /// <summary>
    /// 指定座標を針で綴じる
    /// <param name="position">生成位置</param>
    /// </summary>
    public void bind(Vector2 position)
    {
        // 指定位置に針があるか判定
        int index = getNeedleIndex(position);

        if (index < 0)
        {
            // 針がなければ綴じる
            bindNeedle(position);
        }
        else
        {
            // 針があれば抜く
            pullNeedle(index);
        }
    }

    /// <summary>
    /// 裏返す
    /// </summary>
    public void turnOver()
    {
        isFaceUp = !isFaceUp;
        Debug.Log("紙を裏返した。");
        Debug.Log(string.Format("is face up? :{0}", isFaceUp));
    }

    /// <summary>
    /// 不揃いな紙を揃える
    /// </summary>
    public void align()
    {
        // 整頓されきっているならそれ以上整頓しない
        if (alignStatus != AlignState.Excellent)
        {
            // 一段階そろえる
            alignStatus = (AlignState)Enum.ToObject(typeof(AlignState), (int)alignStatus - 1);
        }

        Debug.Log("紙を揃えた。");
        Debug.Log(string.Format("align status:{0}", alignStatus));
    }

    // NeedleListに指定位置付近の針が含まれるか判定
    // 含まれていればindexを、含まれていなければ-1を返す
    private int getNeedleIndex(Vector2 position)
    {
        for (int i = 0; i < NeedleList.Count; i++)
        {
            if (NeedleList[i].isCollision(position))
            {
                // 指定位置付近の針が見つかった
                return i;
            }
        }

        // 指定位置付近の針が見つからなかった
        return -1;
    }

    // 指定indexのNeedleをNeedleListから削除
    private void pullNeedle(int index)
    {
        // 指定位置がNeedleListの範囲を超えていないか確認
        if (index >= NeedleList.Count)
        {
            Debug.Log("NeedleListの範囲外の針を抜こうとした？");
            Debug.Log(string.Format("target index:{0} NeedleList.Count", index, NeedleList.Count));
            return;
        }

        // 指定位置のNeedleを削除
        Debug.Log("針を抜いた。");
        Debug.Log(string.Format("index:{0}", index));
        NeedleList[index].pull();
        NeedleList.RemoveAt(index);
    }

    // 指定位置に針を生成
    private void bindNeedle(Vector2 position)
    {
        var instance = Needle.instance(position);
        NeedleList.Add(instance);
        Debug.Log("針で綴じた。");
        Debug.Log(string.Format("position.x:{0} position.y:{1}", position.x, position.y));
    }
}
