using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModeTitle : MonoBehaviour
{

    private DataSupplier dataSupplier;

    // Start is called before the first frame update
    void Start()
    {
        dataSupplier = GameObject.FindObjectOfType<DataSupplier>();

        dataSupplier.onDataSourceChanged.AddListener(OnDataSourceChanged);

        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    bool IsHistoricalDataSource(DataSource dataSource)
    {
        if(dataSource.type == "db" || dataSource.type == "file" || dataSource.type == "generated")
        {
            return true;
        }
        return false;
    }

    void OnDataSourceChanged(DataSourceChangedEvent e)
    {
        /*if(IsHistoricalDataSource(e.dataSource))
        {
            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
        }*/
    }
}
