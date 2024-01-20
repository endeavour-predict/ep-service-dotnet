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
    /// IMPORTANT: IN ALL SITUATIONS QRISK3 MUST BE CONSIDERED AN *ESTIMATE* OF FUTURE CVD RISK, NOT AN ABSOLUTE
    /// GUARANTEE, AND MUST IN ALL CASES BE INTERPRETED WITH DUE CLINICAL CONSIDERATION TO THE PATIENT'S COMPLETE
    /// CLINICAL SITUATION. QRISK3 IS NOT A REPLACEMENT FOR CLINICAL JUDGEMENT.
    /// </remarks>        
    public class QRisk3InputModel
    {

        
        /// <summary>
        /// Patient has a diagnosis of CVD recorded at any time prior to the search date.
        /// </summary>    
        /// <example>false</example>
        public bool CVD { get; set; }

        /// <summary>
        /// Assigned sex at birth.
        /// </summary>        
        /// <example>Female</example>
        [Required]
        public Core.EPStandardDefinitions.Gender sex { get; set; }

        /// <summary>
        /// Patients age in years calculated on the search date.
        /// </summary>
        /// <example>45</example>
        [Range(0, double.MaxValue, ErrorMessage = "Please enter valid positive number.")]
        [Required]
        public int age { get; set; }

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
        /// On blood pressure treatment?
        /// Diagnosis of hypertension at any time in the patient’s records AND On antihypertensive treatment if 1+ script in last 6 months prior to the search date.
        /// Antihypertensive treatment includes the following: 
        /// Thiazides, Beta blockers, ACE inhibitors, Angiotensin II Antagonists, Calcium Channel Blockers.
        /// Patients should only be included if both criteria are satisfied i.e.on treatment and have a diagnosis of hypertension.
        /// </summary>
        /// <example>false</example>
        public bool bloodPressureTreatment { get; set; }

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
        /// <example>29.1</example>  
        [Range(0, double.MaxValue, ErrorMessage = "Please enter valid positive number.")]
        public double? BMI { get; set; }

        /// <summary>
        /// Ethnic group, chosen from the 17 categories used by QRisk3.
        /// </summary>
        /// <example>OtherWhiteBackground</example>
        public Core.EPStandardDefinitions.Ethnicity ethnicity { get; set; }

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
