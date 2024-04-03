using Zenject;

namespace Kdevaulo.SpaceInvaders.ScoreBehaviour
{
    public sealed class ScoreService
    {
        [Inject]
        private ScoreView _scoreView;

        private int _currentScore;

        public void Add(int score)
        {
            _currentScore += score;
            _scoreView.SetScore(_currentScore);
        }

        public void Clear()
        {
            _currentScore = 0;
            _scoreView.SetScore(_currentScore);
        }
    }
}