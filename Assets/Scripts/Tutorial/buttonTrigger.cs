using UnityEngine;

namespace LevelTutorial
{
    public class buttonTrigger : MonoBehaviour
    {
        [SerializeField] private Sprite spOpen;
        [SerializeField] private Light btnLight;
        public GameObject hint;
        [SerializeField] private GameObject error;

        // Update is called once per frame
        void OnTriggerEnter(Collider col)
        {
            if (col.tag == "Player Hitbox")
            {
                GetComponent<SpriteRenderer>().sprite = spOpen;
                btnLight.color = Color.green;
                hint.SetActive(false);
                error.SetActive(false);
                LockDoorBehaviour.Instance.OnOpenDoor();
            }
            else if(col.tag == "Player")
            {
                error.SetActive(true);
                Destroy(col.gameObject);
            }
        }
    }
}