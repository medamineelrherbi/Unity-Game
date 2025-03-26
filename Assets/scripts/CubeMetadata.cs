using UnityEngine;
using TMPro;

public class CubeMetadata : MonoBehaviour
{
    public string category; // Category name
    public Color color; // Cube color
    public TextMeshPro textLabel; // Reference to the label

    void Start()
    {
        if (textLabel != null)
        {
            textLabel.text = category; // Set the text to the category name
        }
    }

    void Update()
    {
        if (textLabel != null)
        {
            textLabel.transform.position = transform.position + Vector3.up * 1.5f; // Keep the text above the cube
            textLabel.transform.LookAt(Camera.main.transform); // Make text face the player
            textLabel.transform.Rotate(0, 180, 0); // Flip it so it's readable
        }
    }
}
