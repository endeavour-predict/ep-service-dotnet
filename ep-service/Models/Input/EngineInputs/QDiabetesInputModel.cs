using System.ComponentModel.DataAnnotations;

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
    public class QDiabetesInputModel
    {
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
        public int age { get; set; }

        /// <summary>
        /// Patient has a diagnosis of CVD recorded at any time prior to the search date.
        /// </summary>        
        /// <example>false</example>
        public bool CVD { get; set; }

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
        /// Townsend score. 
        /// The Townsend score associated with the output area of a patient’s postcode.
        /// See: https://statistics.ukdataservice.ac.uk/dataset/2011-uk-townsend-deprivation-scores#:~:text=The%20Townsend%20Deprivation%20Index%20is,is%20available%20for%20that%20area).
        /// </summary>
        /// <example>0</example>
        public double? townsendScore { get; set; }

    }
}
