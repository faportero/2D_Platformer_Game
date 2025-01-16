using Assets.Scripts.Modelos;
using System.Collections.Generic;
public partial class ControlDatos
{
    // Wrapper necesario para serializar la lista
    [System.Serializable]
    public class ListObjectModel
    {
        public List<ObjetoModel> objetos;
    }
}
