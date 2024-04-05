using UnityEngine;

namespace Kdevaulo.SpaceInvaders.ProjectileBehaviour
{
    [CreateAssetMenu(fileName = nameof(ProjectileSettingsData),
        menuName = nameof(ProjectileBehaviour) + "/" + nameof(ProjectileSettingsData))]
    public sealed class ProjectileSettingsData : ScriptableObject
    {
        [field: SerializeField] public float MoveDelay { get; private set; }
        [field: Min(0.00001f)]
        [field: SerializeField] public float MoveStepDivider { get; private set; }
    }
}