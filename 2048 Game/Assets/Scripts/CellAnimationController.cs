using UnityEngine;
using DG.Tweening;

public class CellAnimationController : MonoBehaviour
{
    public static CellAnimationController Instance { get; private set; }

    [SerializeField]
    private CellAnimation animationPref;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;

        DOTween.Init();
    }

    public void SmoothTransition(Cell from, Cell to, bool isMerging)
    {
        Instantiate(animationPref, transform, false).Move(from, to, isMerging);
    }

    public void SmoothAppear(Cell cell)
    {
        Instantiate(animationPref, transform, false).Appear(cell);
    }
}
