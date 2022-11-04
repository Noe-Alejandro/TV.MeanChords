using TV.MeanChords.Handlers.CalculosCuotasHandler;
using Newtonsoft.Json;
using NUnit.Framework;
using System.Collections.Generic;

namespace TV.MeanChords.Testing
{
    public class ExampleInteractor_Tests
    {
        ExampleInteractor service;
        [SetUp]
        public void Setup()
        {
            service = ExampleInteractor.Create();
        }

        [Test]
        public void ExampleValue_Test()
        {
            var expectedValue = new List<GetExampleValueResponse> { 
                new GetExampleValueResponse { Value = 1 }, 
                new GetExampleValueResponse { Value = 2 },
                new GetExampleValueResponse { Value = 3 } };

            var data = JsonConvert.SerializeObject(service.GetExampleValues().Data);

            Assert.AreEqual(JsonConvert.SerializeObject(expectedValue), data);
        }

        [Test]
        public void ExampleRecipe_Test()
        {
            string expectedValue = "{\"Status\":true,\"Data\":[{\"Nombre\":\"Prueba 1\",\"Id\":1,\"Descripcion\":\"Rico platillo 1\",\"Ingredientes\":[{\"Ingrediente\":\"1\"},{\"Ingrediente\":\"2\"}],\"Porciones\":4,\"Image\":\"https://www.annarecetasfaciles.com/files/arepas-colombianas-815x458.jpg\",\"Pasos\":[{\"Paso\":\"Vivir\"},{\"Paso\":\"Morir\"}],\"Calificacion\":4.5}],\"Errors\":[]}";

            var data = JsonConvert.SerializeObject(service.GetExampleRecipe());

            Assert.AreEqual(expectedValue, data);
        }
    }
}