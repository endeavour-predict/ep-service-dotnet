using NUnit.Framework.Internal;

namespace ep_service.UnitTests.Parameter
{
    [TestFixture]
    public class QRisk3
    {
        
        [SetUp]
        public void Setup()
        {            
        }

        [Test]
        public void QRisk3_FileBasedParamTests()
        {
            var expectedMinNumberOfParamTests = 54;

            var tests = test_packs.QRisk3_Resources.FileTests;            
            var service = new ep_service.PredictionService();
            
            int testsRun = 0;
            foreach (var test in tests)
            {
                Console.WriteLine(test.TestName +" is running...");
                var actual_serviceResult = service.GetScore(test.EPInputModel);
                var actual_engineScores = actual_serviceResult.EngineResults.Where(p => p.EngineName == EPStandardDefinitions.Engines.QRisk3).Single();
                var actual_QRisk3Score = actual_engineScores.Results.Where(p => p.id.ToString() == Globals.QRiskScoreUri).Single();
                var actual_QRisk3HeartAgeScore = actual_engineScores.Results.Where(p => p.id.ToString() == Globals.QRiskScoreUri + "HeartAge").SingleOrDefault();
                var actual_Meta = actual_engineScores.CalculationMeta;

                var expected_serviceResult = test.PredictionModel;
                var expected_engineScores = expected_serviceResult.EngineResults.Where(p => p.EngineName == EPStandardDefinitions.Engines.QRisk3).Single();
                var expected_QRisk3Score = expected_engineScores.Results.Where(p => p.id.ToString() == Globals.QRiskScoreUri).Single();
                var expected_QRisk3HeartAgeScore = expected_engineScores.Results.Where(p => p.id.ToString() == Globals.QRiskScoreUri + "HeartAge").SingleOrDefault();
                var expected_Meta = expected_engineScores.CalculationMeta;

                // we always get a score, even if it's 0.0
                Assert.That(actual_QRisk3Score.score, Is.EqualTo(expected_QRisk3Score.score), test.TestName);

                // we don't always get a heart age score (like when CVD = true) so we need to check whether we're expecting one
                if (expected_QRisk3HeartAgeScore != null)
                {
                    Assert.That(actual_QRisk3HeartAgeScore.score, Is.EqualTo(expected_QRisk3HeartAgeScore.score), test.TestName);
                }

                // Final assertion is that the calc reasons match
                Assert.That(actual_Meta.EngineResultStatus, Is.EqualTo(expected_Meta.EngineResultStatus), test.TestName);
                Assert.That(actual_Meta.EngineResultStatusReason, Is.EqualTo(expected_Meta.EngineResultStatusReason), test.TestName);
                testsRun++;
                Console.WriteLine("OK for test " + test.TestName);
            }
            Assert.That(testsRun, Is.GreaterThanOrEqualTo(expectedMinNumberOfParamTests), "Number of tests");
            Console.WriteLine("Tests Run: " + testsRun + " (expectedMinNumberOfParamTests: " + expectedMinNumberOfParamTests+")");

        }
    }
}