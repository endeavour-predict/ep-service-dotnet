using Core;
using System.ComponentModel.DataAnnotations;
using static Core.EPStandardDefinitions;

namespace ep_service.Models
{


    /// <remarks>
    /// Strict SNOMED definitions for all the below parameters are maintained by Endeavour Predict
    /// Using the definitions will ensure the highest possible data quality and consistency being fed to the API
    /// Please consult our website for further details on the definitions, which are also available in 
    /// alternative terminologies.
    /// In Attended Mode, clinicians may use their clinical judgement to over-ride automated searches, 
    /// however this may change the outcome of the risk estimation, and the user assumes all risk for this.    
    /// </remarks>        
    public class EPInputModel
    {


        /// <summary>
        /// Create an Input model from a row of "Test Pack" data
        /// </summary>
        /// <param name="row">Row of data from an Oxford Test Pack</param>
        public static EPInputModel CreateFromQRisk3TestPackRow(string[] row)
        {       
            var epInputModel = new EPInputModel();
            epInputModel.requestedEngines.Add(EPStandardDefinitions.Engines.QRisk3);
            epInputModel.CVD = row[1] == "1";
            if (row[2] == "0") epInputModel.sex = Gender.Female;
            if (row[2] == "1") epInputModel.sex = Gender.Male;
            epInputModel.age = Int32.Parse(row[3]);
            epInputModel.atrialFibrillation = row[4] == "1";
            epInputModel.atypicalAntipsychoticMedication = row[5] == "1";
            epInputModel.systemicCorticosteroids = row[6] == "1";
            epInputModel.impotence = row[7] == "1";
            epInputModel.migraines = row[8] == "1";
            epInputModel.rheumatoidArthritis = row[9] == "1";
            epInputModel.chronicRenalDisease = row[10] == "1";
            epInputModel.severeMentalIllness = row[11] == "1";
            epInputModel.systemicLupusErythematosus = row[12] == "1";
            epInputModel.bloodPressureTreatment = row[13] == "1";
            epInputModel.diabetesStatus = (DiabetesCat)Enum.Parse(typeof(DiabetesCat), row[14]);
            epInputModel.BMI = Double.Parse(row[15]);
            epInputModel.ethnicity = (Ethnicity)Enum.Parse(typeof(Ethnicity), row[16]);
            epInputModel.familyHistoryCHD = row[17] == "1";
            epInputModel.cholesterolRatio = Double.Parse(row[18]);
            epInputModel.systolicBloodPressureMean = Double.Parse(row[19]);
            epInputModel.systolicBloodPressureStDev = Double.Parse(row[20]);
            epInputModel.smokingStatus = (SmokeCat)Enum.Parse(typeof(SmokeCat), row[21]);
            epInputModel.townsendScore = Double.Parse(row[22]);
            return epInputModel;
        }



        /// <summary>
        /// Names of the score engines that you want to invoke in this request (available engines and their details are on the endpoint GET /AvailableScores)
        /// </summary>        
        /// <example>["QRisk3", "QDiabetes"]</example>
        [Required]
        public List<EPStandardDefinitions.Engines> requestedEngines { get; set; } = new();

        /// <summary>
        /// Assigned sex at birth.
        /// </summary>        
        /// <example>Female</example>
        [Required]
        public Core.EPStandardDefinitions.Gender sex { get; set; }

        /// <summary>
        /// Patients age in years calculated on the search date.
        /// </summary>
        ///  <example>45</example>
        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Please enter valid positive number.")]
        public int age { get; set; }

        /// <summary>
        /// Patient has a diagnosis of CVD recorded at any time prior to the search date.
        /// </summary>        
        /// <example>false</example>
        public bool CVD { get; set; }


        /// <summary>
        /// Atrial fibrillation at any time prior to the search date.
        /// </summary>
        /// <example>false</example>
        
        public bool atrialFibrillation { get; set; }

        /// <summary>
        /// On atypical antipsychotic medication?
        /// Second generation ‘atypical’ antipsychotic - prescribed two or more issues in the previous 6 months(includes amisulpride, aripiprazole, clozapine, lurasidone, olanzapine, paliperidone, quetiapine, risperidone, sertindole, or zotepine).
        /// </summary>
        /// <example>false</example>
        public bool atypicalAntipsychoticMedication { get; set; }

        /// <summary>
        /// On regular steroid tablets?	
        /// Systemic corticosteroids – prescribed two or more issues in the previous 6 months.
        /// </summary>
        /// <example>false</example>
        public bool systemicCorticosteroids { get; set; }

        /// <summary>
        /// On blood pressure treatment?
        /// Diagnosis of hypertension at any time in the patient’s records AND On antihypertensive treatment if 1+ script in last 6 months prior to the search date.
        /// Antihypertensive treatment includes the following: 
        /// Thiazides, Beta blockers, ACE inhibitors, Angiotensin II Antagonists, Calcium Channel Blockers.
        /// Patients should only be included if both criteria are satisfied i.e.on treatment and have a diagnosis of hypertension.
        /// </summary>
        /// <example>false</example>
        public bool bloodPressureTreatment { get; set; }


        /// <summary>
        /// A diagnosis of, or treatment for, erectile dysfunction,at any time prior to the search date?	
        /// </summary>
        /// <example>false</example>
        public bool impotence { get; set; }

        /// <summary>        
        /// Diagnosis of migraine at any time prior to the search date?
        /// </summary>
        /// <example>false</example>
        public bool migraines { get; set; }

        /// <summary>
        ///  Rheumatoid arthritis at any time prior to the search date?
        /// </summary>
        /// <example>false</example>
        public bool rheumatoidArthritis { get; set; }

        /// <summary>
        /// Chronic renal disease at any time prior to the search date?
        /// </summary>
        /// <example>false</example>
        public bool chronicRenalDisease { get; set; }

        /// <summary>
        /// Diagnosis of severe mental illness (psychosis, severe depression, manic depression, schizophrenia) at any time prior to the search date?
        /// </summary>
        /// <example>false</example>
        public bool severeMentalIllness { get; set; }

        /// <summary>
        /// Diagnosis of systemic lupus erythematosus (SLE) at any time prior to the search date?
        /// </summary>
        /// <example>false</example>
        public bool systemicLupusErythematosus { get; set; }

        /// <summary>
        /// Gestational diabetes (i.e. diabetes that arose during pregnancy)?
        /// </summary>
        /// <example>false</example>
        public bool gestationalDiabetes{ get; set; }

        /// <summary>
        /// Learning disabilities?
        /// </summary>
        /// <example>false</example>
        public bool learningDisabilities { get; set; }

        /// <summary>
        /// Manic depression or schizophrenia?
        /// </summary>
        /// <example>false</example>
        public bool manicDepressionSchizophrenia { get; set; }

        /// <summary>
        /// Polycystic ovaries?
        /// </summary>
        public bool polycysticOvaries { get; set; }

        /// <summary>
        /// On statins?
        /// </summary>
        public bool statins { get; set; }
        
        /// <summary>
        /// Do immediate family (mother, father, brothers or sisters) have diabetes?
        /// </summary>
        public bool familyHistoryDiabetes { get; set; }
        
        /// <summary>
        /// Fasting blood glucose (mmol/l)
        /// </summary>
        public double? fastingBloodGlucose { get; set; }
        
        /// <summary>
        /// HBA1c(mmol/mol)
        /// </summary>
        public double? hba1c { get; set; }


        /// <summary>
        /// Family history of coronary heart disease in a first degree relative under the age of 60 years recorded before the search date?
        /// </summary>
        /// <example>false</example>
        public bool familyHistoryCHD { get; set; }

        /// <summary>
        /// Cholesterol/HDL ratio.
        /// Acceptable/Credible Range: 1 to 12 
        /// The most recent ratio of total serum cholesterol/HDL recorded in the last 5 years
        /// </summary>
        /// <example>4.1</example>
        [Range(0, double.MaxValue, ErrorMessage = "Please enter valid positive number.")]
        public double? cholesterolRatio { get; set; }

        /// <summary>
        /// Diabetes status
        /// </summary>
        /// <example>None</example>        
        public Core.EPStandardDefinitions.DiabetesCat diabetesStatus { get; set; }

        /// <summary>
        /// Body Mass Index (kg/m^2).
        /// Acceptable/Credible Range: 18 to 47.
        /// The most recently recorded body mass index for the patient recorded prior to the search date recorded in the last 5 years.
        /// </summary>
        /// <example>23.4</example>
        public double? BMI { get; set; }

        /// <summary>
        /// Ethnic group, chosen from the 17 categories.
        /// </summary>
        /// <example>OtherWhiteBackground</example>
        public Core.EPStandardDefinitions.Ethnicity ethnicity { get; set; }

        /// <summary>
        /// Most recent confirmed smoking status.
        /// </summary>
        /// <example>NonSmoker</example>
        public Core.EPStandardDefinitions.SmokeCat smokingStatus { get; set; }

        /// <summary>
        /// Systolic blood pressure readings (mmHg).
        /// Acceptable/Credible Range: 70 to 210
        /// The most recent systolic blood pressure readings prior to search date recorded in the last 5 years
        /// The order here is important, the readings in the list should be most recent first, ending with the oldest reading
        /// </summary>
        /// <example>[120,130,140]</example>
        public List<Double>? systolicBloodPressures { get; set; }


        /// <summary>
        /// Latest Systolic Blood Pressure mean reading (mmHg). If provided will be used instead of the List of systolicBloodPressures
        /// </summary>
        /// <example>140.4</example>
        [Range(0, double.MaxValue, ErrorMessage = "Please enter valid positive number.")]
        public Double? systolicBloodPressureMean { get; set; }

        /// <summary>
        /// Standard Deviation for the systolicBloodPressureMean value
        /// </summary>
        /// <example>3</example>
        [Range(0, double.MaxValue, ErrorMessage = "Please enter valid positive number.")]
        public Double? systolicBloodPressureStDev { get; set; }

        /// <summary>
        /// Most recent confirmed alcohol status.
        /// </summary>
        /// <example>One_to_two_units_per_day</example>
        public Core.EPStandardDefinitions.AlcoholCat6 alcoholStatus { get; set; }


        /// <summary>
        /// Number of years to calculate risk over, not all Engines support this and will use a default.
        /// </summary>
        /// <example>10</example>
        public int predictionYears { get; set; }

        /// <summary>
        /// Taking Antidepressants?
        /// </summary>
        /// <example>false</example>
        public bool takingAntidepressants { get; set; }

        /// <summary>
        /// Any Cancer?
        /// </summary>
        /// <example>false</example>
        public bool anyCancer { get; set; }

        /// <summary>
        /// Asthma or COPD?
        /// </summary>
        /// <example>false</example>
        public bool asthmaOrCOPD { get; set; }

        /// <summary>
        /// Living in a nursing or care home?
        /// </summary>
        /// <example>false</example>
        public bool livingInCareHome { get; set; }

        /// <summary>
        /// Dementia
        /// </summary>
        /// <example>false</example>
        public bool dementia { get; set; }

        /// <summary>
        /// Endocrine problems eg thyrotoxocosis, hyperparathyroidism, Cushing's syndrome?
        /// </summary>
        /// <example>false</example>
        public bool endocrineProblems { get; set; }

        /// <summary>
        /// Epilepsy or taking anticonvulsants?
        /// </summary>
        /// <example>false</example>
        public bool epilepsyOrAnticonvulsants { get; set; }

        /// <summary>
        /// History of falls?
        /// </summary>
        /// <example>false</example>
        public bool historyOfFalls { get; set; }

        /// <summary>
        /// Had a wrist, spine, hip or shoulder fracture?
        /// </summary>
        /// <example>false</example>
        public bool wristSpineHipShoulderFracture { get; set; }

        /// <summary>
        /// Taking oestrogen only HRT?
        /// </summary>
        /// <example>false</example>
        public bool takingOestrogenHRT { get; set; }

        /// <summary>
        /// Chronic liver disease?
        /// </summary>
        /// <example>false</example>
        public bool chronicLiverDisease { get; set; }

      

        /// <summary>
        /// Malabsorption eg Crohn's disease, ulcerative colitis, coeliac disease, steatorrhea or blind loop syndrome?
        /// </summary>
        /// <example>false</example>
        public bool malabsorption { get; set; }

        /// <summary>
        /// Parkinson's disease?	
        /// </summary>
        /// <example>false</example>
        public bool parkinsonsDisease { get; set; }

        /// <summary>
        /// Rheumatoid arthritis or SLE?
        /// </summary>
        /// <example>false</example>
        public bool rheumatoidArthritisOrSLE { get; set; }

        /// <summary>
        /// Either parent have osteoporosis/hip fracture?	
        /// </summary>
        /// <example>false</example>
        public bool familyHistoryOsteoporosis { get; set; }

        /// <summary>
        /// Townsend score. 
        /// The Townsend score associated with the output area of a patient’s postcode.
        /// See: https://statistics.ukdataservice.ac.uk/dataset/2011-uk-townsend-deprivation-scores#:~:text=The%20Townsend%20Deprivation%20Index%20is,is%20available%20for%20that%20area).
        /// </summary>
        /// <example>0</example>
        public double? townsendScore { get; set; }




    }
}
