using System;

using UnityEngine;

namespace Kdevaulo.SpaceInvaders.DropBehaviour
{
    [Serializable]
    public sealed class DropItem
    {
        [field: SerializeField] public Drop DropType;
        [field: SerializeField] public DropView View;
    }
}