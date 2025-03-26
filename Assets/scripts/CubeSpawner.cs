using UnityEngine;

public class CubeSpawner : MonoBehaviour
{
    public GameObject[] cubePrefabs;  // Liste des diff�rents cubes � spawn
    public Transform spawnPoint;  // Position o� les cubes apparaissent
    public string[] categories = { "Sales", "Marketing", "HR", "Finance" };  // Liste des cat�gories
    private GameObject currentCube; // R�f�rence au cube actuellement spawn�

    void Start()
    {
        SpawnNewCube(); // Spawn du premier cube au d�but du jeu
    }

    void Update()
    {
        if (currentCube != null) // V�rifie si un cube existe
        {
            float distance = Vector3.Distance(currentCube.transform.position, spawnPoint.position);

            if (distance > 1.5f) // Si le cube a �t� d�plac� � plus de 1.5 unit�s
            {
                currentCube = null; // On oublie l'ancien cube
                SpawnNewCube(); // On spawn le suivant
            }
        }
    }

    void SpawnNewCube()
    {
        if (cubePrefabs.Length == 0) return; // V�rifie qu'il y a des cubes disponibles

        int randomIndex = Random.Range(0, cubePrefabs.Length); // Choix al�atoire d'un cube
        currentCube = Instantiate(cubePrefabs[randomIndex], spawnPoint.position, Quaternion.identity);

        // Assigne une cat�gorie al�atoire
        CubeMetadata metadata = currentCube.GetComponent<CubeMetadata>();
        if (metadata != null)
        {
            metadata.category = categories[Random.Range(0, categories.Length)];
            if (metadata.textLabel != null)
            {
                metadata.textLabel.text = metadata.category; // Met � jour le texte au-dessus du cube
            }
        }
    }
}
