using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Needle : MonoBehaviour
{
    private static string prefabPath = "Prefabs/Needle";

    // 当たり判定
    private static float range = 50;

    // 座標
    private Vector2 position;

    /// <summary>
    /// 指定座標に針を生成
    /// </summary>
    /// <param name="position">生成座標</param>
    /// <returns>生成した針のNeedleクラス</returns>
    public static Needle instance(Vector2 position)
    {
        Debug.Log(string.Format("針が生成された。x:{0} y:{1}", position.x, position.y));
        GameObject instance = Instantiate((GameObject)Resources.Load(prefabPath));
        Needle script = (Needle)instance.GetComponent(typeof(Needle).Name);
        script.position = position;
        return script;
    }

    /// <summary>
    /// 指定座標から一定範囲内にこの針があるか確認
    /// </summary>
    /// <param name="position">判定座標</param>
    /// <returns>一定範囲内にこの針があるならtrue, なければfalse</returns>
    public bool isCollision(Vector2 position)
    {
        return inRange(this.position.x, position.x) && inRange(this.position.y, position.y);
    }

    // eitherPositionがtargetPosition+-rangeの範囲にあるか判定
    private bool inRange(float targetPosition, float eitherPosition)
    {
        return targetPosition - range <= eitherPosition && eitherPosition <= targetPosition + range;
    }
}
