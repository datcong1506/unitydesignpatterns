using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;
[Serializable]
public abstract class AbEventBehaviour : MonoBehaviour
{
    public abstract ResponseEvent TriggerEvent(GameEvent gameEvent);
}




[Serializable]
public abstract class AbStatePatternIFSM : AbEventBehaviour
{
    public AbStatePatternState initialState;


    [Header("For Debug")]
    public AbStatePatternState currentState;


    [SerializeField]
    public List<AbEventBehaviour> onChangeStateListenerInInspector = new List<AbEventBehaviour>();
    // private Dictionary<AbEventBehaviour, AbEventBehaviour> onChangeStateListener;


    public virtual void Start()
    {
        if (initialState != null)
        {
            Transistion(initialState);
        }
    }

    public abstract ResponseEvent ChangeState(GameEventChangeStateEvent gameEventChangeStateEvent);


    public virtual void Transistion(AbStatePatternState iState)
    {
        if (iState != currentState)
        {
            currentState?.OnExitState();
            InVokeAllChangeStateRegister(iState);
            this.currentState = iState;
            currentState.OnEnterState();
        }
    }


    private void InVokeAllChangeStateRegister(AbStatePatternState newState)
    {

        GameEventChangeStateEvent changeStateEvent = new GameEventChangeStateEvent();
        changeStateEvent.origin = gameObject;
        changeStateEvent.oldState = currentState;
        changeStateEvent.newState = newState;

        for (int i = 0; i < onChangeStateListenerInInspector.Count; i++)
        {
            onChangeStateListenerInInspector[i].TriggerEvent(changeStateEvent);
        }
    }
    public void RemoveRegisterFromListener(AbEventBehaviour abEventBehaviour)
    {
        for (int i = 0; i < onChangeStateListenerInInspector.Count; i++)
        {
            if (onChangeStateListenerInInspector[i] == abEventBehaviour)
            {
                onChangeStateListenerInInspector.RemoveAt(i);
                return;
            }
        }
    }
    public void AddRegisterToListener(AbEventBehaviour abEventBehaviour)
    {
        RemoveRegisterFromListener(abEventBehaviour);
        onChangeStateListenerInInspector.Add(abEventBehaviour);
    }
}

[RequireComponent(typeof(AbStatePatternIFSM))]
[Serializable]
public abstract class AbStatePatternState : AbEventBehaviour
{
    [HideInInspector]
    public AbStatePatternIFSM abStatePatternIFSM;



    public List<Decistion> decistionChangeState;


    protected bool canChangeStateNow = true;

    [Serializable]
    public class Decistion
    {
        public AbStatePatternState abStatePatternState;
        public bool overWrite = true;
    }

    public virtual void Awake()
    {
        abStatePatternIFSM = GetComponent<AbStatePatternIFSM>();
        canChangeStateNow = true;
    }

    public abstract void OnEnterState();

    ///<sumary>
    /// u should put base.onUpdate
    ///</sumary>
    public abstract void OnUpdateState();

    public abstract void OnExitState();



    public void CheckChangeState()
    {
        foreach (var des in decistionChangeState)
        {
            if (!des.overWrite && !canChangeStateNow) continue;
            var change = des.abStatePatternState.DescistionToThisState();
            if (change)
            {
                var changeStateEvent = new GameEventChangeStateEvent();
                changeStateEvent.newState = des.abStatePatternState;
                abStatePatternIFSM.ChangeState(changeStateEvent);
                return;
            }
        }
    }


    public abstract bool DescistionToThisState();
}

[Serializable]
public abstract class AbStatePatternDependencyState : AbEventBehaviour
{
    public virtual void Awake()
    {
        GetComponent<AbStatePatternIFSM>().AddRegisterToListener(this);
    }


    public virtual void OnEnable()
    {
        GetComponent<AbStatePatternIFSM>().AddRegisterToListener(this);
    }
    public virtual void OnDisable()
    {
        GetComponent<AbStatePatternIFSM>().RemoveRegisterFromListener(this);
    }


}

[Serializable]
public abstract class AbStatePatternDependencyStateSelf : AbStatePatternDependencyState
{

}


// [Serializable]
// public abstract class AbStatePatternDependencyStateOther : AbStatePatternDependencyState
// {
//     public GameObject target;
//     private void Start()
//     {
//         if (target == null)
//         {
//             Debug.LogError("Required gameobject target");
//         }
//     }
//     public override void OnEnable()
//     {
//         target.GetComponent<AbStatePatternIFSM>().onChangeStateEventListenner.AddListener(OnStateChange);
//     }
//     public override void OnDisable()
//     {
//         target.GetComponent<AbStatePatternIFSM>().onChangeStateEventListenner.RemoveListener(OnStateChange);
//     }
// }

// [Serializable]
// [RequireComponent(typeof(AbStatePatternIFSM))]
// public class AbStatePatternDecistionChangeState : EventBehaviour
// {
//     public override ResponseEvent TriggerEvent(GameEvent gameEvent)
//     {
//         throw new NotImplementedException();
//     }
// }

public abstract class GameEvent
{
    // public GameEventInfo gameEventInfo;
    public GameObject origin;

}

public class GameEventChangeStateEvent : GameEvent
{
    public AbStatePatternState newState;
    public AbStatePatternState oldState;
}

public class ResponseEvent
{

}
