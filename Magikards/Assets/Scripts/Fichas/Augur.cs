using System.Collections.Generic;
using UnityEngine;

public class Augur : Ficha {
    private void Start(){
        ataque = 1;
        vida = 4;
    }

    public override List<Vector2Int> ObtenerMovimientosPosibles(ref Ficha[,] tablero, int cantCasillasX, int cantCasillasY) {
        List<Vector2Int> r = new List<Vector2Int>();

        for (int dx = -2; dx <= 2; dx++) {
            for (int dy = -2; dy <= 2; dy++) {
                int newX = actualX + dx;
                int newY = actualY + dy;

                // Saltar la zona central (cuadro 3x3)
                if (Mathf.Abs(dx) <= 1 && Mathf.Abs(dy) <= 1) {
                    continue;
                }

                // Verificar límites del tablero
                if (newX >= 0 && newX < cantCasillasX && newY >= 0 && newY < cantCasillasY) {
                    if (tablero[newX, newY] == null) { // Solo moverse a casillas vacías
                        r.Add(new Vector2Int(newX, newY));
                    }
                }
            }
        }

        return r;
    }

    public override List<Vector2Int> ObtenerEnemigosCercanos(ref Ficha[,] tablero, int cantCasillasX, int cantCasillasY) {
        List<Vector2Int> enemigos = new List<Vector2Int>();

        for (int dx = -1; dx <= 1; dx++) {
            for (int dy = -1; dy <= 1; dy++) {
                int nuevoX = actualX + dx;
                int nuevoY = actualY + dy;

                // Verificar límites del tablero
                if (nuevoX >= 0 && nuevoX < cantCasillasX && nuevoY >= 0 && nuevoY < cantCasillasY) {
                    if (tablero[nuevoX, nuevoY] != null && tablero[nuevoX, nuevoY].equipo != equipo) { // Solo identificar enemigos
                        enemigos.Add(new Vector2Int(nuevoX, nuevoY));
                    }
                }
            }
        }

        return enemigos;
    }
}
