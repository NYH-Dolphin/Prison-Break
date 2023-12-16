using UnityEngine;

namespace LevelTutorial
{
    public class buttonTrigger : MonoBehaviour
    {
        [SerializeField] private Sprite spOpen;
        [SerializeField] private Light btnLight;
        [SerializeField] private GameObject hint;

        // Update is called once per frame
        void OnTriggerEnter(Collider col)
        {
            if (col.tag == "Player Hitbox")
            {
                GetComponent<SpriteRenderer>().sprite = spOpen;
                btnLight.color = Color.green;
                hint.SetActive(false);
                LockDoorBehaviour.Instance.OnOpenDoor();
            }
        }
    }
}