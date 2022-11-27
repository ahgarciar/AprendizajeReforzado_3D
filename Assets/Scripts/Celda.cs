using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Celda
{
    private int[] recompensas;
    private int[] indexObjects;
    private int[] actions;
    private Direccion[] directions;

    public int Name { get; set; }

    public int getTotalAcciones() {
        return actions.Length;
    }

    public int getAccion(int index)
    {
        return actions[index];
    }

    public int getRecompensa(int index)
    {
        return recompensas[index];
    }

    public int getIndexCeldaDestino(int index)
    {
        return indexObjects[index];
    }

    public Direccion getDireccion(int index)
    {
        return directions[index];
    }

    public Celda(int name, int[] indexObjects, int[] recompensas, Direccion[] directions, int[] actions) {
        this.Name = name;
        this.recompensas = recompensas;
        this.indexObjects = indexObjects;
        this.directions = directions;
        this.actions = actions;
    }

    public override bool Equals(object obj)
    {         
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }
        else
        {
            Celda c = (Celda)obj;
            return this.Name.Equals(c.Name);
        }        
    }
  
    public override int GetHashCode()
    {        
        return base.GetHashCode();
    }

    public override string ToString()
    {
        string l = base.ToString() + " Nombre: " + Name + "\n";

        for (int i = 0; i < indexObjects.Length; i++)
        {
            l += "indice posible: " + indexObjects[i] + "  recompensa: " + recompensas[i] + "\n";
        }

        return l;
    }
   

}
