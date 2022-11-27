using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    //probabilidad para explorar o explotar
    public float Epsilon { get; set; }

    //factor de aprendizaje --- cuanto mayor sea este factor, más influencia tienen las recompensas futuras en el valor Q del par analizado
    float alpha = 0.1f;

    //factor de descuento 
    float gamma = 0.9f;  //fatiga 

    float[][] tabla_valoresQ;

    ArrayList grid;

    PlayerMovement player;

    ArrayList bestRoute;
    float mejorRecompensa;

    ArrayList currentRoute;
    float recompensaActual;

    // Start is called before the first frame update
    private void Start()
    {
        player = GetComponent<PlayerMovement>();
    }

    public void Inicializar()
    {        
        grid = new ArrayList();
        //Nombre ,IndexObjects , Recompensas, Direcciones, Acciones
        //Derecha,      0
        //Izquierda,    1
        //Arriba,       2
        //Abajo,        3
        //Reinicio,
        //Permanecer    4
        grid.Add(new Celda(0, new int[] { 3,0 }, new int[] { -1, +100 }, new Direccion[] { Direccion.Abajo, Direccion.Permanecer }, new int[] { 3,4 }));
        grid.Add(new Celda(1, new int[] { 4 }, new int[] { -1 }, new Direccion[] { Direccion.Abajo }, new int[] { 3 }));
        grid.Add(new Celda(2, new int[] { 5 }, new int[] { -1 }, new Direccion[] { Direccion.Abajo }, new int[] { 3 }));
        grid.Add(new Celda(3, new int[] { 0, 4, 6 }, new int[] { -1, -1, -10 }, new Direccion[] { Direccion.Arriba, Direccion.Derecha, Direccion.Abajo }, new int[] { 2,0,3 }));
        grid.Add(new Celda(4, new int[] { 1, 3, 5 }, new int[] { -1, -1, -1 }, new Direccion[] { Direccion.Arriba, Direccion.Izquierda, Direccion.Derecha }, new int[] { 2,1,0 }));
        grid.Add(new Celda(5, new int[] { 2, 4, 8 }, new int[] { -10, -1, -1 }, new Direccion[] { Direccion.Arriba, Direccion.Izquierda, Direccion.Abajo }, new int[] { 2,1,3 }));
        grid.Add(new Celda(6, new int[] { 3, 7 }, new int[] { -1, -1 }, new Direccion[] { Direccion.Arriba, Direccion.Derecha }, new int[] { 2,0 }));
        grid.Add(new Celda(7, new int[] { 6, 8 }, new int[] { -10, -1 }, new Direccion[] { Direccion.Izquierda, Direccion.Derecha }, new int[] { 1,0 }));
        grid.Add(new Celda(8, new int[] { 5, 7 }, new int[] { -1, -1 }, new Direccion[] { Direccion.Arriba, Direccion.Izquierda }, new int[] { 2,1 }));
                       
        mejorRecompensa = 0;
        recompensaActual = 0;

        bestRoute = new ArrayList();
        currentRoute = new ArrayList();

        currentRoute.Add(grid[7]);  //Celda 8
        player.moveUser(Direccion.Reinicio);

        tabla_valoresQ = new float[9][];

        for (int i = 0; i < tabla_valoresQ.Length; i++)
        {
            tabla_valoresQ[i] = new float[5];
            for (int j = 0; j < tabla_valoresQ[i].Length; j++)
            {
                tabla_valoresQ[i][j] = 0;
            }
        }

        Exploracion();

    }

    internal void Explotacion() {
        this.Epsilon = 0;
    }

    internal void Exploracion()
    {
        this.Epsilon = 0.4f;
    }

    //Para pruebas
    internal void Meta() {
        player.moveUser(Direccion.Izquierda);
        player.moveUser(Direccion.Arriba);
        player.moveUser(Direccion.Arriba);
    }

    internal void Reiniciar()
    {
        recompensaActual = 0;
        currentRoute.Clear();
        currentRoute.Add(grid[7]);  //Celda 8
        player.moveUser(Direccion.Reinicio);
    }


    internal string getMejorCamino()
    {
        string val_camino = "";
        for (int i = 0; i < bestRoute.Count - 1 ; i++)
        {
            val_camino += ((Celda)bestRoute[i]).Name + " -> ";
        }
        return val_camino;
    }

    internal string getMejorRecompensa()
    {
        return Convert.ToString(mejorRecompensa);
    }

    internal string getRecompensaActual()
    {
        return Convert.ToString(recompensaActual);
    }
    
    internal string getTablaValoresQ() {
        string tablaQ = "";

        for (int i = 0; i < tabla_valoresQ.Length; i++)
        {
            for (int j = 0; j < tabla_valoresQ[i].Length; j++)
            {
                tablaQ += tabla_valoresQ[i][j] + "\t";
            }
            tablaQ += "\n";
        }
        
        return tablaQ;
    }


    Celda celdaActual;
    Celda celdaNueva;
          
    int aux;
    float random;

    internal bool Mover(int tiempo)
    {
        int index;

        celdaActual = (Celda)currentRoute[currentRoute.Count - 1];

        random = UnityEngine.Random.Range(0f, 1f);

        Debug.Log("random: " + random);

        if (random < Epsilon)
        {
            //exploración
            //la acción es escogida aleatoriamente
            index = UnityEngine.Random.Range(0, celdaActual.getTotalAcciones());
        }
        else {
            //explotación
            //la acción es escogida con base en la mejor acción para el estado (tabla de valores Q) -- (future reward)
            index = getIndexAccion_MaxValueQ(celdaActual.Name);
        }

        celdaNueva = (Celda)grid[celdaActual.getIndexCeldaDestino(index)];
       
        currentRoute.Add(celdaNueva);

        if (Epsilon != 0)
        {
            //Actualización de la tabla de valores Q        
            tabla_valoresQ[celdaActual.Name][celdaActual.getAccion(index)] =
                (1 - alpha) * tabla_valoresQ[celdaActual.Name][celdaActual.getAccion(index)]
                + alpha * (celdaActual.getRecompensa(index) + gamma * getMaxValueQ(celdaNueva.Name) - tabla_valoresQ[celdaActual.Name][celdaActual.getAccion(index)]);
        }                


        Debug.Log("Tiempo: " + tiempo);

        //con fatiga
        if (tiempo == 0)
        {
            recompensaActual = celdaActual.getRecompensa(index);
        }
        else if(celdaActual.Name != 0)
        {
            recompensaActual += Mathf.Pow(gamma, tiempo) * celdaActual.getRecompensa(index);
        }
         
        player.moveUser(celdaActual.getDireccion(index));

        if (celdaActual.Name != 0)
        {
            return false;
        }

        //
        recompensaActual += 100;

        if (recompensaActual > mejorRecompensa || Epsilon ==0)
        {
            mejorRecompensa = recompensaActual;

            bestRoute = (ArrayList)currentRoute.Clone();
            Debug.Log("entra");
        }

        return true;
    }


    internal float getMaxValueQ(int S) {

        float valor_accion = float.MinValue;

        for (int i = 0; i < celdaNueva.getTotalAcciones(); i++) //para todas las acciones validas de la celda nueva
        {
            aux = celdaNueva.getAccion(i);

            if (tabla_valoresQ[S][aux] > valor_accion)
            {
                valor_accion = tabla_valoresQ[S][aux];                
            }

        }

        return valor_accion;

    }

    internal int getIndexAccion_MaxValueQ(int S)
    {        
        int index = 0; 

        float valor_accion = float.MinValue;

        for (int i = 0; i < celdaActual.getTotalAcciones(); i++) //para todas las acciones de la celda actual
        {            
            aux = celdaActual.getAccion(i);

            if (tabla_valoresQ[S][aux] > valor_accion)
            {
                valor_accion = tabla_valoresQ[S][aux];                                
                index = i;                
            }

        }

        return index;

    }


}
