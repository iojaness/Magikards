using UnityEngine;

public class Tablero : MonoBehaviour
{
    [Header("Arte")]
    [SerializeField] public Material materialCasilla;
    private const int NUMERO_CASILLAS_X = 13;
    private const int NUMERO_CASILLAS_Y = 18;
    private GameObject[,] casillas;
    private Camera camActual;
    private Vector2Int posActual;

    private void Awake(){
        GenerarTodasCasillas(1, NUMERO_CASILLAS_X, NUMERO_CASILLAS_Y);
    }

    private void Update()
    {
        if(!camActual){
            camActual = Camera.main;
            return;
        }

        RaycastHit info;
        Ray rayo = camActual.ScreenPointToRay(Input.mousePosition);

        if(Physics.Raycast(rayo, out info, 100, LayerMask.GetMask("Casilla", "CasillaSeleccionada"))){
            Vector2Int posicionToque = BuscarIndiceCasilla(info.transform.gameObject);

            if(posActual == -Vector2Int.one){
                posActual = posicionToque;
                casillas[posicionToque.x, posicionToque.y].layer = LayerMask.NameToLayer("CasillaSeleccionada");
            }

            if(posActual != posicionToque){
                casillas[posActual.x, posActual.y].layer = LayerMask.NameToLayer("Casilla");
                posActual = posicionToque;
                casillas[posicionToque.x, posicionToque.y].layer = LayerMask.NameToLayer("CasillaSeleccionada");
            }
        }
        else{
            if(posActual != -Vector2Int.one){
                casillas[posActual.x, posActual.y].layer = LayerMask.NameToLayer("Casilla");
                posActual = -Vector2Int.one;
            }
        }
    }

    private void GenerarTodasCasillas(float tamCasilla, int numCasillasX, int numCasillasY){
        casillas = new GameObject[numCasillasX, numCasillasY];
        for (int x = 0; x < numCasillasX; x++){
            for (int y = 0; y < numCasillasY; y++){
                casillas[x, y] = GenerarUnaCasilla(tamCasilla, x, y);
            }
        }
    }

    private GameObject GenerarUnaCasilla(float tamCasilla, int x, int y){
        GameObject objetoCasilla = new GameObject(string.Format("Casilla {0} {1}", x, y));
        objetoCasilla.transform.parent =transform;

        Mesh malla = new Mesh();
        objetoCasilla.AddComponent<MeshFilter>().mesh = malla;
        objetoCasilla.AddComponent<MeshRenderer>().material = materialCasilla;

        Vector3[] vertices = new Vector3[4];
        vertices[0] = new Vector3(x * tamCasilla, 0, y * tamCasilla);
        vertices[1] = new Vector3(x * tamCasilla, 0, (y + 1) * tamCasilla);
        vertices[2] = new Vector3((x + 1) * tamCasilla, 0, y * tamCasilla);
        vertices[3] = new Vector3((x + 1) * tamCasilla, 0, (y + 1) * tamCasilla);

        int[] triangulos = new int[] {0,1,2,1,3,2};

        malla.vertices = vertices;
        malla.triangles = triangulos;
        malla.RecalculateNormals();

        objetoCasilla.layer = LayerMask.NameToLayer("Casilla");
        objetoCasilla.AddComponent<BoxCollider>();

        return objetoCasilla;
    }

    private Vector2Int BuscarIndiceCasilla(GameObject infoToque){
        for (int x = 0; x < NUMERO_CASILLAS_X; x++){
            for (int y = 0; y < NUMERO_CASILLAS_Y; y++){
                if(casillas[x, y] == infoToque){
                    return new Vector2Int(x, y);
                }
            }
        }
        return -Vector2Int.one;
    }
}
