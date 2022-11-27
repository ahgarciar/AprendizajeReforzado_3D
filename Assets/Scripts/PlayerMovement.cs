using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direccion
{
    Derecha,
    Izquierda,
    Arriba,
    Abajo,
    Reinicio,
    Permanecer

}


public class PlayerMovement : MonoBehaviour
{
    Vector3 posInicio;

    public int speed = 250;

    public void Awake()
    {
        posInicio = GetComponent<Rigidbody>().position;
    }


    public void moveUser(Direccion d)
    {

        switch (d)
        {

            case Direccion.Arriba://adelante

                GetComponent<Rigidbody>().position += transform.forward * speed;
                break;


            case Direccion.Izquierda: //izquierda

                GetComponent<Rigidbody>().position += transform.right * -1 * speed;                
                break;

            case Direccion.Abajo: //atras

                GetComponent<Rigidbody>().position += transform.forward *-1 * speed;
                break;

            case Direccion.Derecha: //derecha

                 GetComponent<Rigidbody>().position += transform.right * speed;
                break;


            case Direccion.Reinicio: //posicion Origen

                GetComponent<Rigidbody>().position = posInicio; //new Vector3(450,50,-800);
                break;
            default:
                //no se efectuará cambio en la posición del objeto
                break;

        }

    }

}