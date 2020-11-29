using NUnit.Framework;
using System.Reflection;
using UnityEngine;

namespace Tests
{
    public class NeedleTest
    {
        // instance
        [Test]
        public void instance()
        {
            const float x = 0;
            const float y = 0;
            var instance = Needle.instance(new Vector2(x, y));

            // 生成した針の座標が正しいか確認
            var targetPosition = getPosition(instance);
            Assert.AreEqual(targetPosition.x, x);
            Assert.AreEqual(targetPosition.y, y);
        }

        // judge 正常系
        [TestCase(1, 1)]
        [TestCase(-1, 1)]
        [TestCase(1, -1)]
        [TestCase(-1, -1)]
        public void judge_normal(int xCode, int yCode)
        {
            // 針を生成し、当たり判定許容範囲を取得する
            var instance = Needle.instance(new Vector2(0, 0));
            var range = getRange(instance);
            // 当たり判定(xとyに1か-1を掛けて上限と下限を調べる)
            var result = instance.isCollision(new Vector2(range * xCode, range * yCode));
            Assert.IsTrue(result);
        }

        // judge 異常系
        [TestCase(1, 1)]
        [TestCase(-1, 1)]
        [TestCase(1, -1)]
        [TestCase(-1, -1)]
        public void judge_error(int xCode, int yCode)
        {
            // 針を生成し、当たり判定許容範囲を取得する
            var instance = Needle.instance(new Vector2(0, 0));
            var range = getRange(instance);
            // 当たり判定(xとyに1か-1を掛けた上で加算し上限超過と下限未満を調べる)
            var result = instance.isCollision(new Vector2(range * xCode + xCode, range * yCode + yCode));
            Assert.IsFalse(result);
        }

        // instanceのpositionを取得する
        private Vector2 getPosition(Needle instance)
        {
            return (Vector2)instance.GetType().GetField("position", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(instance);
        }

        // instanceの当たり判定許容範囲を取得する
        private float getRange(Needle instance)
        {
            return (float)instance.GetType().GetField("range", BindingFlags.NonPublic | BindingFlags.Static).GetValue(instance);
        }
    }
}
