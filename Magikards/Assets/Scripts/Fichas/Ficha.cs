using System.Collections.Generic;
using UnityEngine;

public enum TipoFicha {
    Ninguna = 0,
    Archimago = 1,
    Augur = 2,
    Arcanista = 3,
    Vanguardia = 4
}

public class Ficha : MonoBehaviour {
    public int equipo;
    public int actualX;
    public int actualY;
    public int vida;
    public int ataque;
    public TipoFicha tipo;

    private Vector3 posDeseada = Vector3.one;

    private void Update()
    {
        transform.position = Vector3.Lerp(transform.position, posDeseada, Time.deltaTime * 5);
    }

    public virtual List<Vector2Int> ObtenerMovimientosPosibles(ref Ficha[,] tablero, int cantCasillasX, int cantCasillasY){
        List<Vector2Int> r = new List<Vector2Int>();

        return r;
    }

    public virtual List<Vector2Int> ObtenerEnemigosCercanos(ref Ficha[,] tablero, int cantCasillasX, int cantCasillasY) {
    List<Vector2Int> enemigos = new List<Vector2Int>();

    enemigos.Add(new Vector2Int(3, 3));
    enemigos.Add(new Vector2Int(3, 4));
    enemigos.Add(new Vector2Int(4, 3));
    enemigos.Add(new Vector2Int(4, 4));

    return enemigos;
}


    public virtual void Ubicar(Vector3 pos, bool forzado = false){
        posDeseada = pos;
        if(forzado){
            transform.position = posDeseada;
        }
    }
}