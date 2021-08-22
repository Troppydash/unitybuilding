using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WalkingScript : MonoBehaviour
{
    // editor settings
    public float moveSpeed = 10f;
    public List<GameObject> places;
    public GameObject linePrefab;

    private List<Vector3> positions;
    private int positionIndex;
    private int isReverse;

    void Start()
    {
        // get a list of positions
        positions = new List<Vector3>( places.Select( place => place.transform.position ).ToArray() );
        positions.Insert( 0, transform.position );

        isReverse = -1;
        positionIndex = 0;
        positionIndex = NextPlace();

        if ( linePrefab )
        {
            GenerateGuideLines();
        }
    }

    private void GenerateGuideLines()
    {
        int count = positions.Count;
        for ( int i = 0; i < count - 1; ++i )
        {
            GameObject guideLine = Instantiate( linePrefab );
            guideLine.GetComponent<LineRenderer>().SetPositions( new []
            {
                positions[i] + Vector3.down, positions[i+1] + Vector3.down
            } );
        }
    }

    private int NextPlace()
    {
        if ( positionIndex + 1 >= positions.Count || positionIndex == 0)
        {
            isReverse = -isReverse;
        }
        return positionIndex + isReverse;
    }

    void Update()
    {
        Vector3 next = positions[positionIndex];
        transform.position = Vector3.MoveTowards(transform.position, next, moveSpeed * Time.deltaTime );
        if ( Vector3.Distance( transform.position, next) < 0.1)
        {
            positionIndex = NextPlace();
        }
    }
}