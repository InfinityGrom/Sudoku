using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SudokuGenerator
{
    public static void CreateSudokuObject(out SudokuObject finalObject, out SudokuObject gameObject)
    {
        _finalSudokuObject = null;
        SudokuObject sudokuObject = new SudokuObject();
        CreateRandomGroups(sudokuObject);
        if (TryToSolve(sudokuObject))
        {
            sudokuObject = _finalSudokuObject;
        }
        else
        {
            throw new Exception("Something went wrong");
        }
        finalObject = sudokuObject;
        gameObject = RemoveSomeRandomNumbers(sudokuObject);
    }

    private static SudokuObject RemoveSomeRandomNumbers(SudokuObject sudokuObject)
    {
        SudokuObject newSudokuObject = new SudokuObject();
        newSudokuObject.Values = (int[,])sudokuObject.Values.Clone();
        List<Tuple<int,int>> values = GetValues();
        int endValueIndex = 10;
        if (Difficulty.Difficulty_Number == 1) { endValueIndex = 61; }
        if (Difficulty.Difficulty_Number == 2) { endValueIndex = 46; }
        bool isFinish = false;
        while (!isFinish)
        {
            int index = UnityEngine.Random.Range(0, values.Count);
            var searchedIndex = values[index];
            //index = UnityEngine.Random.Range(0, values.Count);
            SudokuObject nextSudokuObject = new SudokuObject();
            nextSudokuObject.Values = (int[,])newSudokuObject.Values.Clone();
            nextSudokuObject.Values[searchedIndex.Item1,searchedIndex.Item2] = 0;

            if (TryToSolve(nextSudokuObject, true))
            {
                newSudokuObject = nextSudokuObject;
            }
            values.RemoveAt(index);

            if (values.Count < endValueIndex) 
            {
                isFinish=true;           
            }
        }
        return newSudokuObject;
    }

    private static List<Tuple<int,int>> GetValues()
    {
        List<Tuple<int, int>> values = new List<Tuple<int, int>>();
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                values.Add(new Tuple<int, int>(i,j));
            }
        }
        return values;
    }

    private static SudokuObject _finalSudokuObject;
    private static bool TryToSolve(SudokuObject sudokuObject, bool OnlyOne=false)
    {
        //Find empty fields which can be filled
        if (HasEmptyFieldsToFill(sudokuObject, out int row, out int column, OnlyOne)) 
        {
            List<int> possibleValues=GetPossibleValues(sudokuObject, row, column);
            foreach (var possibleValue in possibleValues)
            {
                SudokuObject nextSudokuObject = new SudokuObject();
                nextSudokuObject.Values = (int[,])sudokuObject.Values.Clone();
                nextSudokuObject.Values[row, column] = possibleValue;
                if (TryToSolve(nextSudokuObject, OnlyOne))
                {
                    return true;
                }
            }
        }

        //Does sudokuObject has empty fields
        if (HasEmptyFields(sudokuObject))
        {
            return false;
        }
        return true;

        //finish
    }
    private static bool HasEmptyFields(SudokuObject sudokuObject)
    {
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                if (sudokuObject.Values[i, j] == 0)
                {
                    return true;
                }
            }
        }
        _finalSudokuObject= sudokuObject;
        return false;
    }

    private static List<int> GetPossibleValues(SudokuObject sudokuObject, int row, int column)
    {
        List<int> possibleValues = new List<int>();
        for (int i = 1; i < 10; i++)
        {
            if(sudokuObject.IsPossibleNumberInPosition(i,row, column))
            {
                possibleValues.Add(i);
            }
        }
        return possibleValues;
    }

    private static Boolean HasEmptyFieldsToFill(SudokuObject sudokuObject, out int row, out int column, bool OnlyOne = false)
    {
        row = 0; 
        column = 0;
        int possibleValues = 10;
        for (int i = 0; i<9; i++)
        {
            for (int j=0; j<9; j++)
            {
                if (sudokuObject.Values[i,j]==0)
                {
                    int currentAmount = GetPossibleAmountOfValues(sudokuObject, i, j);
                    if (currentAmount!=0)
                    {
                        if(currentAmount<possibleValues)
                        {
                            possibleValues= currentAmount;
                            row= i;
                            column= j;
                        }
                    }
                }
            }
        }
        if (OnlyOne)
        {
            if(possibleValues==1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        if (possibleValues==10)
        {

            return false;
        }
        else
        {
            return true;
        }
    }

    private static int GetPossibleAmountOfValues(SudokuObject sudokuObject, int row, int column)
    {
        int amount = 0;
        for (int k = 1; k < 10; k++)
        {
            if (sudokuObject.IsPossibleNumberInPosition(k, row, column))
            {
                amount++;
            }
        }
        return amount;
    }

    public static void CreateRandomGroups(SudokuObject sudokuObject)
    {
        List<int> values = new List<int>(){ 0,1,2};
        int index = UnityEngine.Random.Range(0, values.Count);
        InsertRandomGroup(sudokuObject, 1 + values[index]);
        values.RemoveAt(index);

        index = UnityEngine.Random.Range(0, values.Count);
        InsertRandomGroup(sudokuObject, 4 + values[index]);
        values.RemoveAt(index);

        index = UnityEngine.Random.Range(0, values.Count);
        InsertRandomGroup(sudokuObject, 7 + values[index]);
        values.RemoveAt(index);
    }
    public static void InsertRandomGroup(SudokuObject sudokuObject, int group)
    {
        sudokuObject.GetGroupIndex(group, out int startRow, out int  startColumn);
        List <int> values = new List<int>() { 1,2,3,4,5,6,7,8,9 };
        for (int i=startRow; i<startRow+3; i++) 
        {
            for (int k=startColumn; k<startColumn+3; k++)
            {
                int index = UnityEngine.Random.Range(0, values.Count);
                sudokuObject.Values[i, k] = values[index];
                values.RemoveAt(index);
            }
        }
    }
}
