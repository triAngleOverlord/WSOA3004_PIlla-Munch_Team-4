using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolutionController : MonoBehaviour
{
    private SolutionNode[] _solutionNodes;

    private float timecounter = 0;
    private float timeInterval = 2;
    public static SolutionController Instance;
    public List<GameObject> _blocks;

    private bool checking = false;

    private float timercounter = 0;

    private float timeout = 2f;
    // Start is called before the first frame update
    void Start()
    {
        _solutionNodes = GetComponentsInChildren<SolutionNode>();
    }
    private void Awake()
    {
        if (Instance!= null)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    private bool CheckSolution()
    {
        var correct = false;
        foreach (var solution in _solutionNodes)
        {
            solution.Validate(!solution.ShouldContainABlock);
        }

        foreach (var block in _blocks)
        {
           
            if (block != null)
            {
                 
                foreach (var solutionNode in _solutionNodes)
                {
                    var type = solutionNode.ShouldContainABlock;
                    if (solutionNode.ShouldContainABlock &&
                        solutionNode.collider.bounds.Contains(block.transform.position))
                    {
                        solutionNode.Validate(type);
                    }
                    else if (!solutionNode.ShouldContainABlock&&solutionNode.collider.bounds.Contains(block.transform.position))
                    {
                        solutionNode.Validate(type);
                    }
                }
            }
        }

        foreach (var solnode in _solutionNodes)
        {
            if (!solnode.valid)
            {
                return false;
            }
        }

        return true;
    }

    // Update is called once per frame
    void Update()
    {
        var correct =CheckSolution();
        if (correct)
        {
            checking = true;
            
        }
        else
        {
            checking = false;
            timecounter = 0;
        }

        if (checking)
        {
            timecounter += Time.deltaTime;
            if (timecounter>= timeout)
            {
                Debug.Log("yaay");
                Time.timeScale = 0;
            }
        }
    }

    private IEnumerator CheckSol()
    {
        CheckSolution();
        yield return new WaitForSeconds(0.2f);
    }
}
