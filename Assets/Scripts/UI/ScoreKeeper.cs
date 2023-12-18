using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.SceneManagement;

public class ScoreKeeper : MonoBehaviour
{
    public static ScoreKeeper Instance;
    public bool bFinal;
    public List<string> LevelGrades;

    private Image outside;
    private Image inside;

    [SerializeField] private Sprite[] grades;
    [SerializeField] private Sprite[] filledGrades;
    // Start is called before the first frame update
    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }else if(Instance != this)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene().name == "End Dialogue")
        {
            outside = GameObject.Find("/UI/Grade").GetComponent<Image>();
            inside = GameObject.Find("/UI/GradeFilled").GetComponent<Image>();
            string grade = GetFinalGrade();
            if (grade == "D")
            {
                outside.sprite = grades[3];
                inside.sprite = filledGrades[3];
            }
            else if (grade == "C")
            {
                outside.sprite = grades[2];
                inside.sprite = filledGrades[2];
            }
            else if (grade == "B")
            {
                outside.sprite = grades[1];
                inside.sprite = filledGrades[1];
            }
            else
            {
                outside.sprite = grades[0];
                inside.sprite = filledGrades[0];
            }
            Destroy(this.gameObject);
        }
    }

    string GetFinalGrade()
    {
        float gradeAsVal = 0;
        int i = 0;
        Image[] finalOutside = new Image[5];
        Image[] finalInside = new Image[5];
        finalOutside[0] = GameObject.Find("/UI/Grades/CafOut").GetComponent<Image>();
        finalOutside[1] = GameObject.Find("/UI/Grades/KitchOut").GetComponent<Image>();
        finalOutside[2] = GameObject.Find("/UI/Grades/OffOut").GetComponent<Image>();
        finalOutside[3] = GameObject.Find("/UI/Grades/HallOut").GetComponent<Image>();
        finalOutside[4] = GameObject.Find("/UI/Grades/YardOut").GetComponent<Image>();

        finalInside[0] = GameObject.Find("/UI/Grades/CafIn").GetComponent<Image>();
        finalInside[1] = GameObject.Find("/UI/Grades/KitchIn").GetComponent<Image>();
        finalInside[2] = GameObject.Find("/UI/Grades/OffIn").GetComponent<Image>();
        finalInside[3] = GameObject.Find("/UI/Grades/HallIn").GetComponent<Image>();
        finalInside[4] = GameObject.Find("/UI/Grades/YardIn").GetComponent<Image>();

        foreach (var grade in LevelGrades)
        {
            switch(grade)
            {
                case "D":
                    gradeAsVal += 1;
                    finalOutside[i].sprite = grades[3];
                    finalInside[i].sprite = filledGrades[3];
                    i++;
                    break;
                case "C":
                    gradeAsVal += 2;
                    finalOutside[i].sprite = grades[2];
                    finalInside[i].sprite = filledGrades[2];
                    i++;
                    break;
                case "B":
                    gradeAsVal += 3;
                    finalOutside[i].sprite = grades[1];
                    finalInside[i].sprite = filledGrades[1];
                    i++;
                    break;
                case "A":
                    gradeAsVal += 4;
                    finalOutside[i].sprite = grades[0];
                    finalInside[i].sprite = filledGrades[0];
                    i++;
                    break;
            }
        }
        
        gradeAsVal /= LevelGrades.Count;
        Debug.Log(gradeAsVal);
        if (gradeAsVal >= 3.5f)
        {
            Debug.Log("A");
            return "A";
        }
        else if (gradeAsVal >= 2.5f)
        {
            Debug.Log("B");
            return "B";
        }
        else if (gradeAsVal >= 1.5f)
        {
            Debug.Log("C");
            return "C";
        }
        else
        {
            Debug.Log("D");
            return "D";
        }

        return "D";
    }

    // public void AddToScore(string score)
    // {
    //     LevelGrades.Add(score);
    // }
}
