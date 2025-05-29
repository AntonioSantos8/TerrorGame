using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] PlayerMove playerMove;
    [SerializeField] PlayerStates playerStates;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnAnim(int time)
    {
        StartCoroutine(OnAnimCoroutine(time));
    }
    IEnumerator OnAnimCoroutine(int time)
    {
        playerMove.canMove = false;
        yield return new WaitForSeconds(time);
        playerMove.canMove = true;
        playerStates.ChangeState(States.Default);
    }
}
