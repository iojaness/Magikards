using System.Collections.Generic;
using UnityEngine;

public class Archimago : Ficha {

    private void Start() {
        vida = 7;
        ataque = 2;
    }

    public override List<Vector2Int> ObtenerMovimientosPosibles(ref Ficha[,] tablero, int cantCasillasX, int cantCasillasY) {
        List<Vector2Int> r = new List<Vector2Int>();

        // Movimientos hacia abajo
       for (int i = 1; i <= 5 && actualY - i >= 0; i++) {
            if (tablero[actualX, actualY - i] == null) {
                r.Add(new Vector2Int(actualX, actualY - i));
            } else {
                break; // Detén el cálculo si hay un aliado o enemigo
            }
        }

        // Movimientos hacia arriba
        for (int i = 1; i <= 5 && actualY + i < cantCasillasY; i++) {
            if (tablero[actualX, actualY + i] == null) {
                r.Add(new Vector2Int(actualX, actualY + i));
            } else {
                break; // Detén el cálculo si hay un aliado o enemigo
            }
        }

        // Movimientos hacia la izquierda
        for (int i = 1; i <= 5 && actualX - i >= 0; i++) {
            if (tablero[actualX - i, actualY] == null) {
                r.Add(new Vector2Int(actualX - i, actualY));
            } else {
                break; // Detén el cálculo si hay un aliado o enemigo
            }
        }

        // Movimientos hacia la derecha
        for (int i = 1; i <= 5 && actualX + i < cantCasillasX; i++) {
            if (tablero[actualX + i, actualY] == null) {
                r.Add(new Vector2Int(actualX + i, actualY));
            } else {
                break; // Detén el cálculo si hay un aliado o enemigo
            }
        }
        // Movimiento diagonal superior derecha
        for (int i = 1, x = actualX + 1, y = actualY + 1; i <= 5 && x < cantCasillasX && y < cantCasillasY; i++, x++, y++) {
            if (tablero[x, y] == null) {
                r.Add(new Vector2Int(x, y));
            } else {
                break;
            }
        }

        // Movimiento diagonal superior izquierda
        for (int i = 1, x = actualX - 1, y = actualY + 1; i <= 5 && x >= 0 && y < cantCasillasY; i++, x--, y++) {
            if (tablero[x, y] == null) {
                r.Add(new Vector2Int(x, y));
            } else {
                break;
            }
        }

        // Movimiento diagonal inferior derecha
        for (int i = 1, x = actualX + 1, y = actualY - 1; i <= 5 && x < cantCasillasX && y >= 0; i++, x++, y--) {
            if (tablero[x, y] == null) {
                r.Add(new Vector2Int(x, y));
            } else {
                break;
            }
        }

        // Movimiento diagonal inferior izquierda
        for (int i = 1, x = actualX - 1, y = actualY - 1; i <= 5 && x >= 0 && y >= 0; i++, x--, y--) {
            if (tablero[x, y] == null) {
                r.Add(new Vector2Int(x, y));
            } else {
                break;
            }
        }

        return r;
    }

    public override List<Vector2Int> ObtenerEnemigosCercanos(ref Ficha[,] tablero, int cantCasillasX, int cantCasillasY) {
        List<Vector2Int> enemigos = new List<Vector2Int>();

        for (int dx = -2; dx <= 2; dx++) {
            for (int dy = -2; dy <= 2; dy++) {
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