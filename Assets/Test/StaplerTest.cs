using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class StaplerTest
    {
        [Test]
        public void setTarget()
        {
            // 紙とステープラを生成し、setTargetで紙をセット
            var target = new Paper();
            var stapler = new Stapler();
            stapler.setTarget(target);

            // リフレクションを使って、きちんとセットされたか確認
            var setObject = (Paper)stapler.GetType().GetField("target", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(stapler);
            Assert.AreEqual(target, setObject);
        }

        [TestCase(1, true, false)]
        [TestCase(1, false, true)]
        [TestCase(-1, true, false)]
        [TestCase(-1, false, true)]
        public void execute_turnOver(int code, bool initial, bool assert)
        {
            // テストで使用するオブジェクトを生成
            var paper = Paper.instance(initial, Paper.AlignState.Excellent);
            Assert.AreEqual(paper.IsFaceUp, initial);
            var stapler = new Stapler();
            stapler.setTarget(paper);
            var clickPosition = (Vector3)stapler.GetType().GetField("clickPosition", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(stapler);
            clickPosition = Vector3.zero;

            // 実行
            var distance = getRangeToJudge(stapler, "RANGE_TO_JUDGE_AS_HORIZONTAL_SWIPE");
            var objects = new object[1];
            objects[0] = (object)new Vector3((distance + 1) * code, 0, 0);
            stapler.GetType().GetMethod("execute", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(stapler, objects);

            // paper.turnOverが実行されたはずなので結果を確認する
            Assert.AreEqual(paper.IsFaceUp, assert);
        }

        [TestCase(Paper.AlignState.Bad, Paper.AlignState.Good)]
        [TestCase(Paper.AlignState.Good, Paper.AlignState.Excellent)]
        [TestCase(Paper.AlignState.Excellent, Paper.AlignState.Excellent)]
        public void execute_align(Paper.AlignState initial, Paper.AlignState assert)
        {
            // テストで使用するオブジェクトを生成
            var paper = Paper.instance(true, initial);
            Assert.AreEqual(paper.AlignStatus, initial);
            var stapler = new Stapler();
            stapler.setTarget(paper);
            var clickPosition = (Vector3)stapler.GetType().GetField("clickPosition", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(stapler);
            clickPosition = Vector3.zero;

            // 実行
            var distance = getRangeToJudge(stapler, "RANGE_TO_JUDGE_AS_VERTICAL_SWIPE");
            var objects = new object[1];
            objects[0] = (object)new Vector3(0, distance + 1, 0);
            stapler.GetType().GetMethod("execute", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(stapler, objects);

            Debug.Log(paper.AlignStatus);
            // paper.alignが実行されたはずなので結果を確認する
            Assert.AreEqual(paper.AlignStatus, assert);
        }

        [TestCase(1, 1)]
        [TestCase(1, -1)]
        [TestCase(-1, 1)]
        [TestCase(-1, -1)]
        public void execute_bind(int xCode, int yCode)
        {
            // テストで使用するオブジェクトを生成
            var paper = Paper.instance(true, Paper.AlignState.Excellent);
            Assert.AreEqual(getNeedleListCount(paper), 0);
            var stapler = new Stapler();
            stapler.setTarget(paper);
            var clickPosition = (Vector3)stapler.GetType().GetField("clickPosition", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(stapler);
            clickPosition = Vector3.zero;

            // 実行
            var horizontal = getRangeToJudge(stapler, "RANGE_TO_JUDGE_AS_HORIZONTAL_SWIPE");
            var vertical = getRangeToJudge(stapler, "RANGE_TO_JUDGE_AS_VERTICAL_SWIPE");
            var objects = new object[1];
            objects[0] = (object)new Vector3((horizontal - 0.1f) * xCode, (vertical - 0.1f) * yCode, 0);
            stapler.GetType().GetMethod("execute", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(stapler, objects);

            // paper.bindが実行されたはずなので結果を確認する
            Assert.AreEqual(getNeedleListCount(paper), 1);
        }

        // RANGE_TO_JUDGE_AS_HORIZONTAL_SWIPEまたはRANGE_TO_JUDGE_AS_VERTICAL_SWIPEを取得する
        private float getRangeToJudge(Stapler target, string fieldName)
        {
            return (float)target.GetType().GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Static).GetValue(target);
        }

        // target.NeedleList.countを取得
        private int getNeedleListCount(Paper target)
        {
            return ((List<Needle>)target.GetType().GetField("NeedleList", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(target)).Count;
        }
    }
}
