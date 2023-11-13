using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fusion : MonoBehaviour
{
    private Dictionary<string, string> fusionRecipes;

    public GameObject hammer;
    public GameObject spikedBat;
    public GameObject axe;

    // Start is called before the first frame update
    void Start()
    {
        InitializeFusionRecipes();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public GameObject AttemptFusion(string weapon1_name, string weapon2_name) {
        string fusionKey1 = weapon1_name + "+" + weapon2_name;
        string fusionKey2 = weapon2_name + "+" + weapon1_name;
        Debug.Log(fusionKey1);
        Debug.Log(fusionKey2);
        // Check if these two weapons have a fusion; if they do, re;turn it
        if (fusionRecipes.ContainsKey(fusionKey1)) {
            if(fusionRecipes[fusionKey1] == "hammer") {
                return hammer;
            } else if(fusionRecipes[fusionKey1] == "spikedBat") {
                return spikedBat;
            } else if(fusionRecipes[fusionKey1] == "axe") {
                return axe;
            }        }
        if (fusionRecipes.ContainsKey(fusionKey2)) {
            if(fusionRecipes[fusionKey2] == "hammer") {
                return hammer;
            } else if(fusionRecipes[fusionKey2] == "spikedBat") {
                return spikedBat;
            } else if(fusionRecipes[fusionKey2] == "axe") {
                return axe;
            }     
        }
        // otherwise return null
        return null;
    }

    private void InitializeFusionRecipes() {
        fusionRecipes = new Dictionary<string, string> {
            {"dumbbell+wooden_plank", "hammer"},
            {"glass_shard+metal_pipe", "axe"},
            {"glass_shard+wood_bat", "spikedBat"}
        };
    }
}
