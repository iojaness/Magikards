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
    public TipoFicha tipo;

    private Vector3 posDeseada;
    private Vector3 escalaDeseada;
}