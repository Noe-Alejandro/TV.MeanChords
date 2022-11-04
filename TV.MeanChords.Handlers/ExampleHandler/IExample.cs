using TV.MeanChords.Utils.GenericClass;
using System;
using System.Collections.Generic;

namespace TV.MeanChords.Handlers.CalculosCuotasHandler
{
    public interface IExample : IDisposable
    {
        /// <summary>
        /// Devuelve valores de ejemplo
        /// </summary>
        /// <returns></returns>
        ResponseBase<List<GetExampleValueResponse>> GetExampleValues();
        /// <summary>
        /// Devuelve valor de receta ejemplo
        /// </summary>
        /// <returns></returns>
        ResponseBase<List<GetExampleRecipeResponse>> GetExampleRecipe();
    }
}
