using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContentController : MonoBehaviour
{
    [SerializeField] GameObject rowObject;
    [SerializeField] GameObject itemPrefab;

    public void SpawRow(int numberOfRow)
    {
        for(int i = 0; i<numberOfRow; i++)
        {
            GameObject row = Instantiate(rowObject, Vector3.zero, Quaternion.identity, transform);
            row.GetComponent<RowController>().indexOfRow = i;
        }
    }

    public void SpawItem(char value, int indexRow, int indexCol)
    {
        GameObject temp = transform.GetChild(indexRow).gameObject;
        GameObject item = Instantiate(itemPrefab, Vector3.zero, Quaternion.identity, temp.transform);
        item.GetComponent<ItemController>().UpdatePos(indexRow, indexCol);
        item.GetComponent<ItemController>().SetID(value);
    }

    public void SpawItems(int numberOfCol, int numberOfRow, int currentNumber)
    {
        SpawRow(numberOfRow);

        for (int i = 0; i < numberOfRow; i++)
        {
            for(int j = 0; j < numberOfCol; j++)
            {
                SpawItem('s', i, j);
            }
        }
    }

    public void UpdateItems(int rowArr, int colArr, int currentNumber)
    {
        int randomIndex = Random.Range(0, rowArr*colArr);

        int value = Random.Range(0, 10);
        string alphabets = GameController.Instance.alphabets;
        while (alphabets[value] == alphabets[currentNumber])
        {
            value = Random.Range(0, 10);
        }

        for (int i = 0; i < rowArr; i++)
        {
            for (int j = 0; j < colArr; j++)
            {
                UpdateValueOfItem(i, j, alphabets[value]);
            }
        }

        int row = randomIndex / colArr;
        int col = randomIndex - row * colArr;
        UpdateValueOfItem(row, col, alphabets[currentNumber]);
    }

    public void UpdateValueOfItem(int row, int col, char value)
    {
        GameObject rowParent = transform.GetChild(row).gameObject;
        GameObject colParent = rowParent.transform.GetChild(col).gameObject;

        colParent.GetComponent<ItemController>().SetID(value);
    }

    public void HideItem(int row, int col)
    {
        GameObject rowParent = transform.GetChild(row).gameObject;
        GameObject colParent = rowParent.transform.GetChild(col).gameObject;

        colParent.GetComponent<ItemController>().Hide(true);
    }

    public void Notice(int row, int col)
    {
        GameObject rowParent = transform.GetChild(row).gameObject;
        GameObject colParent = rowParent.transform.GetChild(col).gameObject;

        colParent.GetComponent<ItemController>().Noticing();
    }

    public void UnTicked(int row, int col)
    {
        GameObject rowParent = transform.GetChild(row).gameObject;
        GameObject colParent = rowParent.transform.GetChild(col).gameObject;

        colParent.GetComponent<ItemController>().UnTicked();
    }

    public void ChangeSibling(int row, int col, int newRow, int newIndex)
    {
        GameObject rowParent = transform.GetChild(row).gameObject;
        GameObject colParent = rowParent.transform.GetChild(col).gameObject;

        colParent.transform.SetParent(transform.GetChild(newRow));
        colParent.transform.SetSiblingIndex(newIndex);
        //colParent.GetComponent<ItemController>().UpdatePos(newRow, newIndex);
    }

    public void UpdatePosItems()
    {
        for(int i = 0; i < transform.childCount; i++)
        {
            for(int j = 0; j < transform.GetChild(i).transform.childCount; j++)
            {
                transform.GetChild(i).transform.GetChild(j).GetComponent<ItemController>().UpdatePos(i, j);
            }
        }
    }

    public void Reset()
    {
        for(int i = transform.childCount - 1; i >= 0; i--)
        {
            GameObject temp = transform.GetChild(i).gameObject;
            temp.transform.SetParent(null);
            Destroy(temp);
        }
    }
}
