using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEditor.SearchService;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour
{
    public GameObject MainPanel;

    public GameObject SudokuFieldPanel;

    public GameObject FieldPrefab;

    public GameObject ControlPanel;

    public GameObject ControlPrefab;

    public Button NotesButton;

    // Start is called before the first frame update
    void Start()
    {
        CreateFieldPrefabs();

        CreateControlPrefabs();

        CreateSudokuobject();

    }

    public void ClickOn_Solve()
    {
        for (int row=0; row<9; row++)
        {
            for(int column=0; column<9; column++)
            {
                FieldPrefabObject fieldObject = _fieldPrefabObjectDic[new Tuple<int, int>(row, column)];
                if (fieldObject.isChangeAble)
                {
                    if (_finalObject.Values[row,column]==fieldObject.Number)
                    {
                        fieldObject.ChangeToGreen();
                    }
                    else
                    {
                        fieldObject.ChangeToRed();
                    }
                }
            }
        }
    }

    public void ClickOn_BackButton()
    {
        SceneManager.LoadScene(0);
    }

    private SudokuObject _gameObject;
    private SudokuObject _finalObject;

    private void CreateSudokuobject()
    {
        SudokuGenerator.CreateSudokuObject(out SudokuObject finalObject, out SudokuObject gameObject);
        _finalObject = finalObject;
        _gameObject = gameObject;
        for (int i = 0; i<9; i++)
        {
            for (int k = 0; k < 9; k++)
            {
                var currentValue = _gameObject.Values[i, k];
                if (currentValue!=0)
                {
                    FieldPrefabObject fieldObject = _fieldPrefabObjectDic[new Tuple<int, int>(i, k)];
                    fieldObject.SetNumber(currentValue);
                    fieldObject.isChangeAble = false;
                }
            }
        }
    }

    private bool IsNotesButtonActive = false;

    public void ClickOn_NotesButton()
    {
        Debug.Log("Clicked on Notes");
        if (IsNotesButtonActive )
        {
            IsNotesButtonActive = false;
            NotesButton.GetComponent<Image>().color = new Color(1f, 1f, 1f);
        }
        else
        {
            IsNotesButtonActive = true;
            NotesButton.GetComponent<Image>().color = new Color(0.7f, 0.99f, 0.99f);
        }
    }


    private Dictionary<Tuple<int, int>, FieldPrefabObject> _fieldPrefabObjectDic = new Dictionary<Tuple<int, int>, FieldPrefabObject>();


    private void CreateFieldPrefabs()
    {
        for (int j = 0; j < 9; j++)
        {
            for (int k = 0; k < 9; k++)
            {
                GameObject instance = GameObject.Instantiate(FieldPrefab, SudokuFieldPanel.transform);
                FieldPrefabObject fieldPrefabObject = new FieldPrefabObject(instance, j, k);
                _fieldPrefabObjectDic.Add(new Tuple<int, int>(j, k), fieldPrefabObject);

                instance.GetComponent<Button>().onClick.AddListener(() => OnClickFieldPrefab(fieldPrefabObject));
            }
        }
    }
    private void CreateControlPrefabs()
    {
        for (int j = 1; j < 10; j++)
        {
            GameObject instance = GameObject.Instantiate(ControlPrefab, ControlPanel.transform);
            instance.GetComponentInChildren<TMPro.TextMeshProUGUI>().text=j.ToString();

            ControlPrefabObject controlPrefabObject = new ControlPrefabObject();
            controlPrefabObject.Number = j;

            instance.GetComponent<Button>().onClick.AddListener(() => ClickOn_ControlPrefab(controlPrefabObject));
        }
    }
    private void ClickOn_ControlPrefab(ControlPrefabObject controlPrefabObject)
    {
        Debug.Log("Clicked on number: " +  controlPrefabObject.Number);
        if(_currentHoveredFieldPrefab != null) 
        { 
            if (IsNotesButtonActive)
            {
                _currentHoveredFieldPrefab.SetSmallNumber(controlPrefabObject.Number);
            }
            else
            {
                _currentHoveredFieldPrefab.SetNumber(controlPrefabObject.Number);

                
            }
            
        }
    }

    private FieldPrefabObject _currentHoveredFieldPrefab;
    private void OnClickFieldPrefab(FieldPrefabObject fieldPrefabObject)
    {
        Debug.Log("Clicked on row:" + fieldPrefabObject.Row + ", column: " + fieldPrefabObject.Column);
        if (fieldPrefabObject.isChangeAble)
        {
            if (_currentHoveredFieldPrefab != null)
            {
                _currentHoveredFieldPrefab.UnsetHoverMode();
            }
            _currentHoveredFieldPrefab = fieldPrefabObject;
            fieldPrefabObject.SetHoverMode();
        }
    }
}
