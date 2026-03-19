using System.Collections.Generic;
using UnityEngine;

public class PoolObjetos : MonoBehaviour
{
    [SerializeField] private GameObject bolaPrefab;
    [SerializeField] private int tamanoInicial = 10;

    private List<GameObject> poolBolas;

    void Awake()
    {
        poolBolas = new List<GameObject>();

        for (int i = 0; i < tamanoInicial; i++)
        {
            CrearNuevoObjeto();
        }
    }

    public GameObject CrearNuevoObjeto()
    {
        GameObject obj = Instantiate(bolaPrefab);
        obj.SetActive(false);
        poolBolas.Add(obj);
        return obj;
    }

    public GameObject CogerObjeto()
    {
        GameObject obj = poolBolas.Find(x => !x.activeInHierarchy);

        if (obj == null)
        {
            obj = CrearNuevoObjeto();
        }

        obj.SetActive(true);
        return obj;
    }
}
