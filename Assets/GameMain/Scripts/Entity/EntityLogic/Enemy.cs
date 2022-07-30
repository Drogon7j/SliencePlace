using UnityEngine;
using UnityGameFramework.Runtime;

namespace GameMain
{
    public class Enemy : Entity
    {
        [SerializeField] private EnemyData mEnemyData = null;

        protected override void OnShow(object userData)

        {
            base.OnShow(userData);

            mEnemyData = userData as EnemyData;
            if (mEnemyData == null)
            {
                Log.Error("Effect data is invalid.");
                return;
            }
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
        }
    }
}