using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breakable : MonoBehaviour
{
    private AudioControl SFX;
    private static readonly int OutlineWidth = Shader.PropertyToID("_OutlineWidth");
    protected Material Mat;
    protected SpriteRenderer Sr;
    [SerializeField] int componentNumber;

    [SerializeField] private List<GameObject> componentItems;

    private void Awake()
    {
        Sr = GetComponent<SpriteRenderer>();
        Mat = Sr.material;
        OnNotSelected();
    }

    // Start is called before the first frame update
    void Start()
    {
        SFX = GameObject.Find("AudioController").GetComponent<AudioControl>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Item is selected and can be destroyed
    /// </summary>
    public virtual void OnSelected()
    {
        Mat.SetFloat(OutlineWidth, 5);
    }

    public virtual void OnNotSelected()
    {
        Mat.SetFloat(OutlineWidth, 0);
    }

    public void OnHit()
    {
        SFX.PlayHit();
        Debug.Log("breaking");
        for(int i = 0; i < componentNumber; i++)
        {
            Vector3 instPos = transform.position;
            instPos.x += Random.Range(-5.0f, 5.0f);
            instPos.y = 1.45f;
            foreach(GameObject ins in componentItems)
                Instantiate(ins, instPos, transform.rotation);
        }
            
        Destroy(gameObject);
    }

    
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Player") &&  GameObject.Find("[Player]/PlayerSprites/Player Hitbox").GetComponent<Collider>().enabled)
        {
            Debug.Log("here");
            OnHit();
        }
            

    }

}
