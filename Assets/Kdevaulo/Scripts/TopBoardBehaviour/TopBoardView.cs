using TMPro;

using UnityEngine;
using UnityEngine.UI;

namespace Kdevaulo.SpaceInvaders.TopBoardBehaviour
{
    [AddComponentMenu(nameof(TopBoardView) + " in " + nameof(TopBoardBehaviour))]
    public sealed class TopBoardView : MonoBehaviour
    {
        [SerializeField] private Button _pauseButton;

        [SerializeField] private TextMeshProUGUI _scoreHolder;
    }
}