using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LevelEditor
{
    public class LevelMakerTube : Tube
    {
        protected override void Awake()
        {
            base.Awake();
            InitBallPos(testingSprite);
        }

        public void Init()
        {
            Debug.Log("init");
            Awake();
        }
    }


}
