using UnityEngine.UI;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class CellAnimation : MonoBehaviour
{
    [SerializeField]
    private Image image;
    [SerializeField]
    private TextMeshProUGUI points;

    private float moveTime = .1f;
    private float appearTime = .1f;

    private Sequence sequence;

    public void Move(Cell from, Cell to, bool isMerging)
    {
        from.CancelAnimation();
        to.SetAnimation(this);

        image.color = ColorManager.Instance.CellColors[from.Value];
        points.text = from.Points.ToString();
        points.color = from.Value <= 2 ? 
            ColorManager.Instance.PointsDarkColor : 
            ColorManager.Instance.PointsLightColor;

        transform.position = from.transform.position;

        sequence = DOTween.Sequence();

        sequence.Append(transform.DOMove(to.transform.position, moveTime).SetEase(Ease.InOutQuad));

        if (isMerging)
        {
            sequence.AppendCallback(() =>
            {
                image.color = ColorManager.Instance.CellColors[to.Value];
                points.text = to.Points.ToString();
                points.color = to.Value <= 2 ?
                    ColorManager.Instance.PointsDarkColor :
                    ColorManager.Instance.PointsLightColor;
            });

            sequence.Append(transform.DOScale(1.2f, appearTime));
            sequence.Append(transform.DOScale(1, appearTime));
        }

        sequence.AppendCallback(() =>
        {
            to.UpdateCell();
            Destroy();
        });
    }

    public void Appear(Cell cell)
    {
        cell.CancelAnimation();
        cell.SetAnimation(this);

        image.color = ColorManager.Instance.CellColors[cell.Value];
        points.text = cell.Points.ToString();
        points.color = cell.Value <= 2 ?
            ColorManager.Instance.PointsDarkColor :
            ColorManager.Instance.PointsLightColor;

        transform.position = cell.transform.position;
        transform.localScale = Vector2.zero;

        sequence = DOTween.Sequence();

        sequence.Append(transform.DOScale(1.2f, appearTime * 2));
        sequence.Append(transform.DOScale(1f, appearTime * 2));
        sequence.AppendCallback(() =>
        {
            cell.UpdateCell();
            Destroy();
        });
    }

    public void Destroy()
    {
        sequence.Kill();
        Destroy(gameObject);
    }
}
