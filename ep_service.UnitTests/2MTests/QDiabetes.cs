namespace ep_service.UnitTests.TwoMillion
{
    [TestFixture]
    public class QDiabetes
    {
        [SetUp]
        public void Setup()
        {

        }

        [Test]
        public void QDiabetes_2M()
        {
            /***************
            1. Arrange
            ***************/
            // Get the 2M row test pack from the test_packs dependency            
            var csv = test_packs.QDiabetes_Resources.TwoMillion;
            // get a referece to the service that will pass the data to the QRisk calculation engine
            var service = new ep_service.PredictionService();

            /***************
              2. Act
             ***************/
            // Read the test file..                        
            // Call the Engine for each row and compare the Engine result to the testfile result
            int rowsProcessed = 0;
            int scoresMatched = 0;
            int scoresFailed = 0;
            int headerRows = 10;

            foreach (var row in csv.Skip(headerRows).TakeWhile(r => r.Length > 1 && r.Last().Trim().Length > 0))
            {
                string rowId = row[0];

                var inputModel = EPInputModel.CreateFromQDiabetesTestPackRow(row);

                var expectedscore = Double.Parse(row[39]);
                var expectedRefscore = Double.Parse(row[38]);

                var serviceResult = service.GetScore(inputModel);
                var engineScores = serviceResult.EngineResults.Where(p => p.EngineName == EPStandardDefinitions.Engines.QDiabetes).Single();
                var patientScore = engineScores.Results.Where(p => p.id.ToString() == Globals.QDiabetesScoreUri).Single().score;
                var referenceScore = engineScores.Results.Where(p => p.id.ToString() == Globals.QDiabetesScoreUri).Single().typicalScore;

                if (patientScore == expectedscore)
                {
                    scoresMatched++;
                }
                else
                {
                    scoresFailed++;
                }

                if (referenceScore == expectedRefscore)
                {
                    scoresMatched++;
                }
                else
                {
                    scoresFailed++;
                }
                rowsProcessed++;
            }

            /***************
              3. Assert
             ***************/
            Assert.IsTrue(rowsProcessed == 2000000);
            Assert.IsTrue(2 * rowsProcessed == (scoresMatched + scoresFailed));
            Assert.IsTrue(2 * rowsProcessed == scoresMatched);
            Assert.IsTrue(scoresFailed == 0);

        }
    }
}