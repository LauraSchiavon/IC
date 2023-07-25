using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

namespace JigsawPuzzlesCollection.Scripts
{
    public class PieceScript : MonoBehaviour
    {
        private Vector3 m_rightPosition;
        private SortingGroup m_sortingGroup;

        public bool InRightPosition;
        public bool Selected;

        public bool IsMovable;

        void Start()
        {
            m_rightPosition = transform.localPosition;
            m_sortingGroup = GetComponent<SortingGroup>();

            StartCoroutine(TweenToRandomPosition());
        }

        public void MoveToRightPosition()
        {
            SoundManager.Instance.PlaySound(SoundManager.Instance.ShufflePieces);
            StartCoroutine(TweenToPosition(transform.localPosition, m_rightPosition));
        }

        public void MoveToRandomPosition()
        {
            var startX = ((int)(Random.Range(3f, 8f) * 5)) / 5f;
            var startY = ((int)(Random.Range(4f, -4f) * 5)) / 5f;
            var destinationPosition = new Vector3(startX, startY, 0);

            StartCoroutine(TweenToPosition(transform.localPosition, destinationPosition));
        }

        IEnumerator TweenToRandomPosition()
        {
            var startX = ((int)(Random.Range(3f, 8f) * 5)) / 5f;
            var startY = ((int)(Random.Range(4f, -4f) * 5)) / 5f;
            var destinationPosition = new Vector3(startX, startY, 0);

            yield return new WaitForSeconds(1f);
            SoundManager.Instance.PlaySound(SoundManager.Instance.ShufflePieces);

            var tweenEnumerator = TweenToPosition(m_rightPosition, destinationPosition);
            while (tweenEnumerator.MoveNext())
            {
                yield return tweenEnumerator.Current;
            }

            IsMovable = true;
        }

        IEnumerator TweenToPosition(Vector3 startPosition, Vector3 destinationPosition, float duration = 1.0f)
        {
            float time = 0.0f;
            do
            {
                time += Time.deltaTime;
                transform.localPosition = Vector3.Lerp(startPosition, destinationPosition, time / duration);
                yield return 0;
            } while (time < duration);
        }

        void Update()
        {
            if (!IsMovable)
            {
                return;
            }

            if (Vector3.Distance(transform.localPosition, m_rightPosition) < 0.5f)
            {
                if (!Selected)
                {
                    if (InRightPosition == false)
                    {
                        transform.localPosition = m_rightPosition;
                        InRightPosition = true;
                        m_sortingGroup.sortingOrder = 1;
                    }
                }
            }
        }
    }
}
