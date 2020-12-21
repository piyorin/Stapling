using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stapler : MonoBehaviour
{
    // 操作対象の紙
    private Paper target;

    // クリック開始位置
    private Vector3 clickPosition;

    // 横スワイプ判定とする距離
    private const float RANGE_TO_JUDGE_AS_HORIZONTAL_SWIPE = 50;

    // 縦スワイプ判定とする距離
    private const float RANGE_TO_JUDGE_AS_VERTICAL_SWIPE = 50;

    private void Update()
    {
        // クリックが始まった時、座標を保持する
        if (Input.GetMouseButtonDown(0))
        {
            clickPosition = Input.mousePosition;
            Debug.Log(string.Format("クリックが始まった。 x:{0} y:{1}", clickPosition.x, clickPosition.y));
        }

        // クリックが終わった時、始まったときの座標と終わったときの座標を見比べて、タップなのかスワイプなのか判定する
        if (Input.GetMouseButtonUp(0))
        {
            Debug.Log("クリックが終わった。");
            execute(Input.mousePosition);

        }
    }

    /// <summary>
    /// 綴じる対象の紙をセットする
    /// </summary>
    /// <param name="paper">紙</param>
    public void setTarget(Paper paper)
    {
        target = paper;
    }

    // タップかスワイプか判定し、対応した処理を実施
    private void execute(Vector3 clickEndPosition)
    {
        // 横スワイプ判定距離以上に横へ動いていれば横スワイプ
        if (RANGE_TO_JUDGE_AS_HORIZONTAL_SWIPE <= Mathf.Abs(clickPosition.x - clickEndPosition.x))
        {
            // 紙を裏返す
            target.turnOver();
        }
        // 縦スワイプ判定距離以上に下へ動いていれば下スワイプ
        else if (clickPosition.y - clickEndPosition.y <= -RANGE_TO_JUDGE_AS_VERTICAL_SWIPE)
        {
            // 紙を整頓する
            target.align();
        }
        // 縦スワイプ距離以上に上へ動いていれば上スワイプ
        else if (RANGE_TO_JUDGE_AS_VERTICAL_SWIPE <= clickPosition.y - clickEndPosition.y)
        {
            // 紙を提出する
            Debug.Log("紙を提出した。");
        }
        // 縦にも横にもあんまり動いていなければタップ
        else
        {
            // 終わったとき座標の位置で綴じる
            target.bind(clickEndPosition);
        }
    }
}
