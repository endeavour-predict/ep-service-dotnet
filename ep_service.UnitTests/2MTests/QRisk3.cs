namespace ep_service.UnitTests.TwoMillion
{
    [TestFixture]
    public class QRisk3
    {
        [SetUp]
        public void Setup()
        {
            
        }

        [Test]
        public void QRisk3_2M()
        {
            /***************
            1. Arrange
            ***************/
            // Get the 2M row test pack from the test_packs dependency            
            var csv = test_packs.QRisk3_Resources.TwoMillion;
            // get a referece to the service that will pass the data to the QRisk calculation engine
            var service = new ep_service.PredictionService();

            /***************
              2. Act
             ***************/

            // Read the test file..            
            // Call the Engine for each row and compare the Engine result to the testfile result
            int rowsProcessed = 0;
            int rowsMatched = 0;
            int heartAgeRowsMatched = 0;
            int rowsFailed = 0;            

            int headerRows = 1;
            foreach (var row in csv.Skip(headerRows)
                .TakeWhile(r => r.Length > 1 && r.Last().Trim().Length > 0))
            {                
                string rowId = row[0];
                var inputModel = EPInputModel.CreateFromQRisk3TestPackRow(row);
                var expectedscore= Double.Parse(row[42]);
                string expectedHeartAge = row[43];
                var serviceResult = service.GetScore(inputModel);
                var engineScores = serviceResult.EngineResults.Where(p=>p.EngineName == EPStandardDefinitions.Engines.QRisk3).Single();
                var QRisk3Score = engineScores.Results.Where(p=>p.id.ToString() == Globals.QRiskScoreUri).Single();
                var QRisk3HeartAgeScore = engineScores.Results.Where(p => p.id.ToString() == Globals.QRiskScoreUri+ "HeartAge").SingleOrDefault();

                // QHeartAge is not always given, in these cases the engine is telling us "over 84" and we must match that with the expected result
                if (QRisk3HeartAgeScore == null)
                {
                    if (expectedHeartAge == "over 84")
                    {
                        heartAgeRowsMatched++;
                    }
                }
                else 
                {
                    if (QRisk3HeartAgeScore.score == Double.Parse(expectedHeartAge))
                    {
                        heartAgeRowsMatched++;
                    }
                }
                
                if (QRisk3Score.score == expectedscore)
                {
                    rowsMatched++;
                }
                else
                {
                    rowsFailed++;                                        
                }
                
                rowsProcessed++;
            }


            /***************
              3. Assert
             ***************/
            Assert.IsTrue(rowsProcessed == 2000000);
            Assert.IsTrue(rowsProcessed == (rowsMatched + rowsFailed));
            Assert.IsTrue(rowsMatched == rowsProcessed);
            Assert.IsTrue(rowsFailed == 0);
        }
    }
}