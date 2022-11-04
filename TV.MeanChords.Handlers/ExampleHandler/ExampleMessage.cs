using System.Collections.Generic;

namespace TV.MeanChords.Handlers.CalculosCuotasHandler
{

    #region Get Example value

    /// <summary>
    /// Modelo de la respuesta de GetCalculosCuotas
    /// </summary>
    public class GetExampleValueResponse
    {
        public int Value { get; set; }
    }

    #endregion

    #region Get Example Recipe
    public class GetExampleRecipeResponse
    {
        public string Nombre { get; set; }
        public int Id { get; set; }
        public string Descripcion { get; set; }
        public List<Ingredientes> Ingredientes { get; set; }
        public int Porciones { get; set; }
        public string Image { get; set; }
        public List<Pasos> Pasos { get; set; }
        public double Calificacion { get; set; }
    }

    public class Ingredientes
    {
        public string Ingrediente { get; set; }
    }

    public class Pasos
    {
        public string Paso { get; set; }
    }
    #endregion
}
