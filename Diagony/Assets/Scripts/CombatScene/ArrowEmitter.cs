using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArrowEmitter : MonoBehaviour
{
    
    // Public Atributes
    public GameObject ArrowHeadPrefab; // The prefab of arrow head
    public GameObject ArrowNodePrefab; // The prefab of arrow node
    public int ArrowNodeNum;           // The number of arrow nodes
    public float scaleFactor = 1f;     // The scale multiplier for arrow nodes
    public List<GameObject> Enemies;
    //public bool OverEnemy = false;

    // Private Atribures
    //private RectTransform origin;                                                                                                 // The position of P0 (The arrows emitter point)
    private Transform origin;
    //private List<RectTransform> arrowNodes = new List<RectTransform>();                                                           // The list of arrow node's transform
    private List<Transform> arrowNodes = new List<Transform>();
    //public List<GameObject> arrowNodesObject = new List<GameObject>();
    private List<Vector2> controlPoints = new List<Vector2>();                                                                    // The list of control points
    private readonly List<Vector2> controlPointFactors = new List<Vector2> { new Vector2(-0.3f, 0.8f), new Vector2(0.1f, 1.4f) }; // The factors to determine the position of control point P1, P2

    // Private Methods
    // Executes when the gameObject instantiates
    private void Awake()
    {
        
        // Gets position of the arrows emitter point
        //this.origin = this.GetComponent<RectTransform>();
        this.origin = this.GetComponent<Transform>();

        // Instantiates the arrow nodes and arrow head
        //for (int i = 0; i < this.ArrowNodeNum; i++)
        //    this.arrowNodes.Add(Instantiate(this.ArrowNodePrefab, this.transform).GetComponent<RectTransform>());

        //this.arrowNodes.Add(Instantiate(this.ArrowHeadPrefab, this.transform).GetComponent<RectTransform>());

        for (int i = 0; i < this.ArrowNodeNum; i++)
            this.arrowNodes.Add(Instantiate(this.ArrowNodePrefab, this.transform).GetComponent<Transform>());

        this.arrowNodes.Add(Instantiate(this.ArrowHeadPrefab, this.transform).GetComponent<Transform>());

        //for (int i = 0; i < this.ArrowNodeNum; i++)
        //    this.arrowNodesObject.Add(Instantiate(this.ArrowNodePrefab, this.transform));

        //this.arrowNodesObject.Add(Instantiate(this.ArrowHeadPrefab, this.transform));

        // Hides the arrow nodes
        //this.arrowNodes.ForEach(a => a.GetComponent<RectTransform>().position = new Vector2(-1000, -1000));
        this.arrowNodes.ForEach(a => a.GetComponent<Transform>().position = new Vector2(-1000, -1000));

        // Initializes the control points list
        for (int i = 0; i < 4; i++)
            this.controlPoints.Add(Vector2.zero);

        //for (int i = 0; i < this.ArrowNodeNum; i++)
        //    this.arrowNodesObject[i].transform.position = new Vector2(-1000, -1000);

    }

    // Executes every frame
    private void Update()
    {

        ControlColor();

        // P0 is at the arrow emitter point
        this.controlPoints[0] = new Vector2(this.origin.position.x, this.origin.position.y);

        // P3 is at the mouse position
        this.controlPoints[3] = Camera.main.ScreenToWorldPoint(new Vector2(Input.mousePosition.x, Input.mousePosition.y));

        // P1, P2 determines by P0 and P3
        // P1 = P0 + (P3 - P0) * Vector2(-0.3f, 0.8f)
        // P2 = P0 + (P3 - P0) * Vector2(0.1f, 1.4f)
        this.controlPoints[1] = this.controlPoints[0] + (this.controlPoints[3] - this.controlPoints[0]) * this.controlPointFactors[0];
        this.controlPoints[2] = this.controlPoints[0] + (this.controlPoints[3] - this.controlPoints[0]) * this.controlPointFactors[1];

        for(int i = 0; i < this.arrowNodes.Count; i++)
        {

            // Calculates t
            var t = Mathf.Log(1f * i / (this.arrowNodes.Count - 1) + 1f, 2f);

            // Cubic Bezier curve
            // B(t) = (1-t)^3 * P0 + 3 * (1-t)^2 * t * P1 + 3 * (1-t) * t^2 * P2 + t^3 * P3
            this.arrowNodes[i].position =
                Mathf.Pow(1 - t, 3) * this.controlPoints[0] +
                3 * Mathf.Pow(1 - t, 2) * t * this.controlPoints[1] +
                3 * (1 - t) * Mathf.Pow(t, 2) * this.controlPoints[2] +
                Mathf.Pow(t, 3) * this.controlPoints[3];

            // Calculates rotations for each arrow node
            if( i > 0)
            {

                var euler = new Vector3(0, 0, Vector2.SignedAngle(Vector2.up, this.arrowNodes[i].position - this.arrowNodes[i - 1].position));
                this.arrowNodes[i].rotation = Quaternion.Euler(euler);

            }

            // Calculates scales for each arrow node
            var scale = this.scaleFactor * (1f - 0.03f * (this.arrowNodes.Count - 1 - i));
            this.arrowNodes[i].localScale = new Vector3(scale, scale, 1f);

        }

        // The first arrow node's rotation
        this.arrowNodes[0].transform.rotation = this.arrowNodes[1].transform.rotation;

    }

    public void ControlColor()
    {

        if (/*!OverEnemy*/ Vector2.Distance(this.arrowNodes[arrowNodes.Count - 1].transform.position, Enemies[0].transform.position) <= 0.5)
        {

            for (int j = 0; j < this.arrowNodes.Count; j++)
            {

                this.arrowNodes[j].GetComponent<Image>().color = Color.red;

            }

        }
        else
        {

            for (int j = 0; j < this.arrowNodes.Count; j++)
            {

                this.arrowNodes[j].GetComponent<Image>().color = Color.grey;

            }

        }

    }

}
