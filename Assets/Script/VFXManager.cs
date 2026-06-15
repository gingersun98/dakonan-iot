using System.Collections;
using UnityEngine;

public class VFXManager : MonoBehaviour
{
    public static VFXManager instance {  get; private set; }
    public VFX[] listOfVFX;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        } else
        {
            Destroy(this);
        }
    }

    public GameObject PlayVFX(string name)
    {
        VFX chosen = null;
        foreach (VFX vfx in listOfVFX)
        {
            if (vfx.name == name)
            {
                chosen = vfx;
                break;
            }
        }
        if (chosen != null)
        {
            GameObject newVFX = chosen.pool.GetObject();
            newVFX.transform.SetParent(chosen.pool.transform);
            StartCoroutine(StartTimeLimit(newVFX, chosen));
            return newVFX;
        } else
        {
            print(name + " doesn't exist in the VFX library.");
            return null;
        }
    }

    IEnumerator StartTimeLimit(GameObject vfx, VFX chosen)
    {
        yield return new WaitForSeconds(chosen.timeLimit);
        chosen.pool.ReturnObject(vfx);

    }
}

[System.Serializable]
public class VFX
{
    public string name;
    public float timeLimit;
    public ObjectPooling pool; 
}
