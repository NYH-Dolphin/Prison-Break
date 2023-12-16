using System;
using System.Collections.Generic;
using MyCameraEffect;
using UnityEngine;
using Random = UnityEngine.Random;

public class Breakable : MonoBehaviour
{
    private AudioControl SFX;
    private GameObject player;
    private static readonly int OutlineWidth = Shader.PropertyToID("_OutlineWidth");
    protected Material Mat;
    protected SpriteRenderer Sr;
    [SerializeField] public GameObject breakEffectPrefab;

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
        player = GameObject.Find("[Player]");
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
        for (int i = 0; i < componentNumber; i++)
        {
            foreach (GameObject ins in componentItems)
            {
                Vector3 instPos = player.transform.position;
                instPos.x += Random.Range(-2.5f, 2.5f);
                instPos.z += Random.Range(-2f, 2f);
                instPos.y = 1.45f;
                Instantiate(ins, instPos, transform.rotation);
            }
        }
        AudioControl.Instance.PlayHit();
        Destroy(gameObject);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player") &&
            GameObject.Find("[Player]/PlayerSprites/Player Hitbox").GetComponent<Collider>().enabled)
        {
            CameraEffect.Instance.GenerateMeleeImpulse();
            OnHit();
        }
    }

    private void OnDestroy()
    {
        var breakEffect = Instantiate(breakEffectPrefab);
        breakEffect.transform.position = transform.position;
    }
}