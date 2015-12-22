using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DataCollector.Test.GreenhouseObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace DataCollector.Test
{
    /// <summary>
    /// Except for the test methods to test the public method GetItemTypes() and GetSubItemTypes, all
    /// the other test methods are integration tests to test the ability to connect to Greenhouse Harvest
    /// API to collect the following data:
    /// * Collections of Jobs, Candidates, Applications, Users
    /// * Individual Job, Candidate, Application, and User data from their respective Ids
    /// * Collection of Stages of a Job identified by a given JobId
    /// In addition, it also tests faulty cases such as when an invalid job Id is passsed, or when an
    /// invalid item type is given to query the Greenhouse API.
    /// 
    /// NOTE:
    /// It makes little sense to write pure unit tests and mock the Http client as both GetItems and GetItem
    /// don't have any logic worths testing.
    /// </summary>
    [TestClass]
    public class GreenhouseDataCollectorTest
    {
        private IDataCollector _dataCollector;
        private readonly List<string> _validDataTypes = new List<string>
            {
                "jobs",
                "candidates",
                "applications",
                "users"
            };

        private readonly List<string> _validSubDataTypes = new List<string>
            {
                "stages"
            };

        [TestInitialize]
        public void TestInit()
        {
            _dataCollector = DataCollectorFactory.CreateDataCollector("greenhouse");
        }

        [TestMethod]
        public void GreenhouseDataCollector_GetItemTypes_ReturnValidDataTypes()
        {
            var dataTypes = _dataCollector.GetItemTypes();
            dataTypes.ToList()
                .ForEach(dataType => Assert.IsTrue(_validDataTypes.Any(validDataType => validDataType == dataType)));
        }

        [TestMethod]
        [ExpectedException(typeof (InvalidDataException))]
        public void GreenhouseDataCollector_GetItems_InvalidDataType()
        {
            var data = _dataCollector.GetItems("departments");
        }

        [TestMethod]
        public void GreenhouseDataCollector_GetSubItemTypes_ReturnValidDataTypes()
        {
            var dataTypes = _dataCollector.GetSubItemTypes();
            dataTypes.ToList()
                .ForEach(dataType => Assert.IsTrue(_validSubDataTypes.Any(validDataType => validDataType == dataType)));
        }

        [TestMethod]
        public void GreenhouseDataCollecctor_GetItems_JobsReturnJobCollection()
        {
            var jobsInJson = _dataCollector.GetItems("jobs");
            Assert.IsNotNull(jobsInJson);
            var jobs = JsonConvert.DeserializeObject<List<Job>>(jobsInJson);
            Assert.IsTrue(jobs.Count >= 0);
        }

        [TestMethod]
        public void GreenhouseDataCollecctor_GetItem_JobIdReturnJobWithSameJobId()
        {
            var jobsInJson = _dataCollector.GetItems("jobs");
            Assert.IsNotNull(jobsInJson);
            var jobs = JsonConvert.DeserializeObject<List<Job>>(jobsInJson);
            Assert.IsTrue(jobs.Count > 0);
            var jobId = jobs[0].Id;
            var singleJobInJson = _dataCollector.GetItem("jobs", jobId);
            var job = JsonConvert.DeserializeObject<Job>(singleJobInJson);
            Assert.AreEqual(jobId, job.Id);
        }

        [TestMethod]
        public void GreenhouseDataCollecctor_GetItem_InvalidJobIdReturnException()
        {
            var invalidJobId = "999FF";
            try
            {
                var singleJobInJson = _dataCollector.GetItem("jobs", invalidJobId);
                var job = JsonConvert.DeserializeObject<Job>(singleJobInJson);
            }
            catch (Exception e)
            {
                Assert.IsTrue(e.Message.Contains("Failed to get data from Greenhouse API"));
            }
        }

        [TestMethod]
        public void GreenhouseDataCollecctor_GetItems_CandidatesReturnCandidateCollection()
        {
            var candidatesInJson = _dataCollector.GetItems("candidates");
            Assert.IsNotNull(candidatesInJson);
            var candidates = JsonConvert.DeserializeObject<List<Candidate>>(candidatesInJson);
            Assert.IsTrue(candidates.Count >= 0);
        }

        [TestMethod]
        public void GreenhouseDataCollecctor_GetItem_CandidateIdReturnCandidateWithSameCandidateId()
        {
            var candidatesInJson = _dataCollector.GetItems("candidates");
            Assert.IsNotNull(candidatesInJson);
            var candidates = JsonConvert.DeserializeObject<List<Candidate>>(candidatesInJson);
            Assert.IsTrue(candidates.Count > 0);
            var candidateId = candidates[0].Id;
            var singleCandidateInJson = _dataCollector.GetItem("candidates", candidateId);
            var candidate = JsonConvert.DeserializeObject<Candidate>(singleCandidateInJson);
            Assert.AreEqual(candidateId, candidate.Id);
        }

        [TestMethod]
        public void GreenhouseDataCollecctor_GetItems_ApplicationsReturnApplicationCollection()
        {
            var applicationsInJson = _dataCollector.GetItems("applications");
            Assert.IsNotNull(applicationsInJson);
            var applications = JsonConvert.DeserializeObject<List<Application>>(applicationsInJson);
            Assert.IsTrue(applications.Count >= 0);
        }

        [TestMethod]
        public void GreenhouseDataCollecctor_GetItem_ApplicationIdReturnApplicationWithSameApplicationId()
        {
            var applicationsInJson = _dataCollector.GetItems("applications");
            Assert.IsNotNull(applicationsInJson);
            var applications = JsonConvert.DeserializeObject<List<Application>>(applicationsInJson);
            Assert.IsTrue(applications.Count > 0);
            var applicationId = applications[0].Id;
            var singleApplicationInJson = _dataCollector.GetItem("applications", applicationId);
            var application = JsonConvert.DeserializeObject<Application>(singleApplicationInJson);
            Assert.AreEqual(applicationId, application.Id);
        }

        [TestMethod]
        public void GreenhouseDataCollecctor_GetItems_UsersReturnUserCollection()
        {
            var usersInJson = _dataCollector.GetItems("users");
            Assert.IsNotNull(usersInJson);
            var users = JsonConvert.DeserializeObject<List<User>>(usersInJson);
            Assert.IsTrue(usersInJson.StartsWith("[") && usersInJson.EndsWith("]"));
        }

        [TestMethod]
        public void GreenhouseDataCollecctor_GetItem_UserIdReturnUserWithSameUserId()
        {
            var usersInJson = _dataCollector.GetItems("users");
            Assert.IsNotNull(usersInJson);
            var users = JsonConvert.DeserializeObject<List<User>>(usersInJson);
            Assert.IsTrue(users.Count > 0);
            var userId = users[0].Id;
            var singleUserInJson = _dataCollector.GetItem("users", userId);
            var user = JsonConvert.DeserializeObject<Application>(singleUserInJson);
            Assert.AreEqual(userId, user.Id);
        }

        [TestMethod]
        public void GreenhouseDataCollecctor_GetItem_JobStagesReturnStageCollectionForTheJob()
        {
            var jobsInJson = _dataCollector.GetItems("jobs");
            Assert.IsNotNull(jobsInJson);
            var jobs = JsonConvert.DeserializeObject<List<Job>>(jobsInJson);
            Assert.IsTrue(jobs.Count > 0);
            var jobId = jobs[0].Id;
            var jobStagesInJson = _dataCollector.GetSubItems("jobs", jobId, "stages");
            var jobStages = JsonConvert.DeserializeObject<List<Stage>>(jobStagesInJson);
            Assert.IsTrue(jobStages.Count > 0);
        }

        [TestMethod]
        public void GreenhouseDataCollecctor_GetItem_JobInvalidSubItemTypeReturnException()
        {
            var jobsInJson = _dataCollector.GetItems("jobs");
            Assert.IsNotNull(jobsInJson);
            var jobs = JsonConvert.DeserializeObject<List<Job>>(jobsInJson);
            Assert.IsTrue(jobs.Count > 0);
            var jobId = jobs[0].Id;
            try
            {
                var invalidSubItems = "steps";
                var jobStagesInJson = _dataCollector.GetSubItems("jobs", jobId, invalidSubItems);
                Assert.Fail();  //it shouldn't get here!
            }
            catch (Exception e)
            {
                Assert.IsTrue(e.Message.Contains("Failed to get data from Greenhouse API"));
            }
        }
    }
}
