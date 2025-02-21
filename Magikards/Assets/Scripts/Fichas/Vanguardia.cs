using System.Collections.Generic;
using UnityEngine;

public class Vanguardia : Ficha {

    private void Start() {
        vida = 5;
        ataque = 1;
    }

    public override List<Vector2Int> ObtenerMovimientosPosibles(ref Ficha[,] tablero, int cantCasillasX, int cantCasillasY) {
        List<Vector2Int> r = new List<Vector2Int>();

        // Movimientos hacia abajo
        for (int i = 1; i <= 4 && actualY - i >= 0; i++) {
            if (tablero[actualX, actualY - i] == null) {
                r.Add(new Vector2Int(actualX, actualY - i));
            } else {
                break; // Detén el cálculo si hay un aliado o enemigo
            }
        }

        // Movimientos hacia arriba
        for (int i = 1; i <= 4 && actualY + i < cantCasillasY; i++) {
            if (tablero[actualX, actualY + i] == null) {
                r.Add(new Vector2Int(actualX, actualY + i));
            } else {
                break; // Detén el cálculo si hay un aliado o enemigo
            }
        }

        // Movimientos hacia la izquierda
        for (int i = 1; i <= 4 && actualX - i >= 0; i++) {
            if (tablero[actualX - i, actualY] == null) {
                r.Add(new Vector2Int(actualX - i, actualY));
            } else {
                break; // Detén el cálculo si hay un aliado o enemigo
            }
        }

        // Movimientos hacia la derecha
        for (int i = 1; i <= 4 && actualX + i < cantCasillasX; i++) {
            if (tablero[actualX + i, actualY] == null) {
                r.Add(new Vector2Int(actualX + i, actualY));
            } else {
                break; // Detén el cálculo si hay un aliado o enemigo
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
