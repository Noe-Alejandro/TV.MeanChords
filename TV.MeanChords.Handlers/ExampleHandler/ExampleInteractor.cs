using TV.MeanChords.Data.Db.UnitOfWork;
using TV.MeanChords.Utils.GenericClass;
using System.Collections.Generic;

namespace TV.MeanChords.Handlers.CalculosCuotasHandler
{
    public class ExampleInteractor : IExample
    {
        private UoWData UoWData { get; set; }

        public static ExampleInteractor Create() => new ExampleInteractor();

        public ExampleInteractor()
        {
            UoWData = UoWData.Create();
        }

        public ResponseBase<List<GetExampleValueResponse>> GetExampleValues()
        {
            List<GetExampleValueResponse> data = new List<GetExampleValueResponse>();
            data.Add(new GetExampleValueResponse { Value = 1});
            data.Add(new GetExampleValueResponse { Value = 2 });
            data.Add(new GetExampleValueResponse { Value = 3 });
            return ResponseBase<List<GetExampleValueResponse>>.Create(data);
        }

        public ResponseBase<List<GetExampleRecipeResponse>> GetExampleRecipe()
        {
            List<GetExampleRecipeResponse> data = new List<GetExampleRecipeResponse>();
            data.Add(new GetExampleRecipeResponse 
            {
                Nombre = "Prueba 1",
                Id = 1,
                Descripcion = "Rico platillo 1",
                Ingredientes = new List<Ingredientes>{ new Ingredientes{ Ingrediente = "1" }, new Ingredientes { Ingrediente = "2" } },
                Porciones = 4,
                Image = "https://www.annarecetasfaciles.com/files/arepas-colombianas-815x458.jpg",
                Pasos = new List<Pasos> { new Pasos { Paso = "Vivir"}, new Pasos { Paso = "Morir"} },
                Calificacion = 4.5
            });
            return ResponseBase<List<GetExampleRecipeResponse>>.Create(data);
        }

        public void Dispose()
        {
            this.UoWData.Dispose();
            this.UoWData = null;
        }
    }
}
