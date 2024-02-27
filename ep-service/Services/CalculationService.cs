// we have to alias the engine DLLs because they all contain things in the same namespace,
// see: https://stackoverflow.com/questions/9194495/type-exists-in-2-assemblies
extern alias qrisk3;
//extern alias qdiab;
//extern alias qfrac;
//extern alias qfracsd; // qfracture has a different DLL for ther StandardDefns, the other two don't!

using Core;
using ep_models;
using System;

namespace ep_service
{
    
    internal class CalculationService
    {
        internal void PerformCalculations(            
                            EPInputModel inputModel,                                                         
                            out PredictionModel predictionModel
                            )
        {
            predictionModel = new PredictionModel();
            
            if (inputModel.requestedEngines.Contains(EPStandardDefinitions.Engines.QRisk3))
            {
                var calc = new qrisk3::QRISK3Engine.QRiskCVDAlgorithmCalculator("", "");

                // SBP: If provided we use the mean and StDev, If not we calculate it from the list of SBPs
                Double? meanSBP = inputModel.systolicBloodPressureMean;
                Double? stDev = inputModel.systolicBloodPressureStDev;
                
                //var SBPListQuality = new DataQuality();
                //if (!meanSBP.HasValue && inputModel.systolicBloodPressures != null)
                //{
                //    meanSBP = inputModel.systolicBloodPressures.Average();
                //    stDev = Core.Statistics.StandardDeviation(inputModel.systolicBloodPressures);                    
                //    // we have performed a SBP calculation, we need to tell the user about this in the response.
                //    // use the DataQuality object for this                
                //    SBPListQuality.Parameter = "systolicBloodPressures";
                //    SBPListQuality.Quality = ParameterQuality.OK;
                //    SBPListQuality.SubstituteValue = "Using list of SBP readings to create values: systolicBloodPressureMean=" + meanSBP + ", and systolicBloodPressureStDev=" + stDev;
                //    //performedSBPCalc = true;
                //    //meta.PerformedSystolicBloodPressureCalc = true;
                //}

                var calcInputModel = new QRisk3InputModel();
                InputMapper.MapServiceInputToCalculatorInput<QRisk3InputModel>(inputModel, ref calcInputModel);
                
                var calcResult = calc.calculate(
                                    b_cvd: calcInputModel.CVD,
                                    sex: (qrisk3::CRStandardDefinitions.Gender)calcInputModel.sex,
                                    age: calcInputModel.age,
                                    b_AF: calcInputModel.atrialFibrillation,
                                    b_atypicalantipsy: calcInputModel.atypicalAntipsychoticMedication,
                                    b_corticosteroids: calcInputModel.systemicCorticosteroids,
                                    b_impotence2: calcInputModel.impotence,
                                    b_migraine: calcInputModel.migraines,
                                    b_ra: calcInputModel.rheumatoidArthritis,
                                    b_renal: calcInputModel.chronicRenalDisease,
                                    b_semi: calcInputModel.severeMentalIllness,
                                    b_sle: calcInputModel.systemicLupusErythematosus,
                                    b_treatedhyp: calcInputModel.bloodPressureTreatment,
                                    diabetes_cat: (qrisk3::CRStandardDefinitions.DiabetesCat)calcInputModel.diabetesStatus,
                                    bmi: calcInputModel.BMI,
                                    ethnicity: (qrisk3::CRStandardDefinitions.Ethnicity)calcInputModel.ethnicity,
                                    fh_cvd: calcInputModel.familyHistoryCHD,
                                    rati: calcInputModel.cholesterolRatio,
                                    sbp: meanSBP,
                                    sbps5: stDev,
                                    smoke_cat: (qrisk3::CRStandardDefinitions.SmokeCat)calcInputModel.smokingStatus,
                                    town: calcInputModel.townsendScore
                                    );

                predictionModel.EngineResults.Add( new EngineResultModel(calcResult, calcInputModel));
            }


            //if (inputModel.requestedEngines.Contains(EPStandardDefinitions.Engines.QDiabetes.ToString()))
            //{
            //    var calc = new qdiab::QDiabetesEngine.QDiabetesAlgorithmCalculator("", "");

            //    var calcInputModel = new QDiabetesInputModel();
            //    InputMapper.MapApiInputToCalculatorInput(inputModel, ref calcInputModel);

            //    var calcResult = calc.calculate(
            //                        diabetes_cat: (qdiab::CRStandardDefinitions.DiabetesCat)calcInputModel.diabetesStatus,
            //                        sex: (qdiab::CRStandardDefinitions.Gender)calcInputModel.sex,
            //                        age: calcInputModel.age,
            //                        b_atypicalantipsy: calcInputModel.atypicalAntipsychoticMedication,
            //                        b_corticosteroids: calcInputModel.systemicCorticosteroids,
            //                        b_cvd: calcInputModel.CVD,
            //                        b_gestdiab: calcInputModel.gestationalDiabetes,
            //                        b_learning: calcInputModel.learningDisabilities,
            //                        b_manicschiz: calcInputModel.manicDepressionSchizophrenia,
            //                        b_pos: calcInputModel.polycysticOvaries,
            //                        b_statin: calcInputModel.statins,
            //                        b_treatedhyp: calcInputModel.bloodPressureTreatment,
            //                        bmi: calcInputModel.BMI,
            //                        ethnicity: (qdiab::CRStandardDefinitions.Ethnicity)calcInputModel.ethnicity,
            //                        fbs: calcInputModel.fastingBloodGlucose,
            //                        fh_diab: calcInputModel.familyHistoryDiabetes,
            //                        hba1c: calcInputModel.hba1c,
            //                        smoke_cat: (qdiab::CRStandardDefinitions.SmokeCat)calcInputModel.smokingStatus,
            //                        town: calcInputModel.townsendScore
            //                        );
                
            //    predictionModel.EngineResults.Add(new EngineResultModel(calcResult, calcInputModel));
            //}


            //if (inputModel.requestedEngines.Contains(EPStandardDefinitions.Engines.QFracture.ToString()))
            //{
            //    var calc = new qfrac::QFractureEngine.QFractureAlgorithmCalculator("", "");

            //    var calcInputModel = new QFractureInputModel();
            //    InputMapper.MapApiInputToCalculatorInput(inputModel, ref calcInputModel);

            //    var calcResult = calc.calculate(
            //                       sex: (qfracsd::CRStandardDefinitions.Gender)calcInputModel.sex,
            //                            age: calcInputModel.age,
            //                            alcoholCat6: (qfracsd::CRStandardDefinitions.AlcoholCat6)calcInputModel.alcoholStatus,
            //                            b_antidepressant: calcInputModel.takingAntidepressants,
            //                            b_anycancer: calcInputModel.anyCancer,
            //                            b_asthmacopd: calcInputModel.asthmaOrCOPD,
            //                            b_carehome: calcInputModel.livingInCareHome,
            //                            b_corticosteroids: calcInputModel.systemicCorticosteroids,
            //                            b_cvd: calcInputModel.CVD,
            //                            b_dementia: calcInputModel.dementia,
            //                            b_endocrine: calcInputModel.endocrineProblems,
            //                            b_epilepsy2: calcInputModel.epilepsyOrAnticonvulsants,
            //                            b_falls: calcInputModel.historyOfFalls,
            //                            b_fracture4: calcInputModel.wristSpineHipShoulderFracture,
            //                            b_hrt_oest: calcInputModel.takingOestrogenHRT,
            //                            b_liver: calcInputModel.chronicLiverDisease,
            //                            b_malabsorption: calcInputModel.malabsorption,
            //                            b_parkinsons: calcInputModel.parkinsonsDisease,
            //                            b_ra_sle: calcInputModel.rheumatoidArthritisOrSLE,
            //                            b_renal: calcInputModel.chronicRenalDisease,
            //                            bmi: calcInputModel.BMI,
            //                            diabetes_cat: (qfracsd::CRStandardDefinitions.DiabetesCat)calcInputModel.diabetesStatus,
            //                            ethnicity: (qfracsd::CRStandardDefinitions.Ethnicity)calcInputModel.ethnicity,
            //                            fh_osteoporosis: calcInputModel.familyHistoryOsteoporosis,
            //                            smoke_cat: (qfracsd::CRStandardDefinitions.SmokeCat)calcInputModel.smokingStatus,
            //                            surv: calcInputModel.predictionYears
            //                        );

            //    predictionModel.EngineResults.Add(new EngineResultModel(calcResult, calcInputModel));
            //}

        }

      
    }
}
