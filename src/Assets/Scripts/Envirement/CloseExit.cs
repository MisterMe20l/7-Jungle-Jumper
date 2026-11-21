using UnityEngine;

public class CloseExit : MonoBehaviour, IStartFight
{
    [SerializeField] GameObject m_doorPrefab;
    [SerializeField] Transform m_doorSpownPoint;

    GameObject door;

    public void StartFight()
    {
        door = Instantiate(m_doorPrefab, m_doorSpownPoint.position, Quaternion.Euler(Vector2.zero));
    }

    public void RestartFight()
    {
        Destroy(door);
    }
}
