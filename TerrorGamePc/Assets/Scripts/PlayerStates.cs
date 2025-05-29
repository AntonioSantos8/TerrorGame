using UnityEngine;
using UnityEngine.Events;

public enum States
{
    Default,
    Animation
}

public class PlayerStates : MonoBehaviour
{
    public States currentState;
    public UnityEvent OnAnim, OnDefault;

    private States lastState;

    void Start()
    {
        ChangeState(States.Animation); 
    }

    void Update()
    {
        if (currentState != lastState)
        {
            switch (currentState)
            {
                case States.Animation:
                    OnAnim?.Invoke();
                    break;
                case States.Default:
                    OnDefault?.Invoke();
                    break;
            }

            lastState = currentState;
        }
    }

    public void ChangeState(States newState)
    {
        currentState = newState;
    }
}
