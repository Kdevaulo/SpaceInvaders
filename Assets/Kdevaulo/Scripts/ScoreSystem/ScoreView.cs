using TMPro;

using UnityEngine;

namespace Kdevaulo.SpaceInvaders.ScoreSystem
{
    [AddComponentMenu(nameof(ScoreView) + " in " + nameof(ScoreSystem))]
    public sealed class ScoreView : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private TextMeshProUGUI _scoreHolder;

        public void SetScore(int score)
        {
            _scoreHolder.text = score.ToString();
        }
    }
}