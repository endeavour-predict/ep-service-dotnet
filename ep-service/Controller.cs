﻿using ep_service.Models;
using ep_service.Services;
using System.Reflection;

namespace ep_service
{
    public class PredictionService
    {
        public PredictionModel GetScore(EPInputModel EPInputModel)
        {
            PredictionModel outputModel;
            var calculationService = new CalculationService();
            calculationService.PerformCalculations(EPInputModel, out outputModel);

            outputModel.Meta.ServiceVersion= Assembly.GetEntryAssembly().GetName().Version.ToString() ;
            outputModel.Meta.ApiTimeStampUTC = DateTime.UtcNow;
            outputModel.InputModel = EPInputModel;
            return outputModel;
            
        }

    }
}