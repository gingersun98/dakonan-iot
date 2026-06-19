using UnityEngine;

[System.Serializable]
public class AnimationEvent
{
    public enum TypeOfEvent { Disable, Enable} // Modify this enum as much as you need.
    public TypeOfEvent eventType;
    public GameObject[] target;
}
public class AnimationEvents : MonoBehaviour
{
    public AnimationEvent[] events;
    void ActivateEvent(AnimationEvent selected) // Add the effects in your types here...
    {
        switch (selected.eventType)
        {
            case AnimationEvent.TypeOfEvent.Disable:
                foreach (GameObject target in selected.target)
                {
                    target.SetActive(false);
                }
                break;
            case AnimationEvent.TypeOfEvent.Enable:
                foreach (GameObject target in selected.target)
                {
                    target.SetActive(true);
                }
                break;
        }
    }

    public void StartEvent(int index)
    {
        ActivateEvent(events[index]);
    }

    public void StartAllEvent()
    {
        foreach (AnimationEvent anim in  events)
        {
            ActivateEvent(anim);
        }
    }
    
}

