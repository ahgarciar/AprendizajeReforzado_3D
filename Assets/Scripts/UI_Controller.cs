using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Controller : MonoBehaviour
{
    bool endEpisodicTask;

    Grid grilla;

    int tiempo;
    int mejorTiempo;

    public Text valorTiempo;
    public Text valorMejorTiempo;

    public Text valorMejorReconpensa;
    public Text valorMejorCamino;

    public Text valorReconpensaActual;

    public Text valorTablaQ;

    

    private void Awake()
    {
        grilla = GameObject.Find("Player_Object").GetComponent<Grid>();
    }

    public void Iniciar()
    {
        tiempo = 0;

        mejorTiempo = int.MaxValue;

        endEpisodicTask = false;

        grilla.Inicializar();

        StartCoroutine("PlayGame");

        //StartCoroutine("PlayGameTester");

        Debug.Log("Proceso Iniciado");

    }

    // Update is called once per frame
    public void Detener()
    {
        StopCoroutine("PlayGame");
        //StopCoroutine("PlayGameTester");
    }


    float epsilon = 0;

    public void Explotacion_Exploracion() {
        grilla.Explotacion();
    }


    IEnumerator PlayGameTester()
    {
        while (true)
        {

            grilla.Meta();

            yield return new WaitForSeconds(1);

            grilla.Reiniciar();

            yield return new WaitForSeconds(1);
        }
    }



        IEnumerator PlayGame()
    {
        while (true)
        {

            valorTablaQ.text = grilla.getTablaValoresQ();

            if (endEpisodicTask)
            {

                //Que se halla realizado un menor tiempo en un recorrido no garantiza que este sea el mejor recorrido... 
                if (tiempo < mejorTiempo)
                {
                    mejorTiempo = tiempo;
                    valorMejorTiempo.text = Convert.ToString(mejorTiempo);
                }

                tiempo = 0;

                valorMejorReconpensa.text = grilla.getMejorRecompensa();
                valorMejorCamino.text = grilla.getMejorCamino();                

                grilla.Reiniciar(); //nuevo Episodic Task 

                valorReconpensaActual.text = "0";

                Debug.Log("New Episodic Task");
            }

            endEpisodicTask = grilla.Mover(tiempo); //Movimiento aleatorio basado en la 
                                                    //celda en que se encuentra el user 

            valorTiempo.text = tiempo++.ToString();            

            valorReconpensaActual.text = grilla.getRecompensaActual();
                                       
            yield return new WaitForSeconds(.1f);
        }
    }


}
