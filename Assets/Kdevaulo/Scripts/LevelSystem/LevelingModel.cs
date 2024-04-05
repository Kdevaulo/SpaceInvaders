using UniRx;

namespace Kdevaulo.SpaceInvaders.LevelngSystem
{
    public sealed class LevelingModel
    {
        public ReactiveProperty<LevelSettingsData> LevelSettings = new ReactiveProperty<LevelSettingsData>();

        public ReactiveCommand ClearLevel = new ReactiveCommand();
    }
}