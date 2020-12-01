using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class PaperTest
    {
        [TestCase(true, Paper.AlignState.Excellent)]
        [TestCase(true, Paper.AlignState.Good)]
        [TestCase(true, Paper.AlignState.Bad)]
        [TestCase(false, Paper.AlignState.Excellent)]
        [TestCase(false, Paper.AlignState.Good)]
        [TestCase(false, Paper.AlignState.Bad)]
        public void instance(bool isFaceUp, Paper.AlignState alignStatus)
        {
            var paper = Paper.instance(isFaceUp, alignStatus);
            Assert.AreEqual(paper.IsFaceUp, isFaceUp);
            Assert.AreEqual(paper.AlignStatus, alignStatus);
        }

        // bind 何も綴じられていない状態から綴じる
        [Test]
        public void bind_listIsEmpty()
        {
            // 針で綴じられていないことを確認する
            var target = Paper.instance(true, Paper.AlignState.Excellent);
            Assert.AreEqual(getNeedleList(target).Count, 0);

            // 針で綴じる
            target.bind(new Vector2(0, 0));
            Assert.AreEqual(getNeedleList(target).Count, 1);
        }

        // bind 綴じられているところと違うところを綴じる
        [Test]
        public void bind_newBind()
        {
            // 試験対象を生成
            var target = Paper.instance(true, Paper.AlignState.Excellent);

            // 事前に針を生成しておく
            var needle = instanceNeedle(target, new Vector2(0, 0));
            Assert.AreEqual(getNeedleList(target).Count, 1);

            // 生成した針から当たり判定を取得する
            var range = (float)needle.GetType().GetField("range", BindingFlags.NonPublic | BindingFlags.Static).GetValue(needle);

            // 当たり判定から外れる位置に新たな針を生成
            target.bind(new Vector2(range + 1, 0));
            Assert.AreEqual(getNeedleList(target).Count, 2);
        }

        // bind 綴じられているところを綴じようとする
        [Test]
        public void bind_pull()
        {
            // 試験対象を生成
            var target = Paper.instance(true, Paper.AlignState.Excellent);

            // 事前に針を生成しておく
            var position = new Vector2(0, 0);
            instanceNeedle(target, position);
            Assert.AreEqual(getNeedleList(target).Count, 1);

            // 同じ位置に針を生成しようとする
            target.bind(position);
            Assert.AreEqual(getNeedleList(target).Count, 0);
        }

        // turnOver
        [TestCase(true, false)]
        [TestCase(false, true)]
        public void turnOver(bool initial, bool assert)
        {
            // 試験対象を生成
            var target = Paper.instance(initial, Paper.AlignState.Excellent);

            // 裏返し、結果がassertであることを確認する
            target.turnOver();
            Assert.AreEqual(target.IsFaceUp, assert);
        }

        // align
        [TestCase(Paper.AlignState.Bad, Paper.AlignState.Good)]
        [TestCase(Paper.AlignState.Good, Paper.AlignState.Excellent)]
        [TestCase(Paper.AlignState.Excellent, Paper.AlignState.Excellent)]
        public void align(Paper.AlignState initial, Paper.AlignState assert)
        {
            // 試験対象を生成
            var target = Paper.instance(true, initial);

            // 整頓し、結果がassertであることを確認する
            target.align();
            Assert.AreEqual(target.AlignStatus, assert);
        }

        // targetのNeedleListを取得
        private List<Needle> getNeedleList(Paper target)
        {
            return (List<Needle>)target.GetType().GetField("NeedleList", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(target);
        }

        // target.NeedleListに針を追加
        private Needle instanceNeedle(Paper target, Vector2 position)
        {
            // NeedleListを取得
            List<Needle> needleList = getNeedleList(target);

            // NeedleListに同じpositionのものがないことを確認
            foreach (var needle in needleList)
            {
                if (needle.isCollision(position))
                {
                    return null;
                }
            }

            // needleListに新たな針を追加
            var instance = Needle.instance(position);
            needleList.Add(instance);
            return instance;
        }
    }
}
