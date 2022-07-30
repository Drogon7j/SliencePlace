using UnityEngine;
using UnityGameFramework.Runtime;

namespace GameMain
{
    public class Player : Entity
    {
        [SerializeField] private PlayerData mPlayerData = null;

        protected override void OnShow(object userData)

        {
            base.OnShow(userData);

            mPlayerData = userData as PlayerData;
            if (mPlayerData == null)
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