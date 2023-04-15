using GameStateTesting.Story;
using GameStateTesting.Utilities;

namespace GameStateTesting.Test
{
    public class GetJsonFromFileTest
    {
        [Fact]
        public void TestJsonfileLocationEmptyShouldReturnException()
        /*If a json is empty and runs through JsonUtilitye regardless if it is Part1, Part2, or Part 3
         * it will test to see if it is empty. Not sure If I want to keep it because it doesn't 
         tell me which one would fail when it goes through the jsonUtility. Also If new files run through
        this in the future they can be caught here if empty. I think 2 more files will be added and will
        be caught by this*/
        {
            //arrange  (I am going to get all the stuff together for the test)
            string jsonfileLocation = "TestJsonFiles/IsEmpty.json";
            //This should test for any file that is empty make sure that they throw an exception if empty
            //act   (I am going to call the fuctions that run the test)
            Assert.Throws<Exception>(() => JsonUtility.GetJsonStringMessageFromJSON(jsonfileLocation));
            //assert (I am going to validate the my expected results are correct
        }
        /*The 3 below may technically be redundant since the first one tests any file that
        that goes through the JsonUtility but I wanted to make sure these specific files are not empty
        since they house the story and are very important. This way if the above one fails and this doesn't
        I know that it is not this file that may be blank Since I have a couple files using it.*/
        [Fact]
        public void TestJsonPart1EmptyShouldReturnException()
        {
            //arrange  (I am going to get all the stuff together for the test)
            string jsonfileLocation = "Story/Part1.json";
            //act   (I am going to call the fuctions that run the test)
            var actualBeginingMessage = JsonUtility.GetJsonStringMessageFromJSON(jsonfileLocation);
            //assert (I am going to validate the my expected results are correct
            Assert.NotEmpty(actualBeginingMessage);
        }
        [Fact]
        public void TestJsonPart2EmptyShouldReturnException()
        {
            //arrange  (I am going to get all the stuff together for the test)
            string jsonfileLocation = "Story/Part2.json";
            //act   (I am going to call the fuctions that run the test)
            var actualBeginingMessage = JsonUtility.GetJsonStringMessageFromJSON(jsonfileLocation);
            //assert (I am going to validate the my expected results are correct
            Assert.NotEmpty(actualBeginingMessage);
        }
        [Fact]
        public void TestJsonPart3EmptyShouldReturnException()
        {
            //arrange  (I am going to get all the stuff together for the test)
            string jsonfileLocation = "Story/Part3.json";
            //act   (I am going to call the fuctions that run the test)
            var actualBeginingMessage = JsonUtility.GetJsonStringMessageFromJSON(jsonfileLocation);
            //assert (I am going to validate the my expected results are correct
            Assert.NotEmpty(actualBeginingMessage);
        }
        [Fact]
        public void TestJsonFileFirstMessageIsCorrect()
        {  //Checking to make sure we are starting at the correct place when we beging the story.
            //arrange  (I am going to get all the stuff together for the test)
            string jsonfileLocation = "Story/Part1.json";
            List<Message> expectedBeginingMessage = new List<Message>();
            expectedBeginingMessage.Add(new Message { Story = "A Grand adventurer makes their arrival." }); // the incorrect message example
            //act   (I am going to call the fuctions that run the test)
            var actualBeginingMessage = JsonUtility.GetJsonStringMessageFromJSON(jsonfileLocation);
            //assert (I am going to validate the my expected results are correct
            Assert.Equal(expectedBeginingMessage.First().Story, actualBeginingMessage!.First().Story); ///! means ignore nullable warning
        }
        [Fact]
        public void TestFirstJsonFileForCorrectObjects()
        /*Needs to be 3!!! If not 3 then wrong file or something may have happened to the file.*/
        {
            //arrange  (I am going to get all the stuff together for the test)
            int NumofCorrectObjectsAllowed = 3;
            string jsonfileLocation = "Story/Part1.json";
            //act   (I am going to call the fuctions that run the test)
            var GettingTheListFromJson = JsonUtility.GetJsonStringMessageFromJSON(jsonfileLocation);
            int countOfTheList = GettingTheListFromJson.Count();
            //assert (I am going to validate the my expected results are correct
            Assert.Equal(countOfTheList, NumofCorrectObjectsAllowed);
        }
        [Fact]
        public void TestSecondJsonFileForCorrectObjects()
        /*Needs to be 12 Exactly!!! If not 12 then wrong file or something may have happened to the file.*/
        {
            //arrange  (I am going to get all the stuff together for the test)
            int NumofCorrectObjectsAllowed = 12;
            string jsonfileLocation = "Story/Part2.json";
            //act   (I am going to call the fuctions that run the test)
            var GettingTheListFromJson = JsonUtility.GetJsonStringMessageFromJSON(jsonfileLocation);
            int countOfTheList = GettingTheListFromJson.Count();
            //assert (I am going to validate the my expected results are correct
            Assert.Equal(countOfTheList, NumofCorrectObjectsAllowed);
        }
        [Fact]
        public void TestThirdJsonFileForCorrectObjects()
        /*Needs to be 48 Exactly!!! If not 48 then wrong file or something may have happened to the file.*/
        {
            //arrange  (I am going to get all the stuff together for the test)
            int NumofCorrectObjectsAllowed = 48;
            string jsonfileLocation = "Story/Part3.json";
            //act   (I am going to call the fuctions that run the test)
            var GettingTheListFromJson = JsonUtility.GetJsonStringMessageFromJSON(jsonfileLocation);
            int countOfTheList = GettingTheListFromJson.Count();
            //assert (I am going to validate the my expected results are correct
            Assert.Equal(countOfTheList, NumofCorrectObjectsAllowed);
        }
    }
}