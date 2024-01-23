using ep_models;
using System.Reflection;

namespace ep_service
{
    public class PredictionService
    {
        public PredictionModel GetScore(EPInputModel EPInputModel)
        {
            PredictionModel outputModel;
            var calculationService = new CalculationService();
            calculationService.PerformCalculations(EPInputModel, out outputModel);S
            outputModel.ServiceMeta.ServiceVersion= Assembly.GetEntryAssembly().GetName().Version.ToString();
            outputModel.ServiceMeta.RequestTimeStampUTC = DateTime.UtcNow;
            outputModel.EPInputModel = EPInputModel;
            return outputModel;            
        }
    }
}
