using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelTrigger : MonoBehaviour
{
    void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Player")
        {
            //ScoreKeeper.Instance.AddToScore(Score.Instance.grade);
            if(ScoreKeeper.Instance!= null) 
                ScoreKeeper.Instance.LevelGrades.Add(Score.Instance.grade);
            transform.GetChild(0).GetComponent<NextScene>().FadeToLevel(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}