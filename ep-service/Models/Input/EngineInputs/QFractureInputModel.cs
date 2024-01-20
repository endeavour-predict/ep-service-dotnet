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
    public class QFractureInputModel
    {
        /// <summary>
        /// Number of years to calculate risk over, not all Engines support this and will use a default.
        /// </summary>
        /// <example>10</example>
        public int predictionYears { get; set; }

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
        /// On regular steroid tablets?	
        /// Systemic corticosteroids – prescribed two or more issues in the previous 6 months.
        /// </summary>
        /// <example>false</example>
        public bool systemicCorticosteroids { get; set; }

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
        /// Most recent confirmed alcohol status.
        /// </summary>
        /// <example>One_to_two_units_per_day</example>
        public Core.EPStandardDefinitions.AlcoholCat6 alcoholStatus { get; set; }

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
        /// Chronic kidney disease (stage 4 or 5)?
        /// </summary>
        /// <example>false</example>
        public bool chronicRenalDisease { get; set; }

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






    }
}
