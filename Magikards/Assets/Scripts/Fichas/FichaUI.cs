using UnityEngine;
using UnityEngine.UI;

public class FichaUI : MonoBehaviour
{
    public Image imagenVida;   // Imagen que representa la vida
    public Image imagenAtaque; // Imagen que representa el ataque

    public Sprite[] spritesVida;   // Sprites organizados por equipo, tipo y vida
    public Sprite[] spritesAtaque; // Sprites organizados por equipo, tipo y ataque

    private Ficha ficha;

    private void Start()
    {
        ficha = GetComponentInParent<Ficha>();
        if (ficha != null)
        {
            ActualizarUI();
        }
    }

    private void OnEnable()
    {
        ficha = GetComponentInParent<Ficha>();
        if (ficha != null)
        {
            ActualizarUI();
        }
    }


    private void Update()
    {
        if (Camera.main != null)
        {
            Vector3 direccion = transform.position - Camera.main.transform.position;
            transform.forward = direccion;
        }
    }

    public void ActualizarUI()
    {
        if (ficha != null)
        {
            imagenVida.sprite = ObtenerSpriteVida();
            imagenAtaque.sprite = ObtenerSpriteAtaque();
        }
    }

        private Sprite ObtenerSpriteVida()
    {
        int equipoIndex = ficha.equipo * 25; // Cada equipo tiene 25 sprites reservados
        int tipoIndex = 0;
        int vidaIndex = ficha.vida; // Vida como índice, pero limitada según el tipo de ficha

        switch (ficha.tipo)
        {
            case TipoFicha.Arcanista:
                tipoIndex = 0; // Primer tipo, empieza en 0
                vidaIndex = Mathf.Clamp(ficha.vida, 0, 5);
                break;
            case TipoFicha.Vanguardia:
                tipoIndex = 6; // Después de los 6 sprites de Arcanista (0 a 5)
                vidaIndex = Mathf.Clamp(ficha.vida, 0, 5);
                break;
            case TipoFicha.Archimago:
                tipoIndex = 12; // Después de los 6 de Arcanista + 6 de Vanguardia
                vidaIndex = Mathf.Clamp(ficha.vida, 0, 7);
                break;
            case TipoFicha.Augur:
                tipoIndex = 20; // Después de los 6 de Arcanista + 6 de Vanguardia + 8 de Archimago
                vidaIndex = Mathf.Clamp(ficha.vida, 0, 4);
                break;
        }

        int indiceFinal = equipoIndex + tipoIndex + vidaIndex;
        return spritesVida[indiceFinal];
    }



    private Sprite ObtenerSpriteAtaque()
    {
        int equipoIndex = ficha.equipo * 3; // 3 sprites por equipo (ataque 1-3)
        int ataqueIndex = Mathf.Clamp(ficha.ataque, 1, 3) - 1; // Restamos 1 para que ataque 1 corresponda al índice 0

        int indiceFinal = equipoIndex + ataqueIndex;
        return spritesAtaque[indiceFinal];
    }

    public void SetVida(int nuevaVida)
    {
        if (ficha != null)
        {
            ficha.vida = nuevaVida;
            ActualizarUI();
        }
    }

    public void SetAtaque(int nuevoAtaque)
    {
        if (ficha != null)
        {
            ficha.ataque = nuevoAtaque;
            ActualizarUI();
        }
    }
}
