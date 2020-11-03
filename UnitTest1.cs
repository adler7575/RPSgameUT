using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using RestSharp;
using RPSgame.Models;
using System.Collections.Generic;


namespace RPSgameUT
{
    [TestClass]
    public class RPSgameUT1
    {
        private const string gcBaseURI = "https://localhost:44309/";
        private const string gcAPIURInewgame = "api/RPSGame/newgame";
        private string gcAPIURIjoingame = "api/RPSGame/Id/joingame";
        private string gcAPIURIstatus = "api/RPSGame/Id";
        private string gcAPIURInakemove = "api/RPSGame/Id/makemove";

        [TestMethod]
        public void CreateGame1()
        {
            RestClient RC = new RestClient(gcBaseURI);                       
            RestRequest Request = new RestRequest(gcAPIURInewgame, Method.POST);
            Request.RequestFormat = DataFormat.Json;
            Player P = new Player("Anna");
            Request.AddJsonBody(P);
            RestResponse response = (RestResponse)RC.Execute(Request);
            string content = response.Content;

            //RootObject.List = JsonConvert.DeserializeObject<List<RootObject>>(content);

        }
    }
}
