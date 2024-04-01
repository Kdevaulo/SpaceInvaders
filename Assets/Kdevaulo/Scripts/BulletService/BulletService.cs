using System;

using UnityEngine;

using Zenject;

namespace Kdevaulo.SpaceInvaders.BulletService
{
    public sealed class BulletService : ITickable, IPauseHandler
    {
        public void AddBullet(Vector2 direction, float speed)
        {
        }

        void ITickable.Tick()
        {
        }

        void IPauseHandler.HandlePause()
        {
            throw new NotImplementedException();
        }

        void IPauseHandler.HandleResume()
        {
            throw new NotImplementedException();
        }
    }
}