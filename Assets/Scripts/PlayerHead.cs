using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHead : MonoBehaviour
{

    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void updateAnimator(Vector3 change){
        animator.SetFloat("moveX", change.x);
        animator.SetFloat("moveY", change.y);
    }
}
