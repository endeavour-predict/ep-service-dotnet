using ep_models;
using System.Reflection;

namespace ep_service
{
    internal static class InputMapper
    {
        internal static void MapServiceInputToCalculatorInput<T>(EPInputModel epInputModel, ref T calcInputModel) where T : new()
        {           
            // we'll keep a total of how many calculator properties we are able to map, and throw an exception if we don't find a match for ALL of them in the API inputmodel
            // this is a safety net to catch typos and mismatches in the mapping
            int calcPropertiesAvailable = 0;
            int calcPropertiesMapped = 0;

            PropertyInfo[] calcInputProperties = typeof(T).GetProperties();
            PropertyInfo[] apiInputProperties = typeof(EPInputModel).GetProperties();
            foreach (PropertyInfo calcProperty in calcInputProperties)
            {
                calcPropertiesAvailable++;
                // look for a match
                var apiInputProperty = apiInputProperties.Where(p => p.Name == calcProperty.Name).SingleOrDefault();
                if (apiInputProperty != null)
                {
                    calcPropertiesMapped++;
                    calcProperty.SetValue(calcInputModel, apiInputProperty.GetValue(epInputModel));
                }                
            }
            if (calcPropertiesAvailable == 0 || calcPropertiesAvailable != calcPropertiesMapped)
            {
                throw new ApplicationException("Mapper error, number of available fields in calc: ("+ calcPropertiesAvailable + ") doesn't match the number mapped: ("+ calcPropertiesMapped + ")");
            }
        }
    }
}
