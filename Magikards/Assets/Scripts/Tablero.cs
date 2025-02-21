using System.Collections.Generic;
using UnityEngine;

public class Tablero : MonoBehaviour
{
    [Header("Arte")]
    [SerializeField] public Material materialCasilla;
    [SerializeField] private float tamCasilla = 1;
    [SerializeField] private float desplazamientoY = 0.1f;
    [SerializeField] private Vector3 centroTablero = Vector3.zero;
    [SerializeField] private float desplazamientoAgarra = -1.5f;
    [SerializeField] private float desplazamientoMuertas = 0.3f;

    [Header("Prefabs y Materiales")]
    [SerializeField] private GameObject[] prefabsFichas;
    [SerializeField] private Material[] materialesEquipos;

    private Ficha[,] fichas;
    private Ficha agarrada;
    private List<Vector2Int> enemigosCercanos = new List<Vector2Int>();
    private List<Vector2Int> movimientosPosibles = new List<Vector2Int>();
    private List<Vector2Int> muertasAzules = new List<Vector2Int>();
    private List<Vector2Int> muertasRojas = new List<Vector2Int>();
    private const int NUMERO_CASILLAS_X = 13;
    private const int NUMERO_CASILLAS_Y = 16;
    private GameObject[,] casillas;
    private Camera camActual;
    private Vector2Int posActual;
    private Vector3 bordes;

    private void Awake(){
        GenerarTodasCasillas(1, NUMERO_CASILLAS_X, NUMERO_CASILLAS_Y);
        GenerarTodasFichas();
        posicionarTodasFichas();
    }

    private void Update()
    {
        if(!camActual){
            camActual = Camera.main;
            return;
        }

        RaycastHit info;
        Ray rayo = camActual.ScreenPointToRay(Input.mousePosition);

        if(Physics.Raycast(rayo, out info, 100, LayerMask.GetMask("Casilla", "SobreCasilla", "Iluminada", "CasillaEnemiga"))){
            Vector2Int posicionToque = BuscarIndiceCasilla(info.transform.gameObject);

            if(posActual == -Vector2Int.one){
                posActual = posicionToque;
                casillas[posicionToque.x, posicionToque.y].layer = LayerMask.NameToLayer("SobreCasilla");
            }

            if(posActual != posicionToque){
                casillas[posActual.x, posActual.y].layer = (TieneMovimientoValido(ref movimientosPosibles, posActual)) ? LayerMask.NameToLayer("Iluminada") : (TieneEnemigo(ref enemigosCercanos, posActual))? LayerMask.NameToLayer("CasillaEnemiga") : LayerMask.NameToLayer("Casilla");
                posActual = posicionToque;
                casillas[posicionToque.x, posicionToque.y].layer = LayerMask.NameToLayer("SobreCasilla");
            }

            if(Input.GetMouseButtonDown(0)){
                if(fichas[posicionToque.x, posicionToque.y] != null){
                    //es tu turno?
                    if(true){
                        agarrada = fichas[posicionToque.x, posicionToque.y];

                        enemigosCercanos = agarrada.ObtenerEnemigosCercanos(ref fichas, NUMERO_CASILLAS_X, NUMERO_CASILLAS_Y);
                        movimientosPosibles = agarrada.ObtenerMovimientosPosibles(ref fichas, NUMERO_CASILLAS_X, NUMERO_CASILLAS_Y);
                        IluminarCasillas();
                    }
                }
            }

            if(agarrada != null && Input.GetMouseButtonUp(0)){
                Vector2Int posAnterior = new Vector2Int(agarrada.actualX, agarrada.actualY);

                bool movimientoValido = MoverA(agarrada, posicionToque.x, posicionToque.y);

                if(!movimientoValido){
                    agarrada.Ubicar(EncontrarCentroCasilla(posAnterior.x, posAnterior.y));
                }
                agarrada = null;
                DesiluminarCasillas();
            }
        }
        else{
            if(posActual != -Vector2Int.one){
                casillas[posActual.x, posActual.y].layer = (TieneMovimientoValido(ref movimientosPosibles, posActual)) ? LayerMask.NameToLayer("Iluminada") : (TieneEnemigo(ref enemigosCercanos, posActual)) ? LayerMask.NameToLayer("CasillasEnemiga") : LayerMask.NameToLayer("Casilla");
                posActual = -Vector2Int.one;
            }
            
            if (agarrada && Input.GetMouseButtonUp(0)){
                agarrada.Ubicar(EncontrarCentroCasilla(agarrada.actualX, agarrada.actualY));
                agarrada = null;
                DesiluminarCasillas();
            }
        }

        if(agarrada){
            Plane planoHorizontal = new Plane(Vector3.up, Vector3.up * desplazamientoY);
            float distancia = 0.0f;
            if(planoHorizontal.Raycast(rayo, out distancia)){
                agarrada.Ubicar(rayo.GetPoint(distancia) + Vector3.up * desplazamientoAgarra);
            }
        }
    }

    private void GenerarTodasCasillas(float tamCasilla, int numCasillasX, int numCasillasY){
        desplazamientoY += transform.position.y;
        bordes = new Vector3((numCasillasX/2) * tamCasilla, 0, (numCasillasY/2) * tamCasilla) + centroTablero;
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
        vertices[0] = new Vector3(x * tamCasilla, desplazamientoY, y * tamCasilla) - bordes;
        vertices[1] = new Vector3(x * tamCasilla, desplazamientoY, (y + 1) * tamCasilla) - bordes;
        vertices[2] = new Vector3((x + 1) * tamCasilla, desplazamientoY, y * tamCasilla) - bordes;
        vertices[3] = new Vector3((x + 1) * tamCasilla, desplazamientoY, (y + 1) * tamCasilla) - bordes;

        int[] triangulos = new int[] {0,1,2,1,3,2};

        malla.vertices = vertices;
        malla.triangles = triangulos;
        malla.RecalculateNormals();

        objetoCasilla.layer = LayerMask.NameToLayer("Casilla");
        objetoCasilla.AddComponent<BoxCollider>();

        return objetoCasilla;
    }

    private void GenerarTodasFichas(){
        fichas = new Ficha[NUMERO_CASILLAS_X, NUMERO_CASILLAS_Y];

        int equipoAzul = 0, equipoRojo = 1;

        fichas[4, 0] = GenerarUnaFicha(TipoFicha.Augur, equipoAzul);
        fichas[6, 0] = GenerarUnaFicha(TipoFicha.Archimago, equipoAzul);
        fichas[8, 0] = GenerarUnaFicha(TipoFicha.Augur, equipoAzul);
        fichas[5, 1] = GenerarUnaFicha(TipoFicha.Arcanista, equipoAzul);
        fichas[7, 1] = GenerarUnaFicha(TipoFicha.Arcanista, equipoAzul);
        fichas[3, 2] = GenerarUnaFicha(TipoFicha.Vanguardia, equipoAzul);
        fichas[6, 2] = GenerarUnaFicha(TipoFicha.Vanguardia, equipoAzul);
        fichas[9, 2] = GenerarUnaFicha(TipoFicha.Vanguardia, equipoAzul);

        fichas[4, 15] = GenerarUnaFicha(TipoFicha.Augur, equipoRojo);
        fichas[6, 15] = GenerarUnaFicha(TipoFicha.Archimago, equipoRojo);
        fichas[8, 15] = GenerarUnaFicha(TipoFicha.Augur, equipoRojo);
        fichas[5, 14] = GenerarUnaFicha(TipoFicha.Arcanista, equipoRojo);
        fichas[7, 14] = GenerarUnaFicha(TipoFicha.Arcanista, equipoRojo);
        fichas[3, 13] = GenerarUnaFicha(TipoFicha.Vanguardia, equipoRojo);
        fichas[6, 13] = GenerarUnaFicha(TipoFicha.Vanguardia, equipoRojo);
        fichas[9, 13] = GenerarUnaFicha(TipoFicha.Vanguardia, equipoRojo);
    }

    private Ficha GenerarUnaFicha(TipoFicha tipo, int equipo){
        Ficha ficha = Instantiate(prefabsFichas[(int)tipo - 1], transform).GetComponent<Ficha>();

        ficha.tipo = tipo;
        ficha.equipo = equipo;

        // Cambiar el material del sombrero específicamente
        Transform sombrero = ficha.transform.Find("Sombrero"); // Busca el hijo llamado "Sombrero"
        if (sombrero != null)
        {
            MeshRenderer sombreroRenderer = sombrero.GetComponent<MeshRenderer>();
            if (sombreroRenderer != null)
            {
                sombreroRenderer.material = materialesEquipos[equipo]; // Cambia el material del sombrero
            }
        }
        else
        {
            Debug.LogError("No se encontró el objeto 'Sombrero' en el prefab.");
        }

        return ficha;

    }

    private void posicionarTodasFichas(){
        for (int x = 0; x < NUMERO_CASILLAS_X; x++){
            for (int y = 0; y < NUMERO_CASILLAS_Y; y++){
                if(fichas[x, y] != null){
                    posicionarUnaFicha(x, y, true);
                }
            }
        }
    }

    private void posicionarUnaFicha(int x, int y, bool forzado = false){
            fichas[x, y].actualX = x;
            fichas[x, y].actualY = y;
            fichas[x, y].Ubicar(EncontrarCentroCasilla(x, y),forzado);
    }
    
    private Vector3 EncontrarCentroCasilla(int x, int y){
        return new Vector3(x * tamCasilla, desplazamientoY-0.0067f, y * tamCasilla) - bordes + new Vector3(tamCasilla / 2, 0, tamCasilla / 2);
    }

    private void IluminarCasillas(){
        for(int i = 0; i < movimientosPosibles.Count; i++){
            casillas[movimientosPosibles[i].x, movimientosPosibles[i].y].layer = LayerMask.NameToLayer("Iluminada");

        }

        for(int i = 0; i < enemigosCercanos.Count; i++){
            casillas[enemigosCercanos[i].x, enemigosCercanos[i].y].layer = LayerMask.NameToLayer("CasillaEnemiga");
        }
    }

    private void DesiluminarCasillas(){
        for(int i = 0; i < movimientosPosibles.Count; i++){
            casillas[movimientosPosibles[i].x, movimientosPosibles[i].y].layer = LayerMask.NameToLayer("Casilla");
        }

        for(int i = 0; i < enemigosCercanos.Count; i++){
            casillas[enemigosCercanos[i].x, enemigosCercanos[i].y].layer = LayerMask.NameToLayer("Casilla");
        }

        enemigosCercanos.Clear();
        movimientosPosibles.Clear();
    }

    private bool TieneMovimientoValido(ref List<Vector2Int> movimientos, Vector2Int pos){
        for(int i = 0; i < movimientos.Count; i++){
            if(movimientos[i].x == pos.x && movimientos[i].y == pos.y){
                return true;
            }
        }
        return false;
    }

    private bool TieneEnemigo(ref List<Vector2Int> enemigos, Vector2Int pos){
        for(int i = 0; i < enemigos.Count; i++){
            if(enemigos[i].x == pos.x && enemigos[i].y == pos.y){
                return true;
            }
        }
        return false;
    }

    private bool MoverA(Ficha ficha, int x, int y)
    {
        if (!TieneMovimientoValido(ref movimientosPosibles, new Vector2Int(x, y)))
        {
            if (!TieneEnemigo(ref enemigosCercanos, new Vector2Int(x, y)))
            {
                return false;
            }
        }

        Vector2Int posAnterior = new Vector2Int(ficha.actualX, ficha.actualY);

        if (fichas[x, y] != null)
        {
            Ficha enemigo = fichas[x, y];

            if (enemigo.equipo == ficha.equipo)
            {
                return false; // No puedes atacar a un aliado
            }
            else
            {
                // Atacar al enemigo
                enemigo.vida -= ficha.ataque;
                Debug.Log("El enemigo recibió " + ficha.ataque + " de daño. Vida restante: " + enemigo.vida);

                if (enemigo.vida <= 0)
                {
                    if(enemigo.equipo == 0){

                        enemigo.Ubicar(new Vector3(14 * tamCasilla, desplazamientoY, 1 * tamCasilla)
                        - bordes
                        + new Vector3(tamCasilla / 2, 0, tamCasilla / 2)
                        + (Vector3.forward * desplazamientoMuertas * (muertasAzules.Count + muertasAzules.Count)), false);
                        muertasAzules.Add(new Vector2Int(x, y));
                    }
                    else{

                        enemigo.Ubicar(new Vector3(-2 * tamCasilla, desplazamientoY, 14 * tamCasilla)
                        - bordes
                        + new Vector3(tamCasilla / 2, 0, tamCasilla / 2)
                        + (Vector3.back * desplazamientoMuertas * (muertasRojas.Count + muertasRojas.Count)), false);

                        muertasRojas.Add(new Vector2Int(x, y));
                    }
                    fichas[x, y] = null; // Liberar la casilla
                }
                else
                {
                    return false; // Si el enemigo sigue vivo, el movimiento no se realiza
                }
            }
        }

        // Mover la ficha a la nueva posición
        fichas[x, y] = ficha;
        fichas[posAnterior.x, posAnterior.y] = null;

        posicionarUnaFicha(x, y, false);

        return true;
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
