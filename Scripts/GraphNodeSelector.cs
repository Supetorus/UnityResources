using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphNodeSelector : MonoBehaviour
{
    public GameObject selector;
    public GameObject source;
    public GameObject destination;
    public LayerMask layerMask;

    public bool isActive { get { return selector.activeSelf; } }
    public GraphNode sourceNode { get; set; }
    public GraphNode destinationNode { get; set; }

    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hitInfo, 100, layerMask))
        {
            // get node component on collider
            var node = hitInfo.collider.GetComponent<GraphNode>();
            if (node == null) return;

            // set selection object active at node position
            selector.SetActive(true);
            selector.transform.position = node.transform.position;

            if (Input.GetKeyDown(KeyCode.S))
            {
                sourceNode = node;
                source.transform.position = node.transform.position;
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                destinationNode = node;
                destination.transform.position = node.transform.position;
            }
        }
        else
        {
            selector.SetActive(false);
        }

        destination.SetActive(destinationNode != null);
        source.SetActive(sourceNode != null);
    }
}
