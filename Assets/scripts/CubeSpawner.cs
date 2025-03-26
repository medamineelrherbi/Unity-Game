using UnityEngine;

public class CubeSpawner : MonoBehaviour
{
    public GameObject[] cubePrefabs;  // Liste des différents cubes à spawn
    public Transform spawnPoint;  // Position où les cubes apparaissent
    public string[] categories = { "Sales", "Marketing", "HR", "Finance" };  // Liste des catégories
    private GameObject currentCube; // Référence au cube actuellement spawné

    void Start()
    {
        SpawnNewCube(); // Spawn du premier cube au début du jeu
    }

    void Update()
    {
        if (currentCube != null) // Vérifie si un cube existe
        {
            float distance = Vector3.Distance(currentCube.transform.position, spawnPoint.position);

            if (distance > 1.5f) // Si le cube a été déplacé à plus de 1.5 unités
            {
                currentCube = null; // On oublie l'ancien cube
                SpawnNewCube(); // On spawn le suivant
            }
        }
    }

    void SpawnNewCube()
    {
        if (cubePrefabs.Length == 0) return; // Vérifie qu'il y a des cubes disponibles

        int randomIndex = Random.Range(0, cubePrefabs.Length); // Choix aléatoire d'un cube
        currentCube = Instantiate(cubePrefabs[randomIndex], spawnPoint.position, Quaternion.identity);

        // Assigne une catégorie aléatoire
        CubeMetadata metadata = currentCube.GetComponent<CubeMetadata>();
        if (metadata != null)
        {
            metadata.category = categories[Random.Range(0, categories.Length)];
            if (metadata.textLabel != null)
            {
                metadata.textLabel.text = metadata.category; // Met à jour le texte au-dessus du cube
            }
        }
    }
}
