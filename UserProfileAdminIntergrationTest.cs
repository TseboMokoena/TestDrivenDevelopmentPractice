using System;
using System.ComponentModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Unity;
using UserProfileManagement;
using UserProfileManagement.Data;
using AudienceProfiling.Managers;
using System.Collections.Generic;
using System.Linq;

namespace UserProfileManagementIntergrationTests
{
    [TestClass]
    public class UserProfileAdminIntergrationTest
    {
        private readonly UserProfilesAdminApi _adminApi;
        private UserProfilesAdminApi AdminApi => _adminApi ?? new UserProfilesAdminApi();
        private Guid id = new Guid("F39EECB7-F66C-47A0-8B2E-1C5A6C7E14E6");
        private CxenseSegment segment;

        private UserProfilesAdminApi _userProfilesAdminApi;
        private UserProfilesAdminApi UserProfilesAdminApi => _userProfilesAdminApi ?? (_userProfilesAdminApi = new UserProfilesAdminApi());
        private IEnumerable<CxenseSegmentImport> SegmentsToImport => _userProfilesAdminApi.GetAllCxenseSegmentToProcess().Where(x => !string.IsNullOrWhiteSpace(x.AssociatedFileName));
        
        [TestMethod]
        public void PopulateDashboardData_CheckingIfDashboardDataIsWritenToDatabase_AddsDashboardDataToDatabase()
        {
            //arrange
           segment = AdminApi.GetCxenseSegmentsById(id);

            var dashboardData = new CxenseProcessedSegment
            {
                SegmentName = "TestSegment2",
                ProcessedRecords = 5,
                MatchedEmails = 3,
                UnMatchedEmails = 2,
                DateProcessed = DateTime.Now,
                DownloadLink = @"\\24cpt-devweb03\Backend\Segment\tesingcs636559235553038989v.csv"
            };
          
                 //act
            int result = AdminApi.PopulateDashboardData(dashboardData);

            //assert
             Assert.IsNotNull(result);
        }

        [TestMethod]
        public void ImportCxenseSegments_CheckingIfNewSegmentsAreBeingImportedAndProcessed_ProcessesSegmentAndAddsDashboardDataToDatabase()
        {
            //arrange
            var apManager = new AudienceProfilingManager();

            //act
            int result = apManager.ImportCxenseSegments();

            //assert
            Assert.AreEqual(1, result); 
        }

        [TestMethod]
        public void InsertCxenseUserSegment_CheckToSeeIfDataISBeingPersistedToDatabase_AddIntoDataBase()
        {
            //arrange
            string hash = "00001c00eaf00ef00e0c0a0c000a0f00e00d00e0";
            Guid id = new Guid ("000EECB1-F00C-00A0-0B0E-0C0A0C0E00E0"); 
            

            var userSegment = new CxenseUserSegment
            {
                 EmailHash = hash,  
                 SegmentId = id
            };

            //act
            int result = AdminApi.InsertCxenseUserSegment(userSegment);

            //assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void GetUserIdBySegmentId_ChekRetunedUserId_ReturnGuid()
        {
            Guid segmentId = new Guid("A7E9A27E-5812-4710-9193-F77F82E7DCD8");
            CxenseSegmentAudit result = AdminApi.GetUserIdBySegmentId(segmentId);

            System.Console.WriteLine(result.ErapUserId);

            Assert.IsNotNull(result.ErapUserId);
        }
    }
}
