namespace ep_service.UnitTests.Parameter
{
    [TestFixture]
    public class QDiabetes
    {
        
        [SetUp]
        public void Setup()
        {            
        }

        [Test]
        public void QDiabetes_FileBasedParamTests()
        {
            var expectedMinNumberOfParamTests = 30;

            var tests = test_packs.QDiabetes_Resources.FileTests;
            var service = new ep_service.PredictionService();

            int testsRun = 0;
            foreach (var test in tests)
            {
                Console.WriteLine(test.TestName + " is running...");
                var actual_serviceResult = service.GetScore(test.EPInputModel);
                var actual_engineScores = actual_serviceResult.EngineResults.Where(p => p.EngineName == EPStandardDefinitions.Engines.QDiabetes).Single();
                var actual_Score = actual_engineScores.Results.Where(p => p.id.ToString() == Globals.QDiabetesScoreUri).Single().score;
                var actual_Reference = actual_engineScores.Results.Where(p => p.id.ToString() == Globals.QDiabetesScoreUri).Single().typicalScore;
                var actual_Meta = actual_engineScores.CalculationMeta;

                var expected_serviceResult = test.PredictionModel;
                var expected_engineScores = expected_serviceResult.EngineResults.Where(p => p.EngineName == EPStandardDefinitions.Engines.QDiabetes).Single();
                var expected_Score = expected_engineScores.Results.Where(p => p.id.ToString() == Globals.QDiabetesScoreUri).Single().score;
                var expected_Reference = expected_engineScores.Results.Where(p => p.id.ToString() == Globals.QDiabetesScoreUri).Single().typicalScore;
                var expected_Meta = expected_engineScores.CalculationMeta;

                // we always get a score, even if it's 0.0
                Assert.That(actual_Score, Is.EqualTo(expected_Score), test.TestName);
                Assert.That(actual_Reference, Is.EqualTo(expected_Reference), test.TestName);

                // Final assertion is that the calc reasons match
                Assert.That(actual_Meta.EngineResultStatus, Is.EqualTo(expected_Meta.EngineResultStatus), test.TestName);
                Assert.That(actual_Meta.EngineResultStatusReason, Is.EqualTo(expected_Meta.EngineResultStatusReason), test.TestName);
                testsRun++;
                Console.WriteLine("OK for test " + test.TestName);
            }
            Assert.That(testsRun, Is.GreaterThanOrEqualTo(expectedMinNumberOfParamTests), "Number of tests");
            Console.WriteLine("Tests Run: " + testsRun + " (expectedMinNumberOfParamTests: " + expectedMinNumberOfParamTests + ")");

        }
    }
}