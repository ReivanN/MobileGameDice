using UnityEngine;

public class LoaderPrefabs : MonoBehaviour
{
    [Header("Префабы")]
    [SerializeField] private  GameObject[] prefabs;
    [SerializeField] private  GameObject[] prefabsred;

    [Header("Точки появления")]
    [SerializeField] private  GameObject BluePoint;
    [SerializeField] private  GameObject RedPoint;


    public void Start1() 
    {
        if (BluePoint != null && prefabs.Length > 0 && prefabs[0] != null)
        {
            Instantiate(prefabs[0], BluePoint.transform.position, prefabs[0].transform.rotation);
        }
    }

    public void Start1_1()
    {
        if (BluePoint != null && prefabs.Length > 0 && prefabs[1] != null)
        {
            Instantiate(prefabs[1], BluePoint.transform.position, prefabs[1].transform.rotation);
        }
    }

    public void Start2()
    {
        if (RedPoint != null && prefabsred.Length > 0 && prefabsred[0] != null)
        {
            Instantiate(prefabsred[0], RedPoint.transform.position, prefabsred[0].transform.rotation);
        }
    }

    public void Start2_2()
    {
        if (RedPoint != null && prefabsred.Length > 0 && prefabsred[1] != null)
        {
            Instantiate(prefabsred[1], RedPoint.transform.position, prefabsred[1].transform.rotation);
        }
    }

    public void Start3() 
    {
        if (BluePoint != null && prefabs.Length > 0 && prefabs[2] != null)
        {
            Instantiate(prefabs[2], BluePoint.transform.position, prefabs[2].transform.rotation);
        }
    }

    public void Start3_3()
    {
        if (RedPoint != null && prefabs.Length > 0 && prefabsred[2] != null)
        {
            Instantiate(prefabsred[2], RedPoint.transform.position, prefabsred[2].transform.rotation);
        }
    }
}
